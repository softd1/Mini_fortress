using UnityEngine;

public class DefaultEnemy : MonoBehaviour, IEnemy
{
    private int _health;
    private int _damage;
    private int _maxHealth;
    public int health
    {
        get { return _health; }
    }
    public int damage
    {
        get { return _damage; }
    }
    public int maxHealth
    {
        get { return _maxHealth; }
    }

    public void setHealth(int health)
    {
        _health = health;
    }

    public EnemyWayPoint getWaypoint()
    {
        return null;
    }
    public void setWaypoint(EnemyWayPoint waypoint)
    {

    }
    public float wayPointDistance()
    {
        return 0.0f;
    }
    public bool arriveAtWaypoint()
    {
        return false;
    }
    public void move()
    {

    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
