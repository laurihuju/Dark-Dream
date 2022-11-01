using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    private void Start() {
        AudioManager.instance.ChangeMusic("mmbg");
    }
    
    public void Continue()
    {
        // M‰‰ritell‰‰n tallenne ladattavaksi pelin latautuessa ja ladataan peli
        SaveManager.LoadSaveOnLoad();
        SceneManager.LoadScene(1);
    }

    public void NewGame()
    {
        // Ladataan peli, mutta ei tallennetta
        SceneManager.LoadScene(1);
    }

    public void Exit()
    {
        // Suljetaan sovellus
        Application.Quit();
    }
}








