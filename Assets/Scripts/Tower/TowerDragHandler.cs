using UnityEngine;

public class TowerDragHandler : MonoBehaviour
{
    private GameObject previewTower; // ���콺�� ����ٴϴ� ��¥ Ÿ��
    private bool isDragging = false; // �巡�� �� ����
    private bool isOnWall = false;   // �� ���� �ִ��� ����
    private GameObject targetWall;
    void Update()
    {
        if (isDragging)
        {
            FollowMouse();

            if (Input.GetMouseButtonDown(0)) //��ġ
            {
                PlaceTower();
            }
            else if (Input.GetMouseButtonDown(1)) //���
            {
                CancelPlacement();
            }
        }
    }
    public void StartDrag()
    {
        if (TowerManager.Instance.selectedTowerPrefab == null)
        {
            Debug.Log("��ġ�� Ÿ���� �����ϼ���!");
            return;
        }

        if (GoldManager.Instance == null) return;

        if (GoldManager.Instance.CurrentGold < TowerManager.Instance.selectedTowerCost)
        {
            Debug.Log("��尡 �����մϴ�!");
            return;
        }

        // ������ ����
        previewTower = Instantiate(TowerManager.Instance.selectedTowerPrefab);
        previewTower.transform.position = new Vector3(0, 5, 0);
        isDragging = true;

        // ��ġ ������ ���� ���ϰ�
        TowerAttack attackScript = previewTower.GetComponent<TowerAttack>();
        if (attackScript != null)
        {
            attackScript.enabled = false;
        }
    }

    private void FollowMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        int layerMask = ~(1 << LayerMask.NameToLayer("Preview")); // Preview ���̾� ����

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
        {
            previewTower.transform.position = hit.point;

            if (hit.collider.CompareTag("Wall"))
            {
                isOnWall = true;
                targetWall = hit.collider.gameObject;
            }
            else
            {
                isOnWall = false;
                targetWall = null; 
            }
        }
    }
    private void PlaceTower()
    {
        if (!isOnWall || targetWall == null)
        {
            Debug.Log("�� �������� ��ġ�� �� �ֽ��ϴ�!");
            return;
        }

        if (GoldManager.Instance.SpendGold(TowerManager.Instance.selectedTowerCost))
        {
            // �� �߾ӿ� Ÿ�� ����
            Instantiate(TowerManager.Instance.selectedTowerPrefab, targetWall.transform.position, Quaternion.identity);

            // ������ ����
            Destroy(previewTower);
            previewTower = null;
            isDragging = false;
            targetWall = null;
        }
    }
    private void CancelPlacement()
    {
        if (previewTower != null)
        {
            Destroy(previewTower);
            previewTower = null;
            isDragging = false;
        }
    }
}

