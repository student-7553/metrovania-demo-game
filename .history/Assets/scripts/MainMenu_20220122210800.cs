using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    public void clickedPlayButton()
    {
        StartCoroutine(LoadScene());
    }

    IEnumerator LoadScene()
    {

        AsyncOperation coreLoad = SceneManager.LoadSceneAsync("Core", LoadSceneMode.Single);
        
        // AsyncOperation demoLoad = SceneManager.LoadSceneAsync("demo", LoadSceneMode.Additive);
        // AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Scene2");
        // Wait until the asynchronous scene fully loads
        while (!coreLoad.isDone)
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
