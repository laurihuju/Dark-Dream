using UnityEngine;

public class HealthItemType : ItemType
{
    [SerializeField] private int HPChangeAmount;

    /// <summary>
    /// Metodi, joka suoritetaan, kun esine k‰ytet‰‰n. Metodi korvaa ItemType-luokassa olevan metodin.
    /// </summary>
    public override void Use()
    {
        HealthManager.instance.ChangeCurrentHP(HPChangeAmount);
        Debug.Log("Health item tyyppi-ID:ll‰ " + GetTypeID() + " on k‰ytetty!");
    }

    /// <summary>
    /// Metodi, joka kertoo, voidaanko esinetyypin esinett‰ k‰ytt‰‰. Metodi korvaa ItemType-luokassa olevan metodin.
    /// </summary>
    public override bool CanUse()
    {
        return !HealthManager.instance.IsFullHP();
    }
}
