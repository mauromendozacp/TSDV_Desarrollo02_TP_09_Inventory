using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesManager : MonoBehaviour
{
    [SerializeField] GameObject[] enemyPrefab;
    [SerializeField] Vector3 circleOfMovement;
    [SerializeField] Transform playerTransform;
    List<Enemy> enemies;

    public delegate void OnNewEnemyCreatedDelegate(Enemy enemy);
    public OnNewEnemyCreatedDelegate onNewEnemyCreated;

    public delegate void OnAllEnemiesKilledDelegate(Player p, bool w);
    public OnAllEnemiesKilledDelegate onAllEnemiesKilled;

    public delegate void OnEnemiesAmountChangedDelegate(int enemiesAmount);
    public OnEnemiesAmountChangedDelegate onEnemiesAmountChanged;

    int amountEnemies = 10;
    Player player;

    void Start()
    {
        enemies = new List<Enemy>();
        player = FindObjectOfType<Player>();

        for (int i = 0; i < amountEnemies; i++)
        {
            int index = i % 2;
            Vector3 pos = circleOfMovement + new Vector3(Random.Range(-circleOfMovement.y, circleOfMovement.y), 0, Random.Range(-circleOfMovement.y, circleOfMovement.y));
            pos.y = enemyPrefab[index].transform.localScale.y / 2f + transform.position.y;

            GameObject enemy = Instantiate(enemyPrefab[index], pos, Quaternion.identity, transform);

            Enemy enemyComponent = enemy.GetComponent<Enemy>();

            enemyComponent.onDie += RemoveFromList;
            enemyComponent.circleOfMovement = circleOfMovement;
            enemyComponent.player = playerTransform;

            onNewEnemyCreated?.Invoke(enemyComponent);

            enemies.Add(enemyComponent);
        }

        onEnemiesAmountChanged?.Invoke(enemies.Count);
    }

    void RemoveFromList(Enemy enemy)
    {
        enemies.Remove(enemy);

        onEnemiesAmountChanged?.Invoke(enemies.Count);
        player.EnemiesKilled++;

        if (enemies.Count == 0)
        {
            onAllEnemiesKilled?.Invoke(player, true);
        }
    }
}
