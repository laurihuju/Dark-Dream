using System.Collections;
using UnityEngine;

public class SpeedItemType : ItemType
{
    [SerializeField] private float speedMultiplier; //Arvo, jolla pelaajan nopeus kerrotaan
    [SerializeField] private float effectTime; //Aika, jonka nopeusefekti kest‰‰

    /// <summary>
    /// Metodi, joka suoritetaan, kun esine k‰ytet‰‰n. Metodi korvaa ItemType-luokassa olevan metodin.
    /// </summary>
    public override void Use()
    {
        //Aloitetaan nopeusefektin tekev‰ alirutiini.
        StartCoroutine(Speed());
        Debug.Log("Speed item tyyppi-ID:ll‰ " + GetTypeID() + " on k‰ytetty!");
    }

    /// <summary>
    /// Metodi, joka kertoo, voidaanko esinetyypin esinett‰ k‰ytt‰‰. Metodi korvaa ItemType-luokassa olevan metodin.
    /// </summary>
    public override bool CanUse()
    {
        return true;
    }

    /// <summary>
    /// Alirutiini, joka lis‰‰ pelaajalle nopeusefektin m‰‰ritellyksi ajaksi.
    /// </summary>
    /// <returns></returns>
    private IEnumerator Speed()
    {
        //Kerrotaan pelaajan k‰vely- ja juoksunopeus speedMultiplier-muuttujalla.
        PlayerController.instance.SetWalkSpeed(PlayerController.instance.GetWalkSpeed() * speedMultiplier);
        PlayerController.instance.SetSprintSpeed(PlayerController.instance.GetSprintSpeed() * speedMultiplier);

        //Odotetaan effectTime-muuttujan kertoma aika.
        yield return new WaitForSeconds(effectTime);

        //Jaetaan pelaajan k‰vely- ja juoksunopeus speedMultiplier-muuttujalla, jotta nopeus palautuisi takaisin alkuper‰iseen.
        PlayerController.instance.SetWalkSpeed(PlayerController.instance.GetWalkSpeed() / speedMultiplier);
        PlayerController.instance.SetSprintSpeed(PlayerController.instance.GetSprintSpeed() / speedMultiplier);
    }
}
