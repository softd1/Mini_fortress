using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform[] waypoints;
    public float spawnInterval = 1f; // 1�ʸ��� ����

    void Start()
    {
        InvokeRepeating(nameof(SpawnEnemy), 0f, spawnInterval);
    }

    void SpawnEnemy()
    {
        GameObject enemy = Instantiate(enemyPrefab, transform.position, Quaternion.identity);
        EnemyWayPoint enemyWayPoint = enemy.GetComponent<EnemyWayPoint>();
        enemyWayPoint.waypoints = waypoints;
    }

}
