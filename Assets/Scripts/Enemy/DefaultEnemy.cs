using UnityEngine;

public class DefaultEnemy : MonoBehaviour
{
    [SerializeField] private int baseHealth;
    [SerializeField] private int baseSpeed;
    [SerializeField] private int baseValue;
    [SerializeField] private int baseDefense;

    private int _health;
    private EnemyEffectManager effectManager;
    private EnemyManager manager;
    private Vector3 targetWayPoint;
    private int wayPointIndex;

    void Start()
    {
        _health = baseHealth;
        effectManager = GetComponent<EnemyEffectManager>();
    }

    public void takeDamage(int damage, bool fixedDamage)
    {
        int finalDamage = damage;
        if (!fixedDamage && effectManager != null)
        {
            finalDamage = damage < baseDefense
                          ? damage / 2
                          : Mathf.RoundToInt((damage - baseDefense) * effectManager.damageMultiplier);
        }
        _health -= finalDamage;
        Debug.Log($"{name} took {finalDamage}, remaining {_health}");

        if (_health <= 0)
            Remove();
    }

    public void applyEffect(StatusEffect effect)
    {
        if (effectManager != null)
            effectManager.applyEffect(effect);
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
        manager = enemyManager;
        targetWayPoint = manager.getWayPoints(0);
    }

    private void arriveAtWaypoint()
    {
        wayPointIndex++;
        targetWayPoint = manager.getWayPoints(wayPointIndex);
    }

    private void Remove()
    {
        if (GoldManager.Instance != null)
        {
            GoldManager.Instance.AddGold(baseValue);
            Debug.Log($"[Enemy] {name} 처치, 골드 +{baseValue}");
        }
        else
        {
            Debug.LogWarning("[Enemy] 골드 매니저가 없습니다!");
        }

        Destroy(gameObject);
    }

    void Update()
    {
        if (0.5f > Vector3.Distance(transform.position, targetWayPoint))
        {
            arriveAtWaypoint();
        }

        move();

        if (_health < 0)
            Remove();
    }
}