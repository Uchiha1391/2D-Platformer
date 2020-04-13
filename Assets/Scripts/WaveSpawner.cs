using System.Collections;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    public enum SpawnState {SPAWNING,WAITING,COUNTING};

    [System.Serializable]
    public class Wave
    {
        public string name;
        public Transform enemy;
        public int count;
        public float spawnRate;
    }

    public Wave[] waves;
    public Transform[] spawnPoints;
    public float timeBetweenWaves = 3f;


    private float waveCountdown;

    public float WaveCountdown
    {
        get { return waveCountdown; }
    }

    private int nextWave = 0;
    public int NextWave
    {
        get { return nextWave; }
    }

    private SpawnState spawnState = SpawnState.COUNTING;
    private float searchCountdown=1f;

    public SpawnState State
    {
        get { return spawnState;  }
    }

    void Start()
    {
        if (spawnPoints.Length == 0)
        {
            Debug.LogError("Error: No spawn points detected for enemies");
        }
        waveCountdown = timeBetweenWaves;
    }

    void Update()
    {
        if (!Enabled())
            return;
        if(spawnState == SpawnState.WAITING)
        {
            if(!isEnemyAlive())
            {
                WaveCompleted();
            }
            else
            {
                return;
            }
        }

        if(waveCountdown<=0)
        {
            if (spawnState != SpawnState.SPAWNING)
            {
                if(Enabled())
                    StartCoroutine(SpawnWave(waves[nextWave]));
            }

        }
        else
        {
            waveCountdown -= Time.deltaTime;
        }
    }

    bool Enabled()
    {
        return this.enabled;
    }

    void WaveCompleted()
    {
        Debug.Log("Wave Completed!");

        spawnState = SpawnState.COUNTING;
        waveCountdown = timeBetweenWaves;

        if (nextWave + 1 == waves.Length)
        {
            nextWave = 0;
            Debug.Log("All waves complete!!!! Looping...");

        }
        else
        {
            nextWave++;
        }
    }

    bool isEnemyAlive()
    {
        if (!Enabled())
            return true;
        searchCountdown -= Time.deltaTime;
        if (searchCountdown <= 0f)
        {
            searchCountdown = 1f;
            if (GameObject.FindGameObjectWithTag("Enemy") == null)
                return false;
        }
        return true;
    }

    IEnumerator SpawnWave(Wave _wave)
    {

        Debug.Log("Spawning wave: "+_wave.name);
        spawnState = SpawnState.SPAWNING;

        for(int i = 0; i < _wave.count;i++)
        {
            if (SpawnEnemy(_wave.enemy) == false)
                i--;
            yield return new WaitForSeconds(1f / _wave.spawnRate);
        }

        spawnState = SpawnState.WAITING;

        yield return new WaitUntil(Enabled);
        yield break;
    }

    bool SpawnEnemy(Transform _enemy)
    {
        if (!Enabled())
            return false;
        Debug.Log("Spawning Enemy: " + _enemy.name);

        Transform _spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        Instantiate(_enemy, _spawnPoint.transform.position, _spawnPoint.transform.rotation);
        return true;
    }
}
