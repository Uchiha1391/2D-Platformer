using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyAI))]
[RequireComponent(typeof(WaveSpawner))]
[RequireComponent(typeof(WaveUI))]
public class Enemy : MonoBehaviour
{
    [System.Serializable]
    public class EnemyStats
    {
        public int maxHealth = 100;
        private int _curHealth;
        public int curHealth
        {
            get { return _curHealth; }
            set { _curHealth = Mathf.Clamp(value,0,maxHealth); }
        }

        public int damage = 40;

        

        public void Init()
        {
            curHealth = maxHealth;
        }

    }

    public EnemyStats enemyStats = new EnemyStats();
    private float turnRed = 0.02f;
    private Color32 originalColour;
    private float timeToStopRed;

    public Transform deathParticles;
    public float shakeAmount = 0.1f;
    public float shakeLength = 0.1f;

    public string deathSoundName = "Explosion";

    [Header("Optional:")]
    [SerializeField]
    private StatusIndicator statusIndicator;

    void Start()
    {
        enemyStats.Init();
        originalColour = statusIndicator.GetColour();
        if (statusIndicator != null)
        {
            statusIndicator.SetHealth(enemyStats.curHealth, enemyStats.maxHealth);
        }

        GameMaster.gM.onToggleUpgradeMenu += OnUpgradeMenuToggle;

        if(deathParticles==null)
        {
            Debug.LogError("No death particles referenced to enemy");
        }
    }

    void OnUpgradeMenuToggle(bool activeState)
    {
        GetComponent<EnemyAI>().enabled = !activeState;
        GetComponent<WaveUI>().enabled = !activeState;
        GetComponent<WaveSpawner>().enabled = !activeState;
    }

    public void DamageEnemy(int damage)
    {
        enemyStats.curHealth -= damage;
        if (enemyStats.curHealth > 30 && enemyStats.curHealth < 70)
            originalColour = new Color32(255, 165, 0, 255);
        else if (enemyStats.curHealth < 40)
            originalColour = new Color32(255, 0, 0, 255);
        if(enemyStats.curHealth<=0)
        {
            GameMaster.KillEnemy(this);
        }
        if (statusIndicator != null)
        {
            statusIndicator.SetHealth(enemyStats.curHealth, enemyStats.maxHealth);
            timeToStopRed = Time.time + turnRed;
            statusIndicator.SetColour(255, 0, 0);
        }
    }
    void Update()
    {
        if(timeToStopRed<Time.time)
            statusIndicator.SetColour(originalColour.r, originalColour.g, originalColour.b, originalColour.a);
    }

    void OnCollisionEnter2D(Collision2D colliderInfo)
    {
        Player _player = colliderInfo.collider.GetComponent < Player > ();
        if(_player != null)
        {
            _player.DamagePlayer(enemyStats.damage);
            DamageEnemy(99999);
        }
    }
}
