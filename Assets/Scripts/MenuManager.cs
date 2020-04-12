using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField]
    private string hoverOverSound = "ButtonHover";

    [SerializeField]
    private string pressButonSound = "ButtonPress";

    AudioManager audioManager;


    void Start()
    {
        audioManager = AudioManager.instance;
        if(audioManager==null)
        {
            Debug.LogError("No AudioManager Found");
        }
    }

    public void StartGame()
    {
        audioManager.PlaySound(pressButonSound);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        audioManager.PlaySound(pressButonSound);
        Debug.Log("GAME QUIT!");
        Application.Quit();
    }

    public void OnMouseOver()
    {
        audioManager.PlaySound(hoverOverSound);
    }
}
