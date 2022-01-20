using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UiManager : MonoBehaviour
{
    // Start is called before the first frame update

    static UiManager current;

	public TextMeshProUGUI healthTextBar;			
	public TextMeshProUGUI resourceTextBar;	
    public TextMeshProUGUI currencyTextBar;		
    
	public TextMeshProUGUI deathText;		//Text element showing number or deaths
	public TextMeshProUGUI gameOverText;	//Text element showing the Game Over message
    void Start()
    {
        if (current != null && current != this)
		{
			//...destroy this and exit. There can be only one UIManager
			Destroy(gameObject);
			return;
		}
        current = this;
		DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    public static void DisplayGameOverText(){
        if(current == null)
            return;
        
        current.gameOverText.enabled = true;
    } 

    private void Update() {
        healthTextBar.text =PlayerData.playerFloatResources.maximumHealth.ToString()+"/"+ PlayerData.playerFloatResources.currentHealth.ToString();
    }
    
}
