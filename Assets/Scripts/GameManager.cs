using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

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
        GameOver
    }

    public SceneGame Scene { get; set; }
    public int Lives { get; set; }
    public int EnemiesKilled { get; set; }
    public bool Win { get; set; }
    public bool GameOver { get; set; }

    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void RestartGame()
    {
        Lives = 0;
        EnemiesKilled = 0;
        Win = false;
        GameOver = false;
    }

    public void FinishGame(Player player, bool winGame)
    {
        Lives = player.Lives;
        EnemiesKilled = player.EnemiesKilled;
        Win = winGame;
        GameOver = true;
        ChangeScene("GameOver");
    }
}