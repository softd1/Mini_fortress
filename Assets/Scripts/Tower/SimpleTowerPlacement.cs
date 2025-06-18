using UnityEngine;
using UnityEngine.EventSystems;

public class SimpleTowerPlacement : MonoBehaviour
{
    [Header("실제 설치될 화염 타워 프리팹")]
    public GameObject fireTowerPrefab;
    [Header("실제 설치될 빙결 타워 프리팹")]
    public GameObject iceTowerPrefab;

    [Header("타워 비용")]
    public int towerCost = 50;

    [Header("설치 가능 레이어")]
    public LayerMask groundLayerMask;
    [Header("Raycast용 카메라")]
    public Camera cam;

    private GameObject selectedPrefab;
    private GameObject draggingTower;
    private TowerAttack towerAttackComp;
    private bool isDragging = false;
    private int pendingRefund;

    public void OnFireTowerButton() => BeginPlacement(fireTowerPrefab);
    public void OnIceTowerButton() => BeginPlacement(iceTowerPrefab);

    private void BeginPlacement(GameObject prefab)
    {
        if (isDragging) return;
        if (GoldManager.Instance == null)
        {
            Debug.LogError("[타워설치] GoldManager 없음");
            return;
        }
        if (!GoldManager.Instance.SpendGold(towerCost))
        {
            Debug.Log("[타워설치] 골드가 부족합니다.");
            return;
        }

        pendingRefund = towerCost;
        selectedPrefab = prefab;
        draggingTower = Instantiate(selectedPrefab);
        towerAttackComp = draggingTower.GetComponent<TowerAttack>();
        if (towerAttackComp != null) towerAttackComp.enabled = false;
        isDragging = true;
    }

    void Update()
    {
        if (!isDragging || draggingTower == null) return;

        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out var hit, Mathf.Infinity, groundLayerMask))
        {
            draggingTower.transform.position = hit.point;

            if (Input.GetMouseButtonDown(0) &&
                !EventSystem.current.IsPointerOverGameObject())
            {
                if (towerAttackComp != null) towerAttackComp.enabled = true;
                ResetDragState();
            }
   
            else if (Input.GetMouseButtonDown(1))
            {
                GoldManager.Instance.AddGold(pendingRefund);
                Destroy(draggingTower);
                ResetDragState();
            }
        }
    }

    private void ResetDragState()
    {
        draggingTower = null;
        towerAttackComp = null;
        isDragging = false;
        selectedPrefab = null;
    }
}