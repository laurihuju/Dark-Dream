using System.Collections;
using UnityEngine;

public class NightVisionItemType : ItemType
{
    [SerializeField] private float intensityMultiplier;
    [SerializeField] private float effectTime;

    private Coroutine effectCo;

    /// <summary>
    /// Metodi, joka suoritetaan, kun esine k‰ytet‰‰n. Metodi korvaa ItemType-luokassa olevan metodin.
    /// </summary>
    public override void Use()
    {
        if(effectCo == null)
        {
            effectCo = StartCoroutine(NightVisionEffect());
        }
        Debug.Log("Night vision item tyyppi-ID:ll‰ " + GetTypeID() + " on k‰ytetty!");
    }

    private IEnumerator NightVisionEffect()
    {
        LightManager.instance.SetTargetGlobalLightIntensity(LightManager.instance.GetTargetGlobalLightIntensity() * intensityMultiplier);
        float currentIntensity = LightManager.instance.GetTargetGlobalLightIntensity();

        yield return new WaitForSeconds(effectTime);

        if(LightManager.instance.GetTargetGlobalLightIntensity() == currentIntensity)
            LightManager.instance.SetTargetGlobalLightIntensity(LightManager.instance.GetTargetGlobalLightIntensity() / intensityMultiplier);

        effectCo = null;
    }

    /// <summary>
    /// Metodi, joka kertoo, voidaanko esinetyypin esinett‰ k‰ytt‰‰. Metodi korvaa ItemType-luokassa olevan metodin.
    /// </summary>
    public override bool CanUse()
    {
        return effectCo == null && GameManager.instance.GetCurrentMap() == "MainMap";
    }
}
