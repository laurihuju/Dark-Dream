using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

/// <summary>
/// S‰‰telee vignette -efekti‰
/// </summary>
public class VignetteController : MonoBehaviour
{
    public static VignetteController instance;

    [SerializeField] Volume volume;
    private Vignette vignette;
    private float intensity;

    ColorParameter colorParameterRed = new ColorParameter(Color.red);
    ColorParameter colorParameterBlack = new ColorParameter(Color.black);
    void Start()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
            return;
        }

        instance = this;

        volume.profile.TryGet(out vignette);
        
        vignette.color.Override((Color)colorParameterBlack);
    }

   
    
   


    void Update()
    {
        if(HealthManager.instance.GetCurrentHP() < HealthManager.instance.GetMaxHP() && HealthManager.instance.GetCurrentHP() > 10)
        {
            
            vignette.color.Override((Color)colorParameterRed);

            intensity = 1f - (float)HealthManager.instance.GetCurrentHP() / HealthManager.instance.GetMaxHP();
        } else if(HealthManager.instance.GetCurrentHP() <= 10)
        {

            vignette.color.Override((Color)colorParameterBlack);
            
            vignette.intensity.value = Mathf.Sin(Time.realtimeSinceStartup);
            return;
        } else
        {
            vignette.color.Override((Color)colorParameterBlack);
            vignette.intensity.value = 0;
            return;
        }


        if (intensity < 0)
        {
            intensity = 0;


        }
        else if (intensity > 1)
        {

            intensity = 1;



        }

        vignette.intensity.value = intensity;

    }

    public void SetVignetteForMainMap()
    {
        vignette.intensity.value = 0.20f;

        

    }

    

}
