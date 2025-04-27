using UnityEngine;
using System.Collections;
public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 50;
    private int currentHealth;

    public GameObject burnEffectPrefab;
    private GameObject activeBurnEffect;
    void Start()
    {
        currentHealth = maxHealth;
    }
    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            Die();
        }
    }
    void Die()
    {
        GoldManager.Instance.AddGold(50);
        Destroy(gameObject);
        Debug.Log("���� �׾���!");
    }
    //��Ʈ��
    public void ApplyBurn(float burnDuration, int burnDamagePerSecond)
    {
        StartCoroutine(BurnCoroutine(burnDuration, burnDamagePerSecond));
    }

    private IEnumerator BurnCoroutine(float duration, int dps)
    {
        float elapsed = 0f;
        //����Ʈ ����
        if (burnEffectPrefab != null && activeBurnEffect == null)
        {
            activeBurnEffect = Instantiate(burnEffectPrefab, transform.position, Quaternion.identity, transform);
        }


        while (elapsed < duration)
        {
            TakeDamage(dps); // 1�ʸ��� dps ��ŭ ������
            Debug.Log($"�� ��Ʈ ���� ����! {dps} ������");
            elapsed += 1f;
            yield return new WaitForSeconds(1f);
        }
        //����Ʈ ����
        if (activeBurnEffect != null)
        {
            Destroy(activeBurnEffect);
            activeBurnEffect = null;
        }
    }
}
