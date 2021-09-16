using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public static bool isGamePaused = false;
    public GameObject pauseMenuUI;


    public void Resume(){
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isGamePaused = false;
    }
    public void LoadMenu(){
        Debug.Log("Load Menu");
        // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void Quit(){
        
        Debug.Log("Quitting");
        // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        // Application.Quit();
    }
    private void Pause(){
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isGamePaused = true;
    }

    void Update()
    {
        // if(Input.GetKeyDown(KeyCode.Escape)){
        if(Input.GetButtonDown("Cancel")){
            if(isGamePaused){
                Resume();
            } else {
                Pause();
            }
        }
    }
}
