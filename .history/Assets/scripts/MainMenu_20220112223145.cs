
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    public void clickedPlayButton()
    {
        // SceneManager.LoadSceneAsync("Core", LoadSceneMode.Additive);
        SceneManager.LoadSceneAsync("Demo", LoadSceneMode.Single);

        // SceneManager.SetActiveScene(SceneManager.GetSceneByName("Demo"));
        // SceneManager.SetActiveScene();

        // SceneManager.GetSceneByName("Demo");
        // SceneManager.SetActiveScene(SceneManager.GetSceneByName("Demo"));
        // SceneManager.UnloadSceneAsync("MainMenu");

    }

    public void clickedOptionsButton()
    {

    }
    public void clickedQuitButton()
    {
        Application.Quit();
    }
}
