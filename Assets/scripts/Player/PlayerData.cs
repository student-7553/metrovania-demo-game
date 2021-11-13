using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    static PlayerData current;

    static public bool isAlive;

    public class playerFloatResourceClass {
        public float currentHealth;
        public float maximumHealth;
        public float currentMana;
        public float maximumMana;
        public float currentBaseAttackDamage;
        public float baseAttackDamage;

    }

    private playerFloatResourceClass m_playerFloatResources;

    public static playerFloatResourceClass playerFloatResources
    {
        get {return current.m_playerFloatResources; }
        set {
            if (current == null)
			    return;

            current.m_playerFloatResources = value;
        }
    } 
 

    public class playerCollectables{

    }
    void Start()
    {
        if (current != null && current != this)
        {
            Destroy(gameObject);
            return;
        }
        current = this;
        DontDestroyOnLoad(gameObject);
        initPlayerData();
    }

    void initPlayerData(){

        isAlive = true;

        m_playerFloatResources = new playerFloatResourceClass();
        m_playerFloatResources.currentHealth = 30;
        m_playerFloatResources.maximumHealth = 30;
        m_playerFloatResources.currentMana = 50;
        m_playerFloatResources.maximumMana = 50;
        m_playerFloatResources.currentBaseAttackDamage = 10;
        m_playerFloatResources.baseAttackDamage = 10;
    }

    // Update is called once per frame
    void Update()
    {
        // Debug.Log(m_playerFloatResources.currentHealth);
        // check if player is alive
        if(m_playerFloatResources.currentHealth <= 0){
            isAlive = false;
            GameManager.PlayerDied();

        }
        
    }
}
