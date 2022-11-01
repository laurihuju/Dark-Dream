using UnityEngine;

/// <summary>
/// K‰yttˆliittym‰, josta voidaan k‰ytt‰‰ esineit‰.
/// </summary>
public class Inventory : MonoBehaviour
{
    public static Inventory instance;

    [SerializeField] private InventorySlot[] slots; //Taulukko inventoryn esinepaikoista

    private void Awake()
    {
        //Jos toinen Inventory on jo olemassa, tuhotaan t‰m‰ Inventory
        if(instance != null)
        {
            Destroy(this.gameObject);
            return;
        }

        instance = this; //Asetetaan instance-muuttujan arvoksi t‰m‰ Inventory
    }

    private void Start()
    {
        //M‰‰ritell‰‰n numeronappuloiden painamiseen esineiden k‰yttˆ
        PlayerController.instance.GetInput().Game.Button1.performed += _ => ItemUse(0);
        PlayerController.instance.GetInput().Game.Button2.performed += _ => ItemUse(1);
        PlayerController.instance.GetInput().Game.Button3.performed += _ => ItemUse(2);
    }

    /// <summary>
    /// P‰ivitt‰‰ inventoryn esineiden kuvat ja m‰‰r‰t ItemStoragessa olevien esineiden mukaan.
    /// </summary>
    public void UpdateInventory()
    {
        //K‰yd‰‰n l‰pi kaikki inventoryn esinepaikat
        for(sbyte i = 0; i < slots.Length; i++)
        {
            if (ItemStorage.instance.GetContent().Length <= i)
                continue;
            if (ItemStorage.instance.GetContent()[i] == null)
                continue;

            if(ItemStorage.instance.GetContent()[i].IsEmpty())
            {
                slots[i].GetItemImage().enabled = false;
                slots[i].GetItemAmountText().enabled = false;
                slots[i].SetItemTypeID(-1);
            } else
            {
                int itemTypeID = ItemStorage.instance.GetContent()[i].GetItemType(); //Pyydet‰‰n esinepaikkaa vastaavaan kohtaan ItemStorageen varastoidun esinetyypin ID:t‰

                slots[i].SetItemTypeID(itemTypeID); //Asetetaan esinepaikan esinetyypin ID:ksi pyydetty ID

                ItemType type = ItemManager.instance.GetItemType((ushort)itemTypeID); //Haetaan esinetyypin ID:t‰ vastaava esinetyyppi ItemManagerilta
                if (type != null) //Jos esinetyyppi lˆytyi
                {
                    Sprite typeSprite = type.GetTypeSprite(); //Haetaan esinetyyppi‰ vastaava kuva ItemTypelt‰
                    if (typeSprite != null) //Jos esinetyypist‰ lˆytyi kuva, asetetaan kuva esinepaikkaan ja aktivoidaan esinepaikan kuva
                    {
                        slots[i].GetItemImage().enabled = true;
                        slots[i].GetItemImage().sprite = typeSprite;
                    }
                    else //Jos esinetyypist‰ ei lˆytynyt kuvaa, asetetaan esinepaikan kuva ep‰aktiiviseksi
                    {
                        slots[i].GetItemImage().enabled = false;
                    }
                }
                else //Jos esinetyyppi‰ ei lˆytynyt, asetetaan esinepaikan kuva ep‰aktiiviseksi
                {
                    slots[i].GetItemImage().enabled = false;
                }

                slots[i].GetItemAmountText().enabled = true; //Asetetaan esinepaikan m‰‰r‰teksti aktiiviseksi
                slots[i].GetItemAmountText().text = "" + ItemStorage.instance.GetContent()[i].GetItemAmount(); //Asetetaan esinepaikan m‰‰r‰tekstiksi ItemStoragesta saatava kyseist‰ esinetyyppi‰ olevien esineiden m‰‰r‰
            }
        }
    }

    /// <summary>
    /// Metodi k‰ytt‰‰ inventoryn parametrina annetussa paikassa olevan esineen.
    /// </summary>
    /// <param name="itemSlot"></param>
    public void ItemUse(int itemSlot)
    {
        if (slots.Length <= itemSlot || itemSlot < 0) //Kumotaan esineen k‰yttˆ, jos annettua paikkaa ei lˆydy UI:sta.
            return;
        if (slots[itemSlot].GetItemTypeID() == -1) //Kumotaan esineen k‰yttˆ, jos annetussa esinepaikassa ei ole esinett‰.
            return;
        ItemType type = ItemManager.instance.GetItemType((ushort)slots[itemSlot].GetItemTypeID());
        if (type == null) //Kumotaan esineen k‰yttˆ, jos esinepaikassa olevan esineen esinetyyppi‰ ei ole rekisterˆity ItemManageriin.
            return;
        if (!type.CanUse()) //Kumotaan esineen k‰yttˆ, jos esinepaikassa olevan esineen esinetyyppi‰ ei voida k‰ytt‰‰ t‰ll‰ hetkell‰.
            return;
        if (!ItemStorage.instance.RemoveItem((ushort)slots[itemSlot].GetItemTypeID())) //Yritet‰‰n poistaa esinepaikassa olevan esineen tyyppinen esine ItemStoragesta. Jos poistaminen ei onnistu, kumotaan esineen k‰yttˆ.
            return;
        type.Use(); //Suoritetaan esinepaikassa olevan esineen esinetyypin Use-metodi.
    }

    public InventorySlot[] GetSlots()
    {
        return slots;
    }
}
