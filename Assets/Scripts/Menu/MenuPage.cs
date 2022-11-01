using UnityEngine;

public class MenuPage : MonoBehaviour
{
    [SerializeField] private GameObject pageObject;

    [SerializeField] private MenuPage pageToReturn;

    private void Start()
    {
        gameObject.SetActive(false);
    }

    public virtual void OnPageExit()
    {

    }

    public MenuPage GetPageToReturn()
    {
        return pageToReturn;
    }

    public GameObject GetPageObject()
    {
        return pageObject;
    }
}