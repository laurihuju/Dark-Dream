using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] private ushort typeID; //Esineen tyypin ID
    [SerializeField] private ushort itemID; //Esineyksilˆn ID, pit‰‰ olla eri jokaisella yksilˆll‰

    [SerializeField] private int questNeedsToBeActive = -1;

    private void Start()
    {
        //Tuhotaan esine, jos esine on jo ker‰tty. Esine voi olla t‰ss‰ vaiheessa ker‰tty, jos esine on tallennettu ker‰tyksi.
        if (ItemManager.instance.HasItemPicked(itemID))
        {
            Destroy(this.gameObject);
            return;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Tarkistetaan, onko triggeriin tˆrm‰nnyt objekti pelaaja
        if (collision.CompareTag("Player"))
        {
            if (questNeedsToBeActive >= 0) //Jos questNeedsToBeActive on suurempi tai yht‰ suuri kuin 0, pit‰‰ ennen esineen ker‰yst‰ tarkistaa, onko m‰‰ritelty teht‰v‰ aktiivinen.
            {
                Quest questToBeActive = QuestManager.instance.GetQuest(questNeedsToBeActive);
                if (questToBeActive == null)
                    return;
                if (questToBeActive.GetState() != Quest.QuestState.active)
                    return;
            }

            //Kokeillaan lit‰t‰ esine ItemStorageen. Jos esine ei mahdu sinne, palauttaa AddItem-metodi arvon false ja metodin suoritus keskeytet‰‰n.
            if (!ItemStorage.instance.AddItem(typeID))
                return;
            ItemManager.instance.ItemPicked(itemID); //Lis‰t‰‰n t‰m‰ esineyksilˆ ker‰ttyjen esineiden listaan, josta ker‰tyt esineet tallennetaan.

            Destroy(this.gameObject); //Tuhotaan esine, koska se ker‰ttiin.
        }
    }
}
