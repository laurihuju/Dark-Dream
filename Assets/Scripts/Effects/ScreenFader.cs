using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenFader : MonoBehaviour
{
    public static ScreenFader instance;

    [Header("Settings")]

    // Referenssi animaattoriin 
    private Animator anim;

    // Lippu, joka kertoo ett� animaatio on suoritettu loppuun:
    private bool isFading = false;

    private void Awake()
    {
        if(instance != null)
        {
            Destroy(this.gameObject);
            return;
        }

        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        // Otetaan animaattori k�ytt��:
        anim = GetComponent<Animator>();

    }
    /*********************************************************************FadeToClear****************************************************/

    public IEnumerator FadeToClear()
    {
        isFading = true;
        anim.SetTrigger("FadeIn");

        // Silmukkaa suoritetaan niin kauan, kunnes isFading = false:
        while (isFading)
            yield return null;

    }
    /*********************************************************************FadeToBlack****************************************************/

    public IEnumerator FadeToBlack()
    {
        isFading = true;
        anim.SetTrigger("FadeOut");

        // Silmukkaa suoritetaan niin kauan; kunnes isFagin = false 
        while (isFading)
            yield return null;
    }
    // Kertoo alirutiineille, ett� animaatio on suoritettu loppuu:
    void AnimationComplete()
    {
        isFading = false;
    }
}
