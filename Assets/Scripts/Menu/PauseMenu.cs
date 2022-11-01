using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject inventory;
    [SerializeField] private GameObject statusBars;

    private bool isDestroyed;

    private void Start()
    {
        PlayerController.instance.GetInput().Game.Menu.performed += _ => ToggleMenu();

        inventory.SetActive(false);
        statusBars.SetActive(false);
    }

    private void OnDestroy()
    {
        isDestroyed = true;
    }

    private void ToggleMenu()
    {
        if (isDestroyed)
            return;

        inventory.SetActive(!inventory.gameObject.activeSelf);
        statusBars.SetActive(inventory.gameObject.activeSelf);
        if (inventory.gameObject.activeSelf)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }
}
