using UnityEngine;

public class TowerAttack : MonoBehaviour
{
    public float attackRange = 5f;
    public float attackRate = 1f;
    public GameObject bulletPrefab;

    public int baseDamage = 10;              // �⺻ ���� ������
    public bool isFireAttack = false;         // �� ���� ����
    public float fireBurnDuration = 3f;       // �� ���ӽð�
    public int fireBurnDamagePerSecond = 5;   // �� �ʴ� ������

    private float attackCooldown = 0f;
    void Update()
    {
        attackCooldown -= Time.deltaTime;

        GameObject targetEnemy = FindClosestEnemy();

        if (targetEnemy != null && attackCooldown <= 0f)
        {
            Attack(targetEnemy);
            attackCooldown = 1f / attackRate;
        }
    }

    GameObject FindClosestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        GameObject closest = null;
        float shortestDistance = Mathf.Infinity;

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance < shortestDistance && distance <= attackRange)
            {
                shortestDistance = distance;
                closest = enemy;
            }
        }
        return closest;
    }
    void Attack(GameObject enemy)
    {
        if (bulletPrefab != null)
        {
            GameObject bulletObj = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            Bullet bullet = bulletObj.GetComponent<Bullet>();
            if (bullet != null)
            {
                bullet.SetTarget(enemy.transform);

                bullet.damage = baseDamage;
                bullet.isFireBullet = isFireAttack;
                bullet.burnDuration = fireBurnDuration;
                bullet.burnDamagePerSecond = fireBurnDamagePerSecond;
            }
        }
    }
}
