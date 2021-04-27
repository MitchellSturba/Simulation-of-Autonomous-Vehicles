using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject pauseMenuUI;
    public GameObject UISystem;
    // Start is called before the first frame update

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P)) {
            if(GameIsPaused == true) {
                Resume();
            }
            else {
                Pause();
            }
        }
    }

    public void Resume() {
        pauseMenuUI.SetActive(false);
        UISystem.SetActive(true);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    public void Pause() {
        pauseMenuUI.SetActive(true);
        UISystem.SetActive(false);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }
}
