using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    public void clickedPlayButton()
    {
        // LoadScene();
        SceneManager.LoadSceneAsync("Core", LoadSceneMode.Single);
        SceneManager.LoadSceneAsync("demo", LoadSceneMode.Additive);
    }


    // void LoadScene()
    // {
    //     // AsyncOperation demoLoad = 
    //     SceneManager.LoadSceneAsync("Core", LoadSceneMode.Single);
    //     SceneManager.LoadSceneAsync("demo", LoadSceneMode.Additive);

    // }
    // IEnumerator LoadScene()
    // {

    //     AsyncOperation coreLoad = SceneManager.LoadSceneAsync("Core",LoadSceneMode.Single);
    //     AsyncOperation demoLoad = SceneManager.LoadSceneAsync("demo",LoadSceneMode.Additive);

    //     // while (!coreLoad.isDone)
    //     // {
    //     //     Debug.Log("still getting done");
    //     //     yield return null;
    //     }

    //     // Debug.Log("are we getting called?");

    //     // AsyncOperation demoLoad = SceneManager.LoadSceneAsync("demo",LoadSceneMode.Additive);
    // }


    public void clickedOptionsButton()
    {

    }
    public void clickedQuitButton()
    {
        Application.Quit();
    }
}
