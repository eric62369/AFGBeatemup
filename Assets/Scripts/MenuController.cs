using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public GameObject PauseMenu;
    public static bool IsPaused { get; private set; }
    private float previousTimeScale;

    // Start is called before the first frame update
    void Start()
    {
        IsPaused = false;
        previousTimeScale = 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PauseGame()
    {
        IsPaused = !IsPaused;
        PauseMenu.SetActive(IsPaused);
        if (IsPaused)
        {
            previousTimeScale = Time.timeScale;
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = previousTimeScale;
        }
    }

    public void ResumeGame()
    {
        PauseGame();
    }

    public void QuitGame()
    {
        ResumeGame(); // Exit Pause Menu time freeze
        // SceneManager.Load(MainMenuScene);
    }

    public void ChangeControls()
    {

    }
}
