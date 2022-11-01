using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public static ItemManager instance;

    private List<ItemType> itemTypes; //Lista esinetyypeistä
    private List<ushort> pickedItems; //Lista kerättyjen esineiden ID:istä

    private void Awake()
    {
        //Jos ItemManager on jo luotu, tuhotaan tämä ItemManager.
        if (instance != null)
        {
            Destroy(this.gameObject);
            return;
        }

        //Luodaan esinetyyppien lista.
        itemTypes = new List<ItemType>();
        
        instance = this; //Asetetaan instance-muuttujaan tämä ItemManager.
    }

    private void Start()
    {
        //Jos kerättyjen esineiden listaa ei ole vielä luotu, luodaan se.
        if(pickedItems == null)
            pickedItems = new List<ushort>();
    }

    /// <summary>
    /// Rekisteröi ItemTypen lisäämällä sen listaan.
    /// </summary>
    /// <param name="type"></param>
    public void RegisterItemType(ItemType type)
    {
        itemTypes.Add(type);
    }

    /// <summary>
    /// Palauttaa esineen tyypin ID:tä vastaavan rekisteröidyn ItemTypen. Jos ID:tä vastaavaa ItemTypeä ei löydy, palauttaa metodi arvon null.
    /// </summary>
    /// <param name="typeID"></param>
    /// <returns></returns>
    public ItemType GetItemType(ushort typeID)
    {
        foreach(ItemType type in itemTypes)
        {
            if (type.GetTypeID() == typeID)
                return type;
        }
        return null;
    }

    /// <summary>
    /// Kertoo, onko parametrina annettu esineyksilön ID listattu kerätyksi.
    /// </summary>
    /// <param name="itemID"></param>
    /// <returns></returns>
    public bool HasItemPicked(ushort itemID)
    {
        if (pickedItems == null)
            return false;
        return pickedItems.Contains(itemID);
    }

    /// <summary>
    /// Listaa parametrina annetun esineyksilön ID:n kerätyksi.
    /// </summary>
    /// <param name="itemID"></param>
    public void ItemPicked(ushort itemID)
    {
        if (!pickedItems.Contains(itemID))
        {
            pickedItems.Add(itemID);
        }
    }

    public ushort[] GetPickedItems()
    {
        return pickedItems.ToArray();
    }

    public void SetPickedItems(ushort[] pickedItems)
    {
        this.pickedItems = new List<ushort>(pickedItems);
    }
}
