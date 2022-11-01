[System.Serializable]
public class Save
{
    //Pelaajan tiedot
    public float playerPosX;
    public float playerPosY;
    public int playerHP;
    public float playerStamina;

    //Vihollisen tiedot
    public float enemyPosX;
    public float enemyPosY;
    public EnemyBehavior.EnemyState enemyState;

    //Aktiivinen mappi
    public string currentMap;

    //Taulukko ker�tyist� esineist�
    public int[] pickedItems;

    //Taulukko aktivoiduista dialogeista
    public int[] activatedDialogs;

    //Taulukko eri teht�vien tiloista
    public Quest.QuestState[] questStates;

    //ItemStoragen esineet
    public ItemStack[] itemStacks;
}
