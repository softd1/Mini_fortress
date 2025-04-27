using UnityEngine;

public class TowerManager : MonoBehaviour
{
    public static TowerManager Instance { get; private set; } // �̱���

    public GameObject selectedTowerPrefab; // ���� ���õ� Ÿ�� ������
    public int selectedTowerCost;           // ���� ���õ� Ÿ�� ����

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    // Ÿ�� ���� (������ + ����)
    public void SetSelectedTower(GameObject prefab, int cost)
    {
        selectedTowerPrefab = prefab;
        selectedTowerCost = cost;
    }
}
