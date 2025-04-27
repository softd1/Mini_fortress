using UnityEngine;

public class TowerManager : MonoBehaviour
{
    public static TowerManager Instance { get; private set; } // 싱글턴

    public GameObject selectedTowerPrefab; // 현재 선택된 타워 프리팹
    public int selectedTowerCost;           // 현재 선택된 타워 가격

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
    // 타워 선택 (프리팹 + 가격)
    public void SetSelectedTower(GameObject prefab, int cost)
    {
        selectedTowerPrefab = prefab;
        selectedTowerCost = cost;
    }
}
