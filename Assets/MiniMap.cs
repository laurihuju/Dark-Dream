using UnityEngine;
using UnityEngine.UI;

public class MiniMap : MonoBehaviour
{
    //private bool MiniMapIsActive;
    public static MiniMap instance;


    
    [SerializeField] GameObject camera;

    [SerializeField] Animator anim;
    [SerializeField] GameObject Map;
    [SerializeField] RawImage image;
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
            return;
        }

        instance = this;
    }

    private void Start()
    {
        PlayerController.instance.GetInput().Game.MiniMap.performed += _ => ActivateMiniMap();

        Map.SetActive(false);
    }

    public void ActivateMiniMap()
    {
        //Active
        if(!Map.activeSelf && GameManager.instance.GetCurrentMap() != "AnnaHouse" && GameManager.instance.GetCurrentMap() != "FirstMap" && !DialogManager.instance.IsDialogActive())
        {
            image.enabled = true;
            camera.SetActive(true);
            Map.SetActive(true);

            if (InventoryMenu.instance.IsMenuActive())
                InventoryMenu.instance.ForceDeactivateMenu();

            GameManager.instance.SetMiniMapState(true);

            anim.SetTrigger("FadeOut");
        }
        else //Deactive
        {
            anim.SetTrigger("FadeIn");
        }
    }

    /// <summary>
    /// Sammuttaa MiniMapin ja kameran, jotta s‰‰stet‰‰n tehoa.
    /// </summary>
    public void TurnOffMap()
    {
        image.enabled = false;

        camera.SetActive(false);
        Map.SetActive(false);

        GameManager.instance.SetMiniMapState(false);
    }

    public bool IsMapActive()
    {
        return Map.activeSelf;
    }
}
