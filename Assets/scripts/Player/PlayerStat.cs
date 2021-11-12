// This script handles detecting collisions with traps and telling the Game Manager
// when the player dies

using UnityEngine;

public class PlayerStat : MonoBehaviour
{
	
	public int baseAttackDamage;
	public int effectiveAttackDamage;
	public float totalHealth;
	public float remainingHealth;
	public bool isAlive;				//Stores the player's "alive" state
	private int trapsLayer;	
	private int enemyLayer;						
    
	void Start()
	{
		isAlive = true;
		//Get the integer representation of the "Traps" layer
		trapsLayer = LayerMask.NameToLayer("Traps");
		enemyLayer = LayerMask.NameToLayer("Enemies");
	}

	void OnTriggerEnter2D(Collider2D collision)
	{
		//If the collided object isn't on the Traps layer OR if the player isn't currently
		//alive, exit. This is more efficient than string comparisons using Tags
		if (collision.gameObject.layer == trapsLayer || !isAlive){

			PlayerHitTrap();

		} else if ( collision.gameObject.layer == enemyLayer || !isAlive) {
			BaseEnemy tempBaseEnemy = collision.gameObject.GetComponent<BaseEnemy>();
			Debug.Log( tempBaseEnemy.damage); 
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
