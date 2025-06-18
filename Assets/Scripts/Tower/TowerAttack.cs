using UnityEngine;

public class TowerAttack : MonoBehaviour
{
    [Header("���� ���� & �ӵ�")]
    public float attackRange = 5f;   // �����Ÿ�
    public float attackRate = 1f;   // �ʴ� �߻� Ƚ��

    [Header("�Ѿ� & �ѱ�")]
    public GameObject bulletPrefab;
    public Transform firePoint;     // �Ѿ� ���� ����(�� GameObject)

    [Header("������")]
    public int baseDamage = 10;

    private float nextFireTime = 0f;

    void Update()
    {
        // 1) ������(��ٿ�) üũ
        if (Time.time < nextFireTime) return;

        // 2) ���� ����� �� ã��
        GameObject target = FindClosestEnemyInRange();
        if (target == null) return;

        // 3) �߻�
        Shoot(target);
        nextFireTime = Time.time + 1f / attackRate;
    }

    GameObject FindClosestEnemyInRange()
    {
        var enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject best = null;
        float bestDistSqr = attackRange * attackRange;

        foreach (var e in enemies)
        {
            float dSqr = (e.transform.position - transform.position).sqrMagnitude;
            if (dSqr <= bestDistSqr)
            {
                bestDistSqr = dSqr;
                best = e;
            }
        }
        return best;
    }

    void Shoot(GameObject enemy)
    {
        if (bulletPrefab == null || firePoint == null)
        {
            Debug.LogWarning("[TowerAttack] Bullet prefab �Ǵ� firePoint�� �Ҵ���� ����");
            return;
        }

        // 1) origin & target ���� ���� (XZ ���)
        Vector3 origin = firePoint.position;
        Vector3 targetPos = enemy.transform.position;
        targetPos.y = origin.y;

        // 2) ���� ���
        Vector3 offset = targetPos - origin;
        Vector3 dir = offset.normalized;

        Debug.Log($"[TowerAttack] Shooting at enemy '{enemy.name}', dir = {dir}");

        // 3) �Ѿ� ���� & ����
        var b = Instantiate(bulletPrefab, origin, Quaternion.identity);
        var bullet = b.GetComponent<Bullet>();
        if (bullet == null)
        {
            Debug.LogError("[TowerAttack] Bullet ������Ʈ�� ����");
            Destroy(b);
            return;
        }

        // 4) �浹 ���� (Ÿ�� �� �Ѿ�)
        var towerCol = GetComponent<Collider>();
        var bulletCol = b.GetComponent<Collider>();
        if (towerCol && bulletCol)
            Physics.IgnoreCollision(bulletCol, towerCol);

        // 5) �⺻ ������ ����
        bullet.damage = baseDamage;

        // 6) �߻�
        bullet.Move(dir);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}