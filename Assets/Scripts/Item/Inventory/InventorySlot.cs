using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// ItemUseUI:n esinepaikan tietoja hallitseva luokka.
/// </summary>
[System.Serializable]
public class InventorySlot
{
    [SerializeField] private Image itemImage; //Esinepaikan kuva
    [SerializeField] private TextMeshProUGUI itemAmountText; //Esinepaikan m‰‰r‰teksti
    [SerializeField] private GameObject background;

    private int itemTypeID = -1; //Esinepaikassa olevan esinetyypin ID (-1 tarkoittaa, ett‰ paikassa ei ole esinett‰)

    /// <summary>
    /// Palauttaa esinepaikan kuvan.
    /// </summary>
    /// <returns></returns>
    public Image GetItemImage()
    {
        return itemImage;
    }

    /// <summary>
    /// Palauttaa esinepaikan m‰‰r‰tekstin.
    /// </summary>
    /// <returns></returns>
    public TextMeshProUGUI GetItemAmountText()
    {
        return itemAmountText;
    }

    /// <summary>
    /// Palauttaa esinepaikassa olevan esinetyypin ID:n (ID on -1, jos paikassa ei ole esinett‰)
    /// </summary>
    /// <returns></returns>
    public int GetItemTypeID()
    {
        return itemTypeID;
    }

    /// <summary>
    /// Asettaa esinepaikassa olevan esinetyypin ID:n (ID on -1, jos paikassa ei ole esinett‰)
    /// </summary>
    /// <param name="typeID"></param>
    public void SetItemTypeID(int typeID)
    {
        itemTypeID = typeID;
    }

    public GameObject GetBackground()
    {
        return background;
    }
}
