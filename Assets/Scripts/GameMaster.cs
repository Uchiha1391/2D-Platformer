using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    public static GameMaster gM;

    void Awake()
    {
        if (gM == null)
            gM = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<GameMaster>();
    }

    public Transform playerPrefab;
    public Transform spawnPoint;
    public float spawnDelay = 2;
    public Transform spawnPrefab;
    public AudioSource respawnAudio;
    public CameraShake cameraShake;

    void Start()
    {
        if(cameraShake==null)
        {
            Debug.LogError("No camera shake referenced in Game Master");
        }
    }

    public IEnumerator RespawnPlayer()
    {
        respawnAudio = GetComponent<AudioSource>();
        respawnAudio.Play();
        yield return new WaitForSeconds(spawnDelay);
        Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
        Transform spawnParticleClone = Instantiate(spawnPrefab, spawnPoint.position, spawnPoint.rotation);
        Destroy(spawnParticleClone.gameObject, 3f);
        
    }

    public static void KillPlayer(Player player)
    {
        Destroy(player.gameObject);
        gM.StartCoroutine(gM.RespawnPlayer());
    }

    public static void KillEnemy(Enemy enemy)
    {
        gM._KillEnemy(enemy);
    }

    public void _KillEnemy(Enemy _enemy)
    {
        GameObject _clone = (GameObject)Instantiate(_enemy.deathParticles.gameObject, _enemy.transform.position, Quaternion.identity);
        Destroy(_clone, 5f);
        cameraShake.Shake(_enemy.shakeAmount,_enemy.shakeLength);
        Destroy(_enemy.gameObject);
    }
}
