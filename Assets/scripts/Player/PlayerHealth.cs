// This script handles detecting collisions with traps and telling the Game Manager
// when the player dies

using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
	// public GameObject deathVFXPrefab;	//The visual effects for player death
	bool isAlive = true;				//Stores the player's "alive" state
	int trapsLayer;						//The layer the traps are on
    
	void Start()
	{
		//Get the integer representation of the "Traps" layer
		trapsLayer = LayerMask.NameToLayer("Traps");
	}

	void OnTriggerEnter2D(Collider2D collision)
	{
		//If the collided object isn't on the Traps layer OR if the player isn't currently
		//alive, exit. This is more efficient than string comparisons using Tags
		if (collision.gameObject.layer == trapsLayer || !isAlive){
			
			PlayerHitTrap();
		} else {
			return;
		}
	}

	void PlayerHitTrap(){
		isAlive = false;
		gameObject.SetActive(false);
		GameManager.PlayerHitTrap();
		AudioManager.PlayDeathAudio();
	}
}
