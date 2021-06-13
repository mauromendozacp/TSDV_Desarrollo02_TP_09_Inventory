using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesManager : MonoBehaviour
{
    [SerializeField] GameObject[] enemyPrefab;
    [SerializeField] Vector3 circleOfMovement;
    [SerializeField] Transform player;
    List<Enemy> enemies;

    public delegate void OnAllEnemiesKilledDelegate();
    public OnAllEnemiesKilledDelegate onAllEnemiesKilled;

    int amountEnemies = 10;

    void Start()
    {
        enemies = new List<Enemy>();

        for (int i = 0; i < amountEnemies; i++)
        {
            int index = i % 2;
            Vector3 pos = circleOfMovement + new Vector3(Random.Range(-circleOfMovement.y, circleOfMovement.y), 0, Random.Range(-circleOfMovement.y, circleOfMovement.y));
            pos.y = enemyPrefab[index].transform.localScale.y / 2f + transform.position.y;

            GameObject enemy = Instantiate(enemyPrefab[index], pos, Quaternion.identity, transform);

            Enemy enemyComponent = enemy.GetComponent<Enemy>();

            enemyComponent.onAttackPlayer += GameplayManager.GetInstance().SetEnemyAttack;
            enemyComponent.onDie += RemoveFromList;
            enemyComponent.circleOfMovement = circleOfMovement;
            enemyComponent.player = player;

            enemies.Add(enemyComponent);
        }
    }

    void RemoveFromList(Enemy enemy)
    {
        enemies.Remove(enemy);

        if (enemies.Count == 0)
        {
            onAllEnemiesKilled?.Invoke();
        }
    }
}
