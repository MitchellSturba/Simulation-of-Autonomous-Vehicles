using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class SceneLoader : MonoBehaviour
{
    // the numbers associated with each scene below was done by adding them to the scene build settings in Unity (File > Build Settings)
    public void LoadMainMenu() {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
    public void LoadTrackSelection() {
        Time.timeScale = 1f;
        SceneManager.LoadScene(1);
    }
    public void LoadWindridgeCity() {
        Time.timeScale = 1f;
        SceneManager.LoadScene(2);
    }
    public void LoadMiniCity() {
        Time.timeScale = 1f;
        SceneManager.LoadScene(3);
    }
    public void LoadLoopTrack() {
        Time.timeScale = 1f;
        SceneManager.LoadScene(4);
    }
    public void LoadLoopTrackAutonomous() {
        Time.timeScale = 1f;
        SceneManager.LoadScene(5);
    }
}
