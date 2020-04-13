using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    public static GameMaster gM;

    [SerializeField]
    private int maxLives = 3;

    private static int remainingPlayerLives;
    
    public static int RemainingLives
    {
        get { return remainingPlayerLives; }
        
    }

    void Awake()
    {
        remainingPlayerLives = maxLives;
        if (gM == null)
            gM = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<GameMaster>();
    }

    public Transform playerPrefab;
    public Transform spawnPoint;
    public float spawnDelay = 2;
    public Transform spawnPrefab;
    public AudioSource respawnAudio;
    public CameraShake cameraShake;
    public string playerDeathSoundName = "Death";
    public string playerSpawnSoundName = "Spawn";
    public string gameOverSoundName = "GameOver";

    [SerializeField]
    private GameObject gameOverUI;

    [SerializeField]
    private GameObject upgradeMenu;

    public delegate void UpgradeMenuCallBack(bool active);
    public UpgradeMenuCallBack onToggleUpgradeMenu;

    //cache
    private AudioManager audioManager;

    void Start()
    {
        if(cameraShake==null)
        {
            Debug.LogError("No camera shake referenced in Game Master");
        }
        remainingPlayerLives = maxLives;

        //caching
        audioManager = AudioManager.instance;
        if(audioManager==null)
        {
            Debug.LogError("No Audio Manager found in the scene");
        }
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.U))
        {
            ToggleUpgradeMenu();
        }
    }

    private void ToggleUpgradeMenu()
    {
        upgradeMenu.SetActive(!upgradeMenu.activeSelf);
        onToggleUpgradeMenu.Invoke(upgradeMenu.activeSelf);
        GetComponent<WaveSpawner>().enabled = !upgradeMenu.activeSelf;
    }

    public void EndGame()
    {
        audioManager.PlaySound(gameOverSoundName);
        audioManager.StopSound("Music");

        Debug.Log("Game Over");
        gameOverUI.SetActive(true);
    }

    public IEnumerator RespawnPlayer()
    {
        audioManager.PlaySound(playerDeathSoundName);
        yield return new WaitForSeconds(spawnDelay);
        audioManager.PlaySound(playerSpawnSoundName);

        Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
        Transform spawnParticleClone = Instantiate(spawnPrefab, spawnPoint.position, spawnPoint.rotation);
        Destroy(spawnParticleClone.gameObject, 3f);
        
    }

    public static void KillPlayer(Player player)
    {
        Destroy(player.gameObject);
        remainingPlayerLives--;
        if(remainingPlayerLives<=0)
        {
            gM.EndGame();
        }
        else
        {
            gM.StartCoroutine(gM.RespawnPlayer());
        }
    }

    public static void KillEnemy(Enemy enemy)
    {
        gM._KillEnemy(enemy);
    }

    public void _KillEnemy(Enemy _enemy)
    {
        audioManager.PlaySound(_enemy.deathSoundName);

        //Add Particles
        GameObject _clone = (GameObject)Instantiate(_enemy.deathParticles.gameObject, _enemy.transform.position, Quaternion.identity);
        Destroy(_clone, 5f);

        //Camera Shake
        cameraShake.Shake(_enemy.shakeAmount,_enemy.shakeLength);
        Destroy(_enemy.gameObject);
    }
}
