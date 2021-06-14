using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseUI : MonoBehaviour
{
    public void Resume()
    {
        gameObject.SetActive(false);
        Time.timeScale = 1f;
        GameplayManager.GetInstance().Paused = false;
    }

    public void BackToMenu()
    {
        Time.timeScale = 1f;
        GameplayManager.GetInstance().Paused = false;

        SceneManager.LoadScene("MainMenu");
    }
}