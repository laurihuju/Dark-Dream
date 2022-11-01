using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour
{
    private static bool loadSave;
    private static bool isReloadingScene;

    private void Awake()
    {
        LoadSettings();

        if (loadSave)
        {
            loadSave = false;
            LoadSave();
        }

        isReloadingScene = false;
    }

    /// <summary>
    /// Luo ja palauttaa uuden tallenteen.
    /// </summary>
    /// <returns></returns>
    private static Save CreateSave()
    {
        //Luodaan uusi tallenne
        Save save = new Save();

        //Pelaajan tiedot
        save.playerPosX = PlayerController.instance.transform.position.x;
        save.playerPosY = PlayerController.instance.transform.position.y;
        save.playerHP = HealthManager.instance.GetCurrentHP();
        save.playerStamina = HealthManager.instance.GetCurrentStamina();

        //Vihollisen tiedot
        save.enemyPosX = EnemyBehavior.instance.transform.position.x;
        save.enemyPosY = EnemyBehavior.instance.transform.position.y;
        save.enemyState = EnemyBehavior.instance.currentState;

        //Aktiivinen mappi
        save.currentMap = GameManager.instance.GetCurrentMap();

        //Kerätyt esineet
        ushort[] pickedItemsUshort = ItemManager.instance.GetPickedItems();
        int[] pickedItemsInt = new int[pickedItemsUshort.Length];
        for(int i = 0; i < pickedItemsUshort.Length; i++)
        {
            pickedItemsInt[i] = pickedItemsUshort[i];
        }
        save.pickedItems = pickedItemsInt;

        //Aktivoidut dialogit
        ushort[] activatedDialogsUshort = DialogManager.instance.GetActivatedDialogs();
        int[] activatedDialogsInt = new int[activatedDialogsUshort.Length];
        for (int i = 0; i < activatedDialogsUshort.Length; i++)
        {
            activatedDialogsInt[i] = activatedDialogsUshort[i];
        }
        save.activatedDialogs = activatedDialogsInt;

        //Tehtävien tilat
        save.questStates = QuestManager.instance.GetQuestStates();

        //Varastoidut esineet
        save.itemStacks = ItemStorage.instance.GetContent();

        //Palautetaan luotu save
        return save;
    }

    public static SettingsSave CreateSettingsSave()
    {
        SettingsSave save = new SettingsSave();
        save.useShadows = ShadowToggle.instance.IsToggledOn();
        save.onlyPlayerLightShadows = OnlyPlayerShadowToggle.instance.IsToggledOn();

        return save;
    }

    public static void Save()
    {
        Debug.Log("Tallennus aloitettu!");

        SavingText.instance.ShowText();

        Save save = CreateSave();

        BinaryFormatter formatter = new BinaryFormatter();

        FileStream stream = File.Create(Application.persistentDataPath + Path.DirectorySeparatorChar + "Save.txt");

        formatter.Serialize(stream, save);

        stream.Close();

        Debug.Log("Tallenne luotu!");
    }

    public static void SaveSettings()
    {
        Debug.Log("Asetusten tallennus aloitettu!");

        SettingsSave save = CreateSettingsSave();

        BinaryFormatter formatter = new BinaryFormatter();

        FileStream stream = File.Create(Application.persistentDataPath + Path.DirectorySeparatorChar + "Settings.txt");

        formatter.Serialize(stream, save);

        stream.Close();

        Debug.Log("Asetusten tallenne luotu!");
    }

    private static void LoadSave()
    {
        if (!File.Exists(Application.persistentDataPath + Path.DirectorySeparatorChar + "Save.txt"))
        {
            Debug.Log("Tallennusta ei ladattu, koska tallennustiedostoa ei löytynyt!");
            return;
        }

        BinaryFormatter formatter = new BinaryFormatter();

        FileStream stream = File.Open(Application.persistentDataPath + Path.DirectorySeparatorChar + "Save.txt", FileMode.Open);

        Save save = formatter.Deserialize(stream) as Save;

        stream.Close();

        //Asetetaan pelaajan sijainti
        PlayerController.instance.transform.position = new Vector3(save.playerPosX, save.playerPosY, PlayerController.instance.transform.position.z);

        //Asetetaan pelaajan HP ja stamina
        HealthManager.instance.SetCurrentHP(save.playerHP);
        HealthManager.instance.SetCurrentStamina(save.playerStamina);

        //Asetetaan vihollisen sijainti ja tila
        EnemyBehavior.instance.transform.position = new Vector3(save.enemyPosX, save.enemyPosY, EnemyBehavior.instance.transform.position.z);
        EnemyBehavior.instance.currentState = save.enemyState;

        //Asetetaan GameManageriin aktiivinen mappi
        GameManager.instance.ChangeCurrentMap(save.currentMap);

        //Asetetaan ItemManageriin kerätyt esineet
        ushort[] pickedItems = new ushort[save.pickedItems.Length];
        for(int i = 0; i < pickedItems.Length; i++)
        {
            pickedItems[i] = (ushort) save.pickedItems[i];
        }
        ItemManager.instance.SetPickedItems(pickedItems);

        //Asetetaan DialogManageriin aktivoidut dialogit
        ushort[] activatedDialogs = new ushort[save.activatedDialogs.Length];
        for (int i = 0; i < activatedDialogs.Length; i++)
        {
            activatedDialogs[i] = (ushort) save.activatedDialogs[i];
        }
        DialogManager.instance.SetActivatedDialogs(activatedDialogs);

        //Lisätään varastoidut esineet ItemStorageen
        ItemStorage.instance.SetContent(save.itemStacks);
        Inventory.instance.UpdateInventory();

        //Asetetaan tehtävien tilat
        QuestManager.instance.SetQuestStates(save.questStates);

        Debug.Log("Tallenne ladattu!");
    }

    private static void LoadSettings()
    {
        if (!File.Exists(Application.persistentDataPath + Path.DirectorySeparatorChar + "Settings.txt"))
        {
            Debug.Log("Asetuksia ei ladattu, koska tallennustiedostoa ei löytynyt! Asetuksina käytetään oletusasetuksia.");
            return;
        }

        BinaryFormatter formatter = new BinaryFormatter();

        FileStream stream = File.Open(Application.persistentDataPath + Path.DirectorySeparatorChar + "Settings.txt", FileMode.Open);

        SettingsSave save = formatter.Deserialize(stream) as SettingsSave;

        stream.Close();

        ShadowToggle.instance.SetValue(save.useShadows);
        OnlyPlayerShadowToggle.instance.SetValue(save.onlyPlayerLightShadows);

        Debug.Log("Asetukset ladattu!");
    }

    public static void LoadSaveOnLoad()
    {
        loadSave = true;
    }

    public static void ReloadSave()
    {
        if (!isReloadingScene)
        {
            loadSave = true;
            isReloadingScene = true;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
