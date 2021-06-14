using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class UIGameplay : MonoBehaviour
{
    [SerializeField] Image[] lives;
    [SerializeField] TextMeshProUGUI enemies;

    public void UpdateLives(int amountLives)
    {
        int maxLives = 5;
        if (amountLives > maxLives)
            amountLives = maxLives;
        else if (amountLives < 0)
            amountLives = 0;

        for (int i = 0; i < maxLives; i++)
        {
            if (lives[i].enabled)
                lives[i].enabled = false;
        }

        for (int i = 0; i < amountLives; i++)
        {
            lives[i].enabled = true;
        }
    }

    public void UpdateEnemiesAmount(int amountEnemies)
    {
        if (amountEnemies < 0)
            amountEnemies = 0;

        enemies.text = amountEnemies.ToString();
    }
}
