using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class MenuController : MonoBehaviour
{
    public GameObject PauseMenu;
    public static bool IsPaused { get; private set; }
    private float previousTimeScale;
    // Freezing inputs
    public InputActionAsset PlayerInputActions;
    private InputActionMap fighterActionMap;
    private InputActionMap uiActionMap;

    // Start is called before the first frame update
    void Start()
    {
        IsPaused = false;
        previousTimeScale = 0.5f;
        fighterActionMap = PlayerInputActions.FindActionMap("Fighter");
        uiActionMap = PlayerInputActions.FindActionMap("UI");
    }

    public void PauseGame()
    {
        IsPaused = !IsPaused;
        PauseMenu.SetActive(IsPaused);
        if (IsPaused)
        {
            previousTimeScale = Time.timeScale;
            Time.timeScale = 0f;
            fighterActionMap.Disable();
            uiActionMap.Enable();
        }
        else
        {
            Time.timeScale = previousTimeScale;
            fighterActionMap.Enable();
            uiActionMap.Disable();
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
