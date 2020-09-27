using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public GameObject PauseMenu;
    public bool IsPaused;
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

    }

    public void ChangeControls()
    {

    }
}
