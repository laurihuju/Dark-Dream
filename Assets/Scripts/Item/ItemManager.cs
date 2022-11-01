using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public static ItemManager instance;

    private List<ItemType> itemTypes; //Lista esinetyypeist�
    private List<ushort> pickedItems; //Lista ker�ttyjen esineiden ID:ist�

    private void Awake()
    {
        //Jos ItemManager on jo luotu, tuhotaan t�m� ItemManager.
        if (instance != null)
        {
            Destroy(this.gameObject);
            return;
        }

        //Luodaan esinetyyppien lista.
        itemTypes = new List<ItemType>();
        
        instance = this; //Asetetaan instance-muuttujaan t�m� ItemManager.
    }

    private void Start()
    {
        //Jos ker�ttyjen esineiden listaa ei ole viel� luotu, luodaan se.
        if(pickedItems == null)
            pickedItems = new List<ushort>();
    }

    /// <summary>
    /// Rekister�i ItemTypen lis��m�ll� sen listaan.
    /// </summary>
    /// <param name="type"></param>
    public void RegisterItemType(ItemType type)
    {
        itemTypes.Add(type);
    }

    /// <summary>
    /// Palauttaa esineen tyypin ID:t� vastaavan rekister�idyn ItemTypen. Jos ID:t� vastaavaa ItemType� ei l�ydy, palauttaa metodi arvon null.
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
    /// Kertoo, onko parametrina annettu esineyksil�n ID listattu ker�tyksi.
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
    /// Listaa parametrina annetun esineyksil�n ID:n ker�tyksi.
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
