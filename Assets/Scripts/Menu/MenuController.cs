using UnityEngine;

public class MenuController : MonoBehaviour
{
    [SerializeField] private MenuPage mainPage;
    private MenuPage currentPage;

    private void Start()
    {
        PlayerController.instance.GetInput().Game.Menu.performed += _ => ReturnPage();
    }

    public void ReturnPage()
    {
        if(currentPage == null)
        {
            ToggleMenu();
            return;
        } else
        {
            if (currentPage.GetPageToReturn() == null)
            {
                ToggleMenu();
                currentPage = null;
                return;
            }
        }
        ChangePage(currentPage.GetPageToReturn());
    }

    public void ChangePage(MenuPage newPage)
    {
        if (currentPage != null)
        {
            currentPage.OnPageExit();
            currentPage.GetPageObject().SetActive(false);
        } else
        {
            mainPage.OnPageExit();
            mainPage.GetPageObject().SetActive(false);
        }
        newPage.GetPageObject().SetActive(true);
        currentPage = newPage;
    }

    public virtual void ToggleMenu()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }
}