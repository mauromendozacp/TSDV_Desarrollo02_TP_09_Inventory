using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private TMP_Text title;
    [SerializeField] private TMP_Text lifes;
    [SerializeField] private TMP_Text enemiesKilled;

    // Start is called before the first frame update
    void Start()
    {
        title.text = "YOU ";
        if (GameManager.Instance.Win)
            title.text += "WIN!";
        else
            title.text += "LOSE!";

        lifes.text = "Lives: " + GameManager.Instance.Lives;
        enemiesKilled.text = "Enemies Killed: " + GameManager.Instance.EnemiesKilled;
    }

    public void BackToMenu(string sceneName)
    {
        GameManager.Instance.RestartGame();
        GameManager.Instance.ChangeScene(sceneName);
    }
}
