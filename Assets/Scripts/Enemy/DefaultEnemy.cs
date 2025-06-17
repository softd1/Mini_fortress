using UnityEngine;

public class DefaultEnemy : MonoBehaviour
{
    private int _health;
    [SerializeField]
    private int _maxHealth;
    [SerializeField]
    private int baseSpeed;


    private Vector3 targetWayPoint;
    private EnemyManager manager;
    private int wayPointIndex;

    private EnemyEffectManager effectManager;
    private void Start()
    {
        effectManager = gameObject.GetComponent<EnemyEffectManager>();
    }

    public void takeDamage(int damage)
    {
        _health -= damage;
    }

    public EnemyWayPoint getWaypoint()
    {
        return null;
    }
    public float wayPointDistance()
    {
        return 0.0f;
    }

    public void move()
    {
        float spd = baseSpeed * (effectManager ? effectManager.speedMultiplier : 1f);
        transform.position =
            Vector3.MoveTowards(transform.position, targetWayPoint, spd * Time.deltaTime);
    }
    public void setManager(EnemyManager enemyManager)
    {
        // 만들때 호출
        manager = enemyManager;
        targetWayPoint = manager.getWayPoints(0);
    }

    private void arriveAtWaypoint()
    {
        wayPointIndex++;
        targetWayPoint = manager.getWayPoints(wayPointIndex);
    }

    void Update()
    {
        if (0.5f > Vector3.Distance(transform.position, targetWayPoint))
        {
            arriveAtWaypoint();
        }

        move();
    }
}
