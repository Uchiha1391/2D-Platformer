using UnityEngine;

[System.Serializable]
public class Sound
{
    public string soundName;
    public AudioClip soundClip;

    [Range(0f,1f)]
    public float volume = 0.7f;

    [Range(0.5f,1.5f)]
    public float pitch = 1f;

    [Range(0f,0.5f)]
    public float randomVolume = 0.1f;

    [Range(0f,0.5f)]
    public float randmPitch = 0.1f;

    public bool loop = false;

    private AudioSource soundSource;

    public void SetSource(AudioSource _source)
    {
        soundSource = _source;
        soundSource.clip = soundClip;
        soundSource.loop = loop;
    }

    public void Play()
    {
        soundSource.volume = volume * (1+Random.Range(-randomVolume/2f, +randomVolume/2f));
        soundSource.pitch = pitch;
        soundSource.Play();
    }

    public void Stop()
    {
        soundSource.Stop();
    }
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField]
    Sound[] sounds;

    void Awake()
    {
        if (instance != null)
        {
            if(instance!=this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                instance = this;
                DontDestroyOnLoad(this);
            }
        }
        else
        {
            instance = this;
        }
    }

    void Start()
    {
        for(int i = 0; i < sounds.Length; i++)
        {
            GameObject _go = new GameObject("Sound_ " + i + "_"+sounds[i].soundName);
            _go.transform.SetParent(this.transform);
            sounds[i].SetSource(_go.AddComponent<AudioSource>());
        }

        PlaySound("Music");
    }

    public void PlaySound(string _name)
    {
        for(int i = 0; i < sounds.Length; i++)
        {
            if(sounds[i].soundName==_name)
            {
                sounds[i].Play();
                return;
            }
        }

        //no sound with _name found
        Debug.LogWarning("AudioManager: Sound not found in sounds list. " + _name);
    }

    public void StopSound(string _name)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].soundName == _name)
            {
                sounds[i].Stop();
                return;
            }
        }

        //no sound with _name found
        Debug.LogWarning("AudioManager: Sound not found in sounds list. " + _name);
    }
}
