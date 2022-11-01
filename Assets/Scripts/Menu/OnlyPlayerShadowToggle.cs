using UnityEngine;
using UnityEngine.UI;

public class OnlyPlayerShadowToggle : MonoBehaviour
{
    public static OnlyPlayerShadowToggle instance;

    [SerializeField] private Slider slider;

    [SerializeField] private LightInformation[] lights;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
            return;
        }

        instance = this;
    }

    public bool IsToggledOn()
    {
        if (slider.value == 0)
            return false;
        return true;
    }

    public void SetValue(bool isToggledOn)
    {
        if (isToggledOn)
            slider.value = 1;
        else
            slider.value = 0;
        UpdateSetting();
    }

    public void UpdateSetting()
    {
        if (slider.value == 1)
        {
            for (int i = 0; i < lights.Length; i++)
            {
                lights[i].GetLight().shadowIntensity = 0;
            }
        } else
        {
            for (int i = 0; i < lights.Length; i++)
            {
                lights[i].GetLight().shadowIntensity = lights[i].GetShadowIntensity();
            }
        }
    }
}