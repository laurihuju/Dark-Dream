using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

[System.Serializable]
public class LightInformation
{
    [SerializeField] private Light2D light;
    [SerializeField] private float shadowIntensity;

    public Light2D GetLight()
    {
        return light;
    }

    public float GetShadowIntensity()
    {
        return shadowIntensity;
    }
}
