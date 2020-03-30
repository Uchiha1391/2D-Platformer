using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        public void Init()
        {
            curHealth = maxHealth;
        }

    }

    public EnemyStats enemyStats = new EnemyStats();
    private float turnRed = 0.02f;
    private Color32 originalColour;
    private float timeToStopRed;

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
}
