using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class Bullet : MonoBehaviour
{
    [Header("�⺻ �Ӽ�")]
    public float speed = 10f;
    public float lifeTime = 2f;
    public int damage = 10;

    [Header("ȭ�� (Burn)")]
    public bool appliesBurn = false;
    public float burnDuration = 3f;
    public int burnDamagePerSecond = 5;

    [Header("���� (Slow)")]
    public bool appliesFreeze = false;
    public float freezeDuration = 2f;
    public float freezeSlowAmount = 0.15f;  // �ӵ����� ���� (��: 0.15 �� 15%)

    [Header("���� (Shock)")]
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

        // ��� ������
        enemy.takeDamage(damage, fixedDamage: false);

        // �����̻� ����
        if (effects != null)
        {
            if (appliesBurn)
            {
                effects.applyEffect(new FireEffect(burnDuration, burnDamagePerSecond));
                Debug.Log($"[Bullet] ȭ�� ����: {burnDamagePerSecond}/��, ���� {burnDuration}��");
            }
            if (appliesFreeze)
            {
             
                effects.applyEffect(new FrozenEffect(freezeDuration, freezeSlowAmount));
                Debug.Log($"[Bullet] ���� ����: �ӵ� {freezeSlowAmount * 100f:F0}% ����, ���� {freezeDuration}��");
            }
            if (appliesShock)
            {
                effects.applyEffect(new ShockEffect(shockDuration));
                Debug.Log($"[Bullet] ���� ����: ���� 50% ����, ���� {shockDuration}��");
            }
        }

        Destroy(gameObject);
    }
}