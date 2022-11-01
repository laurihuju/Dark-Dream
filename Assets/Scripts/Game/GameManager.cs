using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private bool isDialogActive; //Muuttuja, joka kertoo, onko dialogi aktiivinen
    private bool inventoryOpen; //Muuttuja, joka kertoo, onko inventory auki
    private bool isTeleporting; //Muuttuja, joka kertoo, onko inventory auki
    private bool isMiniMapOpen; //Muuttuja, joka kertoo, onko minikartta auki

    [SerializeField] private Map[] maps; //Taulukko mappien tiedot sis‰lt‰vist‰ luokista
    [SerializeField] private string currentMap = ""; //Nykyisen mapin nimi

    private void Awake()
    {
        //Jos toinen GameManager on jo olemassa, tuhotaan t‰m‰ GameManager.
        if (instance != null)
        {
            Destroy(this.gameObject);
            return;
        }

        instance = this; //Asetetaan instance-muuttujan arvoksi t‰m‰ GameManager.

        QualitySettings.vSyncCount = 1;
    }
    

    private void Start()
    {
        //Asetetaan valot ja musiikit vastaamaan pelin alussa olevaa mappia, joka voi olla joko aloitusmappi tai tallennuksesta ladattu mappi.
        LightManager.instance.SetLightsForNewMap(currentMap);
    }

    /// <summary>
    /// Kertoo GameManagerille, onko dialogi aktiivinen vai ei. Lis‰ksi metodi suorittaa toisen metodin, jossa p‰‰tet‰‰n, voiko pelaaja liikkua.
    /// </summary>
    /// <param name="dialogActive"></param>
    public void SetDialogState(bool dialogActive)
    {
        isDialogActive = dialogActive;

        UpdateMovementState();
    }

    /// <summary>
    /// Kertoo GameManagerille, onko inventory auki vai ei. Lis‰ksi metodi suorittaa toisen metodin, jossa p‰‰tet‰‰n, voiko pelaaja liikkua.
    /// </summary>
    /// <param name="inventoryOpen"></param>
    public void SetInventoryState(bool inventoryOpen)
    {
        this.inventoryOpen = inventoryOpen;

        UpdateMovementState();
    }

    /// <summary>
    /// Kertoo GameManagerille, onko teleporttaus k‰ynniss‰. T‰t‰ tietoa hyˆdynnet‰‰n p‰‰tt‰m‰‰n, saako pelaaja liikkua vai ei.
    /// </summary>
    public void SetTeleportationState(bool isTeleporting)
    {
        this.isTeleporting = isTeleporting;

        UpdateMovementState();
    }

    /// <summary>
    /// Kertoo GameManagerille, onko minikartta auki. T‰t‰ tietoa hyˆdynnet‰‰n p‰‰tt‰m‰‰n, saako pelaaja liikkua vai ei.
    /// </summary>
    /// <param name="mapOpen"></param>
    public void SetMiniMapState(bool mapOpen)
    {
        this.isMiniMapOpen = mapOpen;

        UpdateMovementState();
    }

    /// <summary>
    /// M‰‰rittelee GameManagerille annettujen tietojen perusteella, voiko pelaaja liikkua.
    /// </summary>
    public void UpdateMovementState()
    {
        if (isDialogActive || inventoryOpen || isTeleporting || isMiniMapOpen)
        {
            PlayerController.instance.DisableMoving();
        } else
        {
            PlayerController.instance.AllowMoving();
        }
    }

    /// <summary>
    /// Muuttaa GameManageriin kirjatun nykyisen kartan.
    /// </summary>
    /// <param name="map"></param>
    public void ChangeCurrentMap(string map)
    {
        currentMap = map;
    }

    /// <summary>
    /// Palauttaa GameManageriin kirjatun nykyisen kartan.
    /// </summary>
    /// <returns></returns>
    public string GetCurrentMap()
    {
        return currentMap;
    }

    /// <summary>
    /// Palauttaa parametrina annettua nime‰ vastaavan mapin.
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public GameObject GetMapByName(string name)
    {
        Map map = System.Array.Find<Map>(maps, map => map.GetName().ToLower() == name.ToLower());
        if (map == null)
            return null;
        return map.GetMapObject();
    }

    /// <summary>
    /// Avaa main menun.
    /// </summary>
    public void GoToMenu()
    {
        SceneManager.LoadScene(0);
    }

}
