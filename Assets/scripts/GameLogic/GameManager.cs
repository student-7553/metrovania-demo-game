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

    static GameManager current;
    int numberOfDeaths;                         //Number of times player has died
    float totalGameTimeThisSession;                        //Length of the total game time
    bool m_isGameOver;                            //Is the game currently over?


    public class respawnLocation {
        public Vector2 playerlocation = new Vector2(0,0); 
        public Vector2 cameraLocation = new Vector2(0,0);
        public string cameraAnchorState;
        public float stateHeight;
    }

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


    respawnLocation m_playerDeathRespawnData ;

    public static respawnLocation playerDeathRespawnData
    {
        get {return current.m_playerDeathRespawnData; }
        set {
            if (current == null)
			    return;

            current.m_playerDeathRespawnData = value;
        }
    } 

    


  

    void Start()
    {
        if (current != null)
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
        // collectableList = new List<Collectable>();
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

        totalGameTimeThisSession += Time.deltaTime;
    }


    public static void PlayerDied()
    {
		if (current == null)
			return;

        // m_playerFloatResources.currentHealth
        PlayerData.playerFloatResources.currentHealth = PlayerData.playerFloatResources.maximumHealth;
        // PlayerData.enabledCollision;
        current.StartCoroutine(RestartScene());

    }

    public static void PlayerHitTrap()
    {
		if (current == null)
			return;
        

        PlayerData.isAlive = true;
        current.StartCoroutine(RestartScene());

    }

	public static void TriggerUiText(){
		if (current == null)
			return;

		UiManager.DisplayGameOverText();
	}	

	private static IEnumerator RestartScene()
    {
        yield return new WaitForSeconds(0.5f);
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }


}


