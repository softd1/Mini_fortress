interface IEnemy
{
    int maxHealth { get; }
    int health { get; }
    void setHealth(int health);

    int damage { get; }

    EnemyWayPoint getWaypoint();
    void setWaypoint(EnemyWayPoint waypoint);
    float wayPointDistance();
    bool arriveAtWaypoint();
    void move();
}
