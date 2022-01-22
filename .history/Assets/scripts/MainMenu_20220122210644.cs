using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    public void clickedPlayButton()
    {
        SceneManager.LoadSceneAsync("Core", LoadSceneMode.Single);
        SceneManager.LoadSceneAsync("demo", LoadSceneMode.Additive);\

        StartCoroutine(LoadScene());

        // SceneManager.SetActiveScene(SceneManager.GetSceneByName("Demo"));
        // SceneManager.SetActiveScene();

        // SceneManager.GetSceneByName("Demo");
        // SceneManager.SetActiveScene(SceneManager.GetSceneByName("Demo"));
        // SceneManager.UnloadSceneAsync("MainMenu");

    }

    IEnumerator LoadScene()
    {
        // The Application loads the Scene in the background as the current Scene runs.
        // This is particularly good for creating loading screens.
        // You could also load the Scene by using sceneBuildIndex. In this case Scene2 has
        // a sceneBuildIndex of 1 as shown in Build Settings.

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Scene2");

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }


    public void clickedOptionsButton()
    {

    }
    public void clickedQuitButton()
    {
        Application.Quit();
    }
}
