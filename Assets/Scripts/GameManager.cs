using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public int MainMenuScene { get; } = 0;
    public int GamePlayScene { get; } = 1;
    public int DeathScene { get; } = 2;
    public int WinScene { get; } = 3;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public enum SceneGame
    {
        MainMenu,
        GamePlay,
        DeathScene,
        WinScene
    }

    public SceneGame Scene { get; set; }
    public bool Win { get; set; } = false;
    public bool GameOver { get; set; }

    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void RestartGame()
    {
        GameOver = false;
        Win = false;
    }
}