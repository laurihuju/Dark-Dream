using System.Collections;
using UnityEngine;

public class SavingText : MonoBehaviour
{
    public static SavingText instance;

    [SerializeField] private Animator textAnim;
    [SerializeField] private float textTime;
    [SerializeField] private float timeToWaitBeforeShowing;

    private void Awake()
    {
        if(instance != null)
        {
            Destroy(this.gameObject);
            return;
        }

        instance = this;
    }

    public void ShowText()
    {
        StartCoroutine(ShowTextCo());
    }

    private IEnumerator ShowTextCo()
    {
        yield return new WaitForSecondsRealtime(timeToWaitBeforeShowing);

        textAnim.SetTrigger("fadein");

        yield return new WaitForSecondsRealtime(textTime);

        textAnim.SetTrigger("fadeout");
    }
}
