using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class playerFloatResourceClass
{
    public float currentHealth;
    public float maximumHealth;
    public float currentMana;
    public float maximumMana;
    public float currentBaseAttackDamage;
    public float baseAttackDamage;

}

public class playerBoolUpgrades
{
    public bool isDashAvailable;
    public bool isAttackAvailable;
    public bool isHealAvailable;
    public bool isRangedSpiritAttackAvailable;
    public bool isDeflectAvailable;
    public bool isHeavySpiritAttackAvailable;
    public bool isSpiritDashAvailable;
    public bool isDrillAvailable;
    public bool isMarkAvailable;
    public bool isDiveAvailable;

}
public class PlayerData : MonoBehaviour
{
    static PlayerData current;

    static public bool isAlive;



    private playerFloatResourceClass m_playerFloatResources;

    public static playerFloatResourceClass playerFloatResources
    {
        get { return current.m_playerFloatResources; }
        set
        {
            if (current == null)
                return;

            current.m_playerFloatResources = value;
        }
    }

    private playerBoolUpgrades m_playerBoolUpgrades;

    public static playerBoolUpgrades playerBoolUpgrades
    {
        get { return current.m_playerBoolUpgrades; }
        set
        {
            if (current == null)
                return;

            current.m_playerBoolUpgrades = value;
        }
    }


    public class playerCollectables
    {

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

    void initPlayerData()
    {

        isAlive = true;

        m_playerFloatResources = new playerFloatResourceClass();
        m_playerFloatResources.currentHealth = 30;
        m_playerFloatResources.maximumHealth = 30;
        m_playerFloatResources.currentMana = 50;
        m_playerFloatResources.maximumMana = 50;
        m_playerFloatResources.currentBaseAttackDamage = 10;
        m_playerFloatResources.baseAttackDamage = 10;



        m_playerBoolUpgrades = new playerBoolUpgrades();
        m_playerBoolUpgrades.isDashAvailable = true;
        m_playerBoolUpgrades.isAttackAvailable = true;
        m_playerBoolUpgrades.isHealAvailable = false;
        m_playerBoolUpgrades.isRangedSpiritAttackAvailable = false;
        m_playerBoolUpgrades.isDeflectAvailable = false;
        m_playerBoolUpgrades.isHeavySpiritAttackAvailable = false;
        m_playerBoolUpgrades.isSpiritDashAvailable = true;
        m_playerBoolUpgrades.isDrillAvailable = false;
        m_playerBoolUpgrades.isMarkAvailable = false;
        m_playerBoolUpgrades.isDiveAvailable = false;
        
        
       
    }

    // Update is called once per frame
    void Update()
    {


        // check if player is alive
        if (m_playerFloatResources.currentHealth <= 0)
        {
            // isAlive = false;
            GameManager.PlayerDied();

        }

    }
}
