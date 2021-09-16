// This script is a Manager that controls the the flow and control of the game. It keeps
// track of player data (orb count, death count, total game time) and interfaces with
// the UI Manager. All game commands are issued through the static methods of this class

using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    //This class holds a static reference to itself to ensure that there will only be
    //one in existence. This is often referred to as a "singleton" design pattern. Other
    //scripts access this one through its public static methods
    static GameManager current;

    int numberOfDeaths;                         //Number of times player has died
    float totalGameTime;                        //Length of the total game time
    bool isGameOver;                            //Is the game currently over?
    List<Collectable> collectableList;	

  

    void Awake()
    {
        if (current != null && current != this)
        {
            Destroy(gameObject);
            return;
        }
        current = this;
        DontDestroyOnLoad(gameObject);
        Application.targetFrameRate = 60;
        awakeGameLogic();
        

    }

    private void awakeGameLogic(){
        collectableList = new List<Collectable>();
    }

    void Update()
    {
        if (isGameOver)
            return;

        totalGameTime += Time.deltaTime;
    }

    public static void RegisterCollectable( Collectable collectable){
        if(current == null){
            return;
        }
        if(!current.collectableList.Contains(collectable)){
            current.collectableList.Add(collectable);
        }

        // other stuff
    }

    public static void PlayerGrabbedCollectable(Collectable collectable){
        if(current == null){
            return;
        }
        if(!current.collectableList.Contains(collectable)){
            return;
        }
        current.collectableList.Remove(collectable);
        Debug.Log("Collected one collectable");
        collectableUpdateLogic();

    }

    public static void PlayerDied()
    {
		if (current == null)
			return;
        Debug.Log("Player died");
        current.StartCoroutine(RestartScene());

    }

	public static void TriggerUiText(){
		if (current == null)
			return;

		UiManager.DisplayGameOverText();
	}	

	private static IEnumerator RestartScene()
    {

        yield return new WaitForSeconds(1.5f);
        // //Clear the current list of orbs
        // orbs.Clear();

        // //Play the scene restart audio
        // AudioManager.PlaySceneRestartAudio();

        //Reload the current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private static void collectableUpdateLogic(){

        if (current.collectableList.Count == 0){
            Debug.Log("There is 0 collectable left");
        }
			

    }

}


