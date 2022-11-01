using UnityEngine;

public class ItemStorage : MonoBehaviour
{
    public static ItemStorage instance;

    [SerializeField] private int itemSlots; //ItemStoragen esinepaikkojen m‰‰r‰
    private ItemStack[] stacks; //Taulukko ItemStoragen esinepaikoista, jotka sis‰lt‰v‰t ItemStackeja.

    private void Awake()
    {
        //Jos ItemStorage on jo luotu, tuhotaan t‰m‰ ItemStorage.
        if (instance != null)
        {
            Destroy(this.gameObject);
            return;
        }

        //Luodaan ItemStackien taulukko ja t‰ytet‰‰n se uusilla esinepaikoilla
        stacks = new ItemStack[itemSlots];
        for(int i = 0; i < itemSlots; i++)
        {
            stacks[i] = new ItemStack(1);
        }

        instance = this; //Asetetaan instance-muuttujaan t‰m‰ ItemStorage.
    }

    /// <summary>
    /// Varastoi parametrina annettua esinetyyppi‰ olevan esineen ItemStorageen. Jos varastoiminen onnistuu, palauttaa metodi arvon true. Jos varastoiminen ep‰onnistuu, palauttaa metodi arvon false.
    /// </summary>
    /// <param name="typeID"></param>
    /// <param name="amount"></param>
    /// <returns></returns>
    public bool AddItem(ushort typeID, ushort amount)
    {
        ItemType type = ItemManager.instance.GetItemType(typeID);

        for(ushort i = 0; i < amount; i++)
        {
            int stackIndex = GetStackForItem(typeID);
            if (stackIndex == -1)
            {
                Debug.Log("Itemin tyyppi-ID:ll‰ " + typeID + " ker‰‰minen ep‰onnistui, koska ItemStoragessa ei ole tilaa kyseisen tyyppiselle esineelle!");
                Inventory.instance.UpdateInventory();
                return false;
            }
            ItemStack stack = stacks[stackIndex];

            stack.SetItemType(typeID);
            if (type != null)
                stack.SetSize(type.GetMaxStackSize());
            else
                stack.SetSize(1);
            

            if (!stack.AddItem())
                return false;
        }

        Inventory.instance.UpdateInventory();
        QuestManager.instance.UpdateQuestsWithItem(typeID);

        return true;
    }

    /// <summary>
    /// Varastoi parametrina annettua esinetyyppi‰ olevan esineen ItemStorageen. Jos varastoiminen onnistuu, palauttaa metodi arvon true. Jos varastoiminen ep‰onnistuu, palauttaa metodi arvon false.
    /// </summary>
    /// <param name="typeID"></param>
    /// <returns></returns>
    public bool AddItem(ushort typeID)
    {
        return AddItem(typeID, 1);
    }

    /// <summary>
    /// Poistaa parametrina annettua esinetyyppi‰ olevan esineen ItemStoragesta. Jos poistaminen onnistuu, palauttaa metodi arvon true. Jos annettua esinetyyppi‰ vastaavaa esinett‰ ei ole varastoitu, palauttaa metodi arvon false.
    /// </summary>
    /// <param name="typeID"></param>
    /// <returns></returns>
    public bool RemoveItem(ushort typeID)
    {
        int stackIndex = GetStackWithItem(typeID);
        if (stackIndex == -1)
        {
            Debug.Log("Itemin tyyppi-ID:ll‰ " + typeID + " poistaminen ItemStoragesta ep‰onnistui, koska ItemStoragessa ei ole kyseisen tyyppisi‰ esineit‰!");
            return false;
        }
        ItemStack stack = stacks[stackIndex];

        if (!stack.RemoveItem())
            return false;

        Inventory.instance.UpdateInventory();
        QuestManager.instance.UpdateQuestsWithItem(typeID);
        return true;
    }

    public void ChangeItemStackPositions(int index1, int index2)
    {
        if (index1 >= itemSlots || index2 >= itemSlots)
            return;

        ItemStack stack = stacks[index1];
        stacks[index1] = stacks[index2];
        stacks[index2] = stack;
    }

    public ItemStack[] GetContent()
    {
        return stacks;
    }

    public void SetContent(ItemStack[] stacks)
    {
        this.stacks = stacks;

        for(int i = 0; i < stacks.Length; i++)
        {
            if(stacks[i] != null)
            {
                int typeID = stacks[i].GetItemTypeInt();
                if(typeID >= 0 && typeID < ushort.MaxValue)
                {
                    ItemType type = ItemManager.instance.GetItemType((ushort)typeID);
                    if(type != null)
                    {
                        stacks[i].SetSize(type.GetMaxStackSize());
                    }
                }
            }
        }
    }

    public int GetItemAmount(ushort itemTypeID)
    {
        int itemAmount = 0;

        for (int i = 0; i < stacks.Length; i++)
        {
            if (stacks[i] != null)
            {
                if (stacks[i].GetItemType() == itemTypeID && !stacks[i].IsEmpty())
                    itemAmount += stacks[i].GetItemAmount();
            }
        }
        return itemAmount;
    }

    private int GetStackForItem(ushort itemTypeID)
    {
        for (int i = 0; i < stacks.Length; i++)
        {
            if (stacks[i] != null)
            {
                if (stacks[i].CanAddItem() && stacks[i].GetItemType() == itemTypeID && !stacks[i].IsEmpty())
                    return i;
            }
        }

        for (int i = 0; i < stacks.Length; i++)
        {
            if(stacks[i] != null)
            {
                if (stacks[i].IsEmpty())
                    return i;
            }
        }

        return -1;
    }

    private int GetStackWithItem(ushort itemTypeID)
    {
        for (int i = 0; i < stacks.Length; i++)
        {
            if (stacks[i] != null)
            {
                if (stacks[i].GetItemType() == itemTypeID && !stacks[i].IsEmpty())
                    return i;
            }
        }

        return -1;
    }
}
