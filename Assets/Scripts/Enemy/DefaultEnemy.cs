using UnityEngine;

public class DefaultEnemy : MonoBehaviour
{
    private int _health;
    [SerializeField]
    private int _maxHealth;
    [SerializeField]
    private int _speed;


    private Vector3 targetWayPoint;
    private EnemyManager manager;
    private int wayPointIndex;



    public void setHealth(int health)
    {
        _health = health;
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
        transform.position = Vector3.MoveTowards(transform.position, targetWayPoint, _speed * Time.deltaTime);
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

    void Start()
    {

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
