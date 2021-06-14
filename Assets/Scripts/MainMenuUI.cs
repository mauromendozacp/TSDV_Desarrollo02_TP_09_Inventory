using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
    public void PlayGame(string sceneName)
    {
        GameManager.Instance.ChangeScene(sceneName);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
