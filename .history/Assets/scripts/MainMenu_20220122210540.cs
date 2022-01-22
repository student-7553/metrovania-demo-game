
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    public void clickedPlayButton()
    {
        SceneManager.LoadSceneAsync("Core", LoadSceneMode.Single);
        SceneManager.LoadSceneAsync("demo", LoadSceneMode.Additive);

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
