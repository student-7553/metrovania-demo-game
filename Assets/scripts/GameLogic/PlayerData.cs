using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
// using SimpleJSON;
using SimpleJSON;

public class playerFloatResourceClass
{
    public float currentHealth;
    public float maximumHealth;
    public float currentMana;
    public float maximumMana;
    public float currentBaseAttackDamage;
    public float baseAttackDamage;
    public float currentSpiritRangedAttackDamage;
    public float currentSpiritMeleeAttackDamage;

}

public class playerBoolUpgrades
{
    public bool isDashAvailable;
    public bool isAttackAvailable;
    public bool isHealAvailable;
    public bool isRangedSpiritAttackAvailable;
    public bool isDeflectAvailable;
    public bool iSpiritMeleeAttackAvailable;
    public bool isSpiritDashAvailable;
    public bool isDrillAvailable;
    public bool isMarkAvailable;
    public bool isDiveAvailable;

}
public class playerResourceUsageClass
{
    public float lance;
    public float focusStrike;
    public float focusDash;
    public float focusDive;
    public float deflect;
}
public class PlayerData : MonoBehaviour
{
    static PlayerData current;
    static public bool isAlive;

    // death,trap
    static public string lastDeath;

    static public playerResourceUsageClass playerResourceUsage;

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




    void Start()
    {
        if (current != null && current != this)
        {
            Destroy(gameObject);
            return;
        }
        current = this;
        DontDestroyOnLoad(gameObject);


        // foreach(JSONNode record in data["startingPlayerFloatResource"])
        // {
        //     Debug.Log ("nombre: " + record["nombre"].Value + "score: " + record["puntos"].AsInt);
        // }
        // string jsonString = File.ReadAllText (path); 
        // Debug.Log(jsonString);
        // playerFloatResourceClass listaRecord = JsonUtility.FromJson<playerFloatResourceClass> (jsonString);
        // // print(listaRecord);
        // Debug.Log(listaRecord.currentHealth);



        initPlayerData();
    }

    void initPlayerData()
    {


        string path = Application.dataPath + "/Json/PlayerData.json";
        string jsonString = File.ReadAllText(path);
        JSONNode data = JSON.Parse(jsonString);

        isAlive = true;
        lastDeath = "death";

        playerResourceUsage = new playerResourceUsageClass();
        playerResourceUsage.lance = data["playerResourceUsage"]["lance"];
        playerResourceUsage.focusStrike = data["playerResourceUsage"]["focusStrike"];
        playerResourceUsage.focusDash = data["playerResourceUsage"]["focusDash"];
        playerResourceUsage.focusDive = data["playerResourceUsage"]["focusDive"];
        playerResourceUsage.deflect = data["playerResourceUsage"]["deflect"];;

        m_playerFloatResources = new playerFloatResourceClass();
        m_playerFloatResources.currentHealth = data["startingPlayerFloatResource"]["currentHealth"];
        m_playerFloatResources.maximumHealth = data["startingPlayerFloatResource"]["maximumHealth"];
        m_playerFloatResources.currentMana = data["startingPlayerFloatResource"]["currentMana"];
        m_playerFloatResources.maximumMana = data["startingPlayerFloatResource"]["maximumMana"];
        m_playerFloatResources.currentBaseAttackDamage = data["startingPlayerFloatResource"]["currentBaseAttackDamage"];
        m_playerFloatResources.baseAttackDamage = data["startingPlayerFloatResource"]["baseAttackDamage"];
        m_playerFloatResources.currentSpiritRangedAttackDamage = data["startingPlayerFloatResource"]["currentSpiritRangedAttackDamage"];
        m_playerFloatResources.currentSpiritMeleeAttackDamage = data["startingPlayerFloatResource"]["currentSpiritMeleeAttackDamage"];



        m_playerBoolUpgrades = new playerBoolUpgrades();
        m_playerBoolUpgrades.isDashAvailable = true;
        m_playerBoolUpgrades.isAttackAvailable = true;
        m_playerBoolUpgrades.isHealAvailable = true;
        m_playerBoolUpgrades.isRangedSpiritAttackAvailable = true;
        m_playerBoolUpgrades.isDeflectAvailable = true;
        m_playerBoolUpgrades.iSpiritMeleeAttackAvailable = true;
        m_playerBoolUpgrades.isSpiritDashAvailable = true;
        m_playerBoolUpgrades.isDrillAvailable = true;
        m_playerBoolUpgrades.isMarkAvailable = true;
        m_playerBoolUpgrades.isDiveAvailable = true;


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
