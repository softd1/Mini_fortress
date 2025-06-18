using UnityEngine;
using UnityEngine.EventSystems;

public class SimpleTowerPlacement : MonoBehaviour
{
    [Header("���� ��ġ�� ȭ�� Ÿ�� ������")]
    public GameObject fireTowerPrefab;
    [Header("���� ��ġ�� ���� Ÿ�� ������")]
    public GameObject iceTowerPrefab;

    [Header("Ÿ�� ���")]
    public int towerCost = 50;

    [Header("��ġ ���� ���̾�")]
    public LayerMask groundLayerMask;
    [Header("Raycast�� ī�޶�")]
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
            Debug.LogError("[Ÿ����ġ] GoldManager ����");
            return;
        }
        if (!GoldManager.Instance.SpendGold(towerCost))
        {
            Debug.Log("[Ÿ����ġ] ��尡 �����մϴ�.");
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