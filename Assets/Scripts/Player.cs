using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets._2D;

[RequireComponent(typeof(Platformer2DUserControl))]
public class Player : MonoBehaviour
{

    public int fallBoundary = -20;

    public string damageSoundName = "Grunt";

    private AudioManager audioManager;

    [SerializeField]
    private StatusIndicator statusIndicator;

    private PlayerStats playerStats;

    void Start()
    {
        playerStats = PlayerStats.instance;

        playerStats.curHealth = playerStats.maxHealth;

        if(statusIndicator == null)
        {
            Debug.LogError("No Status Indicator referenced on Player");
        }
        else
        {
            statusIndicator.SetHealth(playerStats.curHealth, playerStats.maxHealth);
        }

        GameMaster.gM.onToggleUpgradeMenu += OnUpgradeMenuToggle;

        audioManager = AudioManager.instance;
        if(audioManager==null)
        {
            Debug.LogError("No AudioManager found in the scene");
        }

        InvokeRepeating("RegenHealth", playerStats.healthRegenRate, playerStats.healthRegenRate);
    }

    void RegenHealth()
    {
        playerStats.curHealth+=1;
        statusIndicator.SetHealth(playerStats.curHealth, playerStats.maxHealth);
    }

    void Update()
    {
        if(transform.position.y<=fallBoundary)
        {
            DamagePlayer(9999999);
        }
    }

    void OnUpgradeMenuToggle(bool activeState)
    {
        GetComponent<Platformer2DUserControl>().enabled = !activeState;
        Weapon _weapon = GetComponentInChildren<Weapon>();
        ArmRotation _arm = GetComponentInChildren<ArmRotation>();
        if (_weapon!=null)
        {
            _weapon.enabled = !activeState;
            
        }
        if(_arm!=null)
        {
            _arm.enabled = !activeState; 
        }
    }

    public void DamagePlayer(int damage)
    {
        playerStats.curHealth -= damage;
        if (playerStats.curHealth <= 0)
        {
            GameMaster.KillPlayer(this);
        }
        else
        {
            audioManager.PlaySound(damageSoundName);
        }
        statusIndicator.SetHealth(playerStats.curHealth, playerStats.maxHealth);
    }

    void OnDestroy()
    {
        GameMaster.gM.onToggleUpgradeMenu -= OnUpgradeMenuToggle;
    }

}
