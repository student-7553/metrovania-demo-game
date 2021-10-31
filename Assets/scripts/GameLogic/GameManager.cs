// This script is a Manager that controls the the flow and control of the game. It keeps
// track of player data (orb count, death count, total game time) and interfaces with
// the UI Manager. All game commands are issued through the static methods of this class

using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

[DefaultExecutionOrder(-100)]
public class GameManager : MonoBehaviour
{
    //This class holds a static reference to itself to ensure that there will only be
    //one in existence. This is often referred to as a "singleton" design pattern. Other
    //scripts access this one through its public static methods
    public class respawnLocation {
        public Vector2 playerlocation; 
        public Vector2 cameraLocation;
        public string cameraAnchorState;
        public float stateHeight;
    }   

    static GameManager current;
    int numberOfDeaths;                         //Number of times player has died
    float totalGameTime;                        //Length of the total game time
    bool m_isGameOver;                            //Is the game currently over?
    List<Collectable> collectableList;	

    respawnLocation m_playerSpikeRespawnData;

    public static respawnLocation playerSpikeRespawnLocation
    {
        get {return current.m_playerSpikeRespawnData; }
        set {
            if (current == null)
			    return;

            current.m_playerSpikeRespawnData = value;
        }
    }

    // public void 

  

    void Awake()
    {
        if (current != null && current != this)
        {
            Destroy(gameObject);
            return;
        }
        current = this;
        DontDestroyOnLoad(gameObject);

        gameSettings();
        awakeGameLogic();        
        
    }

    private void awakeGameLogic(){
        collectableList = new List<Collectable>();
        m_playerSpikeRespawnData = new respawnLocation();


        
        PlayerMovement playerMovement = (PlayerMovement)FindObjectOfType(typeof(PlayerMovement));
        m_playerSpikeRespawnData.playerlocation = playerMovement.transform.position;

        PlayerCameraAchor playerCameraAchor = (PlayerCameraAchor)FindObjectOfType(typeof(PlayerCameraAchor));
        m_playerSpikeRespawnData.cameraLocation = playerCameraAchor.transform.position;
        m_playerSpikeRespawnData.cameraAnchorState = playerCameraAchor.anchorState;
        m_playerSpikeRespawnData.stateHeight = playerCameraAchor.customHeight;

    }

    private void gameSettings(){
        Application.targetFrameRate = 60;
    }

    void Update()
    {
        if (m_isGameOver)
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

        collectableUpdateLogic();

    }

    public static void PlayerDied()
    {
		if (current == null)
			return;
        current.StartCoroutine(RestartScene());

    }

    public static void PlayerHitTrap()
    {
		if (current == null)
			return;
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
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private static void collectableUpdateLogic(){

        if (current.collectableList.Count == 0){
            Debug.Log("There is 0 collectable left");
        }
			

    }

    // public static void PlayerHitRespawnPoint( Vector2 newPoint){
        

    // }

}


