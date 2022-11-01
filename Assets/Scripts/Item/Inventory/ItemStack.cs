[System.Serializable]
public class ItemStack
{
    private int itemTypeID;
    private int size;

    private int itemAmount;

    public ItemStack(sbyte size)
    {
        this.size = size;
        this.itemTypeID = -1;
    }

    public bool AddItem()
    {
        if (itemAmount + 1 > size)
            return false;

        itemAmount++;
        return true;
    }

    public bool RemoveItem()
    {
        if (itemAmount < 1)
            return false;

        itemAmount--;
        return true;
    }

    public ushort GetItemType()
    {
        return (ushort)itemTypeID;
    }

    public int GetItemTypeInt()
    {
        return itemTypeID;
    }

    public void SetItemType(int itemType)
    {
        this.itemTypeID = itemType;
    }

    public bool CanAddItem()
    {
        return itemAmount < size;
    }

    public bool IsEmpty()
    {
        return itemAmount == 0;
    }

    public ushort GetItemAmount()
    {
        return (ushort)itemAmount;
    }

    public void SetSize(int size)
    {
        this.size = size;
    }
}
