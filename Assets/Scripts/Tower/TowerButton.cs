using UnityEngine;

public class TowerButton : MonoBehaviour
{
    public GameObject towerPrefab;
    public int cost;

    private TowerManager towerManager;
    private TowerDragHandler towerDragHandler;

    private void Start()
    {
        towerDragHandler = FindObjectOfType<TowerDragHandler>();
    }

    public void OnClickSelectTower()
    {
        if (TowerManager.Instance != null)
        {
            TowerManager.Instance.SetSelectedTower(towerPrefab, cost);

            //타워 드래그 시작
            if (towerDragHandler != null)
            {
                towerDragHandler.StartDrag();
            }
            else
            {
                Debug.LogError("TowerDragHandler가 연결되지 않았습니다.");
            }
        }
        else
        {
            Debug.LogError("TowerManager가 연결되지 않았습니다.");
        }
    }
}
