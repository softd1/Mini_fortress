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
        Debug.Log("적이 죽었다!");
    }
    //도트뎀
    public void ApplyBurn(float burnDuration, int burnDamagePerSecond)
    {
        StartCoroutine(BurnCoroutine(burnDuration, burnDamagePerSecond));
    }

    private IEnumerator BurnCoroutine(float duration, int dps)
    {
        float elapsed = 0f;
        //이펙트 생성
        if (burnEffectPrefab != null && activeBurnEffect == null)
        {
            activeBurnEffect = Instantiate(burnEffectPrefab, transform.position, Quaternion.identity, transform);
        }


        while (elapsed < duration)
        {
            TakeDamage(dps); // 1초마다 dps 만큼 데미지
            Debug.Log($"불 도트 피해 입음! {dps} 데미지");
            elapsed += 1f;
            yield return new WaitForSeconds(1f);
        }
        //이펙트 제거
        if (activeBurnEffect != null)
        {
            Destroy(activeBurnEffect);
            activeBurnEffect = null;
        }
    }
}
