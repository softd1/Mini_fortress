using System.Diagnostics;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;

    [HideInInspector] public int damage;
    [HideInInspector] public bool isFireBullet;
    [HideInInspector] public float burnDuration;
    [HideInInspector] public int burnDamagePerSecond;

    private Transform target;
    public void SetTarget(Transform targetTransform)
    {
        target = targetTransform;
    }
    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 direction = (target.position - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;
        if (Vector3.Distance(transform.position, target.position) < 0.2f)
        {
            HitTarget();
        }
    }
    void HitTarget()
    {
        EnemyHealth enemyHealth = target.GetComponent<EnemyHealth>();
        if (enemyHealth != null)
        {
            enemyHealth.TakeDamage(damage);
            UnityEngine.Debug.Log($"[Bullet] 기본 데미지 적용: {damage}");

            if (isFireBullet)
            {
                enemyHealth.ApplyBurn(burnDuration, burnDamagePerSecond);
                UnityEngine.Debug.Log($"[Bullet] 화상 도트 적용: {burnDuration}초 동안 초당 {burnDamagePerSecond} 데미지");
            }
        }
        Destroy(gameObject);
    }
}
