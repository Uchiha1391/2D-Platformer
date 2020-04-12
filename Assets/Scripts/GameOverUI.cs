using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    [SerializeField]
    private string mouseHoverSoundName = "ButtonHover";

    [SerializeField]
    private string mouseButtonPressName = "ButtonPress";

    private AudioManager audioManager;

    void Start()
    {
        audioManager = AudioManager.instance;
        if(audioManager==null)
        {
            Debug.LogError("No Audio Manager found in the scene");
        }
    }

    public void Quit()
    {
        audioManager.PlaySound(mouseButtonPressName);

        Debug.Log("APPLICATION QUIT!");
        Application.Quit();
    }

    public void Retry()
    {
        audioManager.PlaySound(mouseButtonPressName);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnMouseOver()
    {
        audioManager.PlaySound(mouseHoverSoundName);
    }
}
