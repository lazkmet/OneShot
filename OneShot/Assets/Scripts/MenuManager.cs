using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuManager : MonoBehaviour
{
    public LivesDisplay livesDisplay;
    public TextMeshProUGUI[] pointsDisplays = { };
    public Canvas[] screens;
    public bool pausable;
    public Canvas pauseScreen;
    //public PlayerMovement player; removing because I don't think it's used?
    public bool isPaused { get; private set; }
    public void DeactivateAll() {
        for (int i = 0; i < screens.Length; i++)
        {
            screens[i].gameObject.SetActive(false);
        }
    }
    public void SetActiveScreen(int i) {
        DeactivateAll();
        i = Mathf.Clamp(i, 0, screens.Length - 1);
        screens[i].gameObject.SetActive(true);
    }
    public void SetScene(string sceneName) {
        SceneManager.LoadScene(sceneName);
    }
    public void TogglePause()
    {
        if (pausable) {
            isPaused = !isPaused;
            if (isPaused)
            {
                Time.timeScale = 0;
                if (pauseScreen != null) {
                    pauseScreen.gameObject.SetActive(true);
                }
            }
            else {
                Time.timeScale = 1;
                if (pauseScreen != null)
                {
                    pauseScreen.gameObject.SetActive(false);
                }
            }
        }
    }
    public void Quit()
    {
        Application.Quit();
    }
    public void Reset()
    {
        if (screens.Length > 0)
        {
            SetActiveScreen(0);
        }
        isPaused = false;
        Time.timeScale = 1;
        if (pausable) { 
            pauseScreen.gameObject.SetActive(false);
        }
    }
    public void UpdateDisplay(int points = -1, int lives = -1)
    {
        if (!(lives < 0)) {
            livesDisplay.UpdateLives(lives);
        }
        if (!(points < 0)) {
            foreach (TextMeshProUGUI p in pointsDisplays) {
                p.text = points.ToString();
            }
        }

    }
}
