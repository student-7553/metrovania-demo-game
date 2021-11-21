
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    public void clickedPlayButton(){
        // SceneManager.
        SceneManager.LoadSceneAsync("demo");
    }

    public void clickedOptionsButton(){
        
    }
    public void clickedQuitButton(){
        Application.Quit();
    }
}
