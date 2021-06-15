using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesManager : MonoBehaviour
{
    [SerializeField] GameObject[] enemyPrefab;
    [SerializeField] Vector3 circleOfMovement;
    [SerializeField] Transform player;
    List<Enemy> enemies;

    public delegate void OnNewEnemyCreatedDelegate(Enemy enemy);
    public OnNewEnemyCreatedDelegate onNewEnemyCreated;

    public delegate void OnAllEnemiesKilledDelegate();
    public OnAllEnemiesKilledDelegate onAllEnemiesKilled;

    public delegate void OnEnemiesAmountChangedDelegate(int enemiesAmount);
    public OnEnemiesAmountChangedDelegate onEnemiesAmountChanged;

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

            enemyComponent.onDie += RemoveFromList;
            enemyComponent.circleOfMovement = circleOfMovement;
            enemyComponent.player = player;

            onNewEnemyCreated?.Invoke(enemyComponent);

            enemies.Add(enemyComponent);
        }

        onEnemiesAmountChanged?.Invoke(enemies.Count);
    }

    void RemoveFromList(Enemy enemy)
    {
        enemies.Remove(enemy);

        onEnemiesAmountChanged?.Invoke(enemies.Count);

        if (enemies.Count == 0)
        {
            onAllEnemiesKilled?.Invoke();
        }
    }
}
