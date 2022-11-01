using UnityEngine;
using UnityEngine.UI;

public class ShadowToggle : MonoBehaviour
{
    public static ShadowToggle instance;

    [SerializeField] private Slider slider;

    [SerializeField] private GameObject shadows;

    private void Awake()
    {
        if(instance != null)
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
        if (slider.value == 0)
            shadows.SetActive(false);
        else
            shadows.SetActive(true);
    }
}
