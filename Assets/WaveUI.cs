using UnityEngine;
using UnityEngine.UI;

public class WaveUI : MonoBehaviour
{
    [SerializeField]
    WaveSpawner spawner;

    [SerializeField]
    Animator waveAnimator;

    [SerializeField]
    Text waveCountdownText;

    [SerializeField]
    Text waveCountText;

    private WaveSpawner.SpawnState prevState;

    // Start is called before the first frame update
    void Start()
    {
        if(spawner==null)
        {
            Debug.LogError("No spawner referenced!");
            this.enabled = false;
        }
        if (waveCountText == null)
        {
            Debug.LogError("No wave count text referenced!");
            this.enabled = false;
        }
        if (waveAnimator == null)
        {
            Debug.LogError("No wave animator referenced!");
            this.enabled = false;
        }
        if (waveCountdownText == null)
        {
            Debug.LogError("No wave countdown text referenced!");
            this.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        switch(spawner.State)
        {
            case WaveSpawner.SpawnState.COUNTING:
                UpdateCountingUI();
                break;
            case WaveSpawner.SpawnState.SPAWNING:
                UpdateSpawningUI();
                break;
        }

        prevState = spawner.State;
    }

    void UpdateCountingUI()
    {
        if(prevState!=WaveSpawner.SpawnState.COUNTING)
        {
            waveAnimator.SetBool("WaveIncoming", false);
            waveAnimator.SetBool("WaveCountdown", true);
            Debug.Log("Counting");
        }
        waveCountdownText.text = ((int)spawner.WaveCountdown).ToString();
    }

    void UpdateSpawningUI()
    {
        if(prevState!=WaveSpawner.SpawnState.SPAWNING)
        {
            waveAnimator.SetBool("WaveIncoming", true);
            waveAnimator.SetBool("WaveCountdown", false);
            waveCountText.text = (spawner.NextWave+1).ToString();
            Debug.Log("Spawning");
        }

    }
}
