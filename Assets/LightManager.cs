using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

/// <summary>
/// Tarkistaa miss‰ paikassa ollaan ja k‰ynnist‰‰ valot sen perusteella.
/// </summary>

public class LightManager : MonoBehaviour
{
    public static LightManager instance;

    // Talojen omat valot sammutetaan, kun ei niit‰ tarvita. T‰m‰ s‰‰st‰‰ tehoa.
    [Header("Lights")]
    [SerializeField] GameObject LightsForAnnaHouse;
    [SerializeField] GameObject LightsForHouse2;
    [SerializeField] GameObject LightsForHouse3;
    [SerializeField] GameObject LightsForLastHouse;

    // Pelin p‰‰valo
    [SerializeField] Light2D globalLight;
   
    // Pelaajan valo:
    [SerializeField] GameObject playerLight;

    // Muut efektiti, kuten esim. sade.
    [Header("other effects")]
    [SerializeField] GameObject Rain;


    [Header("Options for lights")]
    // Valoiden (eli firstmap, mainmap ja houses) voimakkuus

    [SerializeField] float LightIntensityForMainMap;
    [SerializeField] float LightIntensityForFirstMap;
    [SerializeField] float LightIntensityForHouses;
    [SerializeField] private float lightChangeSmoothness;
    private float targetGlobalLightIntensity;


    // V‰rit valoille:

    [SerializeField] Color32 ColorForMainMap;

    [SerializeField] Color32 ColorForFirstMap;

    [SerializeField] Color32 ColorForHouses;


    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
            return;
        }

        instance = this;

        SetGlobalLightForHouses();
    }

    private void Start()
    {
        targetGlobalLightIntensity = globalLight.intensity;
    }

    /// <summary>
    /// Muuttaa global lightin intensity‰ tasaisesti kohti kohdeintensity‰.
    /// </summary>
    private void Update()
    {
        if (globalLight.intensity == targetGlobalLightIntensity)
            return;
        if (Mathf.Abs(globalLight.intensity - targetGlobalLightIntensity) <= 0.1)
        {
            globalLight.intensity = targetGlobalLightIntensity;
            return;
        }
        globalLight.intensity = Mathf.Lerp(globalLight.intensity, targetGlobalLightIntensity, lightChangeSmoothness * Time.deltaTime);
    }


    public void SetLightsForNewMap(string nextMap)
    {
        if (nextMap == "FirstMap")
        {
            SetGlobalLightForFirstMap();

            AudioManager.instance.ChangeMusic("safety");

            LightsForAnnaHouse.SetActive(false);
          

           

        }
        else if (nextMap == "AnnaHouse")
        {
            SetGlobalLightForHouses();

            AudioManager.instance.ChangeMusic("safety");

            LightsForAnnaHouse.SetActive(true);
           


        }
        else if (nextMap == "MainMap")
        {
            SetGlobalLightForMainMap();

            AudioManager.instance.ChangeMusic("gameBG");
            playerLight.SetActive(true);

            LightsForHouse2.SetActive(false);
            LightsForHouse3.SetActive(false);
            LightsForLastHouse.SetActive(false);
           

          
            Rain.SetActive(true);


        }
        else if (nextMap == "LastHouse")
        {
            SetGlobalLightForHouses();

            AudioManager.instance.ChangeMusic("House2&3");
            Rain.SetActive(false);
          
            LightsForLastHouse.SetActive(true);
            playerLight.SetActive(false);


        }
        else if (nextMap == "House2")
        {
            SetGlobalLightForHouses();

            AudioManager.instance.ChangeMusic("fireplace");
            Rain.SetActive(false);
            playerLight.SetActive(false);
        
            LightsForHouse2.SetActive(true);


        }
        else if (nextMap == "House4")
        {
            SetGlobalLightForHouses();

            AudioManager.instance.ChangeMusic("House2&3");

            Rain.SetActive(false);
            playerLight.SetActive(false);
         
            LightsForHouse3.SetActive(true);

        } else if(nextMap == "OldTower")
        {

            SetGlobalLightForHouses();

            AudioManager.instance.ChangeMusic("OldHouse");
            
            Rain.SetActive(false); 


        }
    }

    

    public void SetGlobalLightForMainMap()
    {
        

        // 2.38
        // 11124B
        globalLight.intensity = LightIntensityForMainMap;
        targetGlobalLightIntensity = LightIntensityForMainMap;
        globalLight.color = ColorForMainMap;

    }
    public void SetGlobalLightForHouses()
    {
        

        globalLight.intensity = LightIntensityForHouses;
        targetGlobalLightIntensity = LightIntensityForHouses;
        globalLight.color = ColorForHouses;

        // 0.27
    }
    /// <summary>
    /// Asettaa valolle intesiivisyydeksi "LightIntensityForFirstMap"
    /// </summary>
    public void SetGlobalLightForFirstMap()
    {
     

        // 1
        // ffffff
        globalLight.intensity = LightIntensityForFirstMap;
        targetGlobalLightIntensity = LightIntensityForFirstMap;
        globalLight.color = ColorForFirstMap;
    }

    public float GetTargetGlobalLightIntensity()
    {
        return targetGlobalLightIntensity;
    }

    public void SetTargetGlobalLightIntensity(float newIntensity)
    {
        targetGlobalLightIntensity = newIntensity;
    }


    public void SetOffEffects()
    {

        Rain.SetActive(false);
        playerLight.SetActive(false);

    }


}
