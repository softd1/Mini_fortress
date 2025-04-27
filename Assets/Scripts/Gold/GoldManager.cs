using UnityEngine;

public class GoldManager : MonoBehaviour
{
    public static GoldManager Instance; 
    public int CurrentGold = 100; 
    private void Awake()
    {
        // �̱��� �ʱ�ȭ
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);  // �̹� �����ϸ� ����
        }
        else
        {
            Instance = this;  // ���� �ν��Ͻ��� �̱������� ����
        }
    }
    
    public bool SpendGold(int amount)
    {
        if (CurrentGold >= amount)
        {
            CurrentGold -= amount;
            return true;
        }
        return false;
    }

    // ��� ȹ�� �޼���
    public void AddGold(int amount)
    {
        CurrentGold += amount;
    }
}
