using UnityEngine;

public class ItemType : MonoBehaviour
{
    [SerializeField] private ushort typeID; //Esinetyypin ID

    [SerializeField] private string itemName; //Esinetyypin nimi, jota k‰ytet‰‰n tooltipiss‰

    [SerializeField] private Sprite typeSprite; //Esinetyypin kuva (k‰ytet‰‰n UI:ssa)

    [SerializeField] private int maxStackSize;

    private void Awake()
    {
        ItemManager.instance.RegisterItemType(this); //Rekisterˆid‰‰n esinetyyppi ItemManageriin.
    }

    /// <summary>
    /// Metodi, joka suoritetaan, kun esine k‰ytet‰‰n. Metodi voidaan korvata toisella ItemTypen periv‰ss‰ luokassa.
    /// </summary>
    public virtual void Use()
    {
        Debug.Log("Tyhj‰ item tyyppi-ID:ll‰ " + typeID + " on k‰ytetty!");
    }

    /// <summary>
    /// Metodi, joka kertoo, voidaanko esinetyypin esinett‰ k‰ytt‰‰. Metodi voidaan korvata toisella ItemTypen periv‰ss‰ luokassa.
    /// </summary>
    public virtual bool CanUse()
    {
        return false;
    }

    /// <summary>
    /// Palauttaa esinetyypin ID:n
    /// </summary>
    /// <returns></returns>
    public ushort GetTypeID()
    {
        return typeID;
    }

    /// <summary>
    /// Palauttaa esinetyypin kuvan
    /// </summary>
    /// <returns></returns>
    public Sprite GetTypeSprite()
    {
        return typeSprite;
    }

    /// <summary>
    /// Palauttaa esinetyypin nimen.
    /// </summary>
    /// <returns></returns>
    public string GetItemName()
    {
        return itemName;
    }

    public int GetMaxStackSize()
    {
        if(maxStackSize > 0)
            return maxStackSize;
        return 1;
    }
}
