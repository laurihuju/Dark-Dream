
public class SettingsMenuPage : MenuPage
{
    public override void OnPageExit()
    {
        ShadowToggle.instance.UpdateSetting();
        OnlyPlayerShadowToggle.instance.UpdateSetting();
        SaveManager.SaveSettings();
    }
}
