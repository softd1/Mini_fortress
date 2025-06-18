using UnityEngine;

public class TowerAttack : MonoBehaviour
{
    [Header("공격 범위 & 속도")]
    public float attackRange = 5f;   // 사정거리
    public float attackRate = 1f;   // 초당 발사 횟수

    [Header("총알 & 총구")]
    public GameObject bulletPrefab;
    public Transform firePoint;     // 총알 생성 지점(빈 GameObject)

    [Header("데미지")]
    public int baseDamage = 10;

    private float nextFireTime = 0f;

    void Update()
    {
        // 1) 재장전(쿨다운) 체크
        if (Time.time < nextFireTime) return;

        // 2) 가장 가까운 적 찾기
        GameObject target = FindClosestEnemyInRange();
        if (target == null) return;

        // 3) 발사
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
            Debug.LogWarning("[TowerAttack] Bullet prefab 또는 firePoint가 할당되지 않음");
            return;
        }

        // 1) origin & target 높이 맞춤 (XZ 평면)
        Vector3 origin = firePoint.position;
        Vector3 targetPos = enemy.transform.position;
        targetPos.y = origin.y;

        // 2) 방향 계산
        Vector3 offset = targetPos - origin;
        Vector3 dir = offset.normalized;

        Debug.Log($"[TowerAttack] Shooting at enemy '{enemy.name}', dir = {dir}");

        // 3) 총알 생성 & 세팅
        var b = Instantiate(bulletPrefab, origin, Quaternion.identity);
        var bullet = b.GetComponent<Bullet>();
        if (bullet == null)
        {
            Debug.LogError("[TowerAttack] Bullet 컴포넌트가 없음");
            Destroy(b);
            return;
        }

        // 4) 충돌 무시 (타워 ↔ 총알)
        var towerCol = GetComponent<Collider>();
        var bulletCol = b.GetComponent<Collider>();
        if (towerCol && bulletCol)
            Physics.IgnoreCollision(bulletCol, towerCol);

        // 5) 기본 데미지 세팅
        bullet.damage = baseDamage;

        // 6) 발사
        bullet.Move(dir);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}