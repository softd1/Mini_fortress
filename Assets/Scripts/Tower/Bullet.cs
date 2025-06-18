using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class Bullet : MonoBehaviour
{
    [Header("기본 속성")]
    public float speed = 10f;
    public float lifeTime = 2f;
    public int damage = 10;

    [Header("화상 (Burn)")]
    public bool appliesBurn = false;
    public float burnDuration = 3f;
    public int burnDamagePerSecond = 5;

    [Header("빙결 (Slow)")]
    public bool appliesFreeze = false;
    public float freezeDuration = 2f;
    public float freezeSlowAmount = 0.15f;  // 속도감소 비율 (예: 0.15 → 15%)

    [Header("감전 (Shock)")]
    public bool appliesShock = false;
    public float shockDuration = 1f;

    private Vector3 moveDir;

    void Awake()
    {
        Destroy(gameObject, lifeTime);

        var rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        rb.useGravity = false;

        var col = GetComponent<Collider>();
        col.isTrigger = true;
    }

    public void Move(Vector3 dir)
    {
        moveDir = dir.normalized;
    }

    void Update()
    {
        transform.position += moveDir * speed * Time.deltaTime;
    }

    void OnTriggerEnter(Collider other)
    {
        var enemy = other.GetComponent<Enemy>();
        var effects = other.GetComponent<EnemyEffectManager>();
        if (enemy == null) return;

        // 즉시 데미지
        enemy.takeDamage(damage, fixedDamage: false);

        // 상태이상 적용
        if (effects != null)
        {
            if (appliesBurn)
            {
                effects.applyEffect(new FireEffect(burnDuration, burnDamagePerSecond));
                Debug.Log($"[Bullet] 화상 적용: {burnDamagePerSecond}/초, 지속 {burnDuration}초");
            }
            if (appliesFreeze)
            {
             
                effects.applyEffect(new FrozenEffect(freezeDuration, freezeSlowAmount));
                Debug.Log($"[Bullet] 빙결 적용: 속도 {freezeSlowAmount * 100f:F0}% 감소, 지속 {freezeDuration}초");
            }
            if (appliesShock)
            {
                effects.applyEffect(new ShockEffect(shockDuration));
                Debug.Log($"[Bullet] 감전 적용: 피해 50% 증가, 지속 {shockDuration}초");
            }
        }

        Destroy(gameObject);
    }
}