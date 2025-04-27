using UnityEngine;

public class TowerDragHandler : MonoBehaviour
{
    private GameObject previewTower; // 마우스를 따라다니는 가짜 타워
    private bool isDragging = false; // 드래그 중 여부
    private bool isOnWall = false;   // 벽 위에 있는지 여부
    private GameObject targetWall;
    void Update()
    {
        if (isDragging)
        {
            FollowMouse();

            if (Input.GetMouseButtonDown(0)) //설치
            {
                PlaceTower();
            }
            else if (Input.GetMouseButtonDown(1)) //취소
            {
                CancelPlacement();
            }
        }
    }
    public void StartDrag()
    {
        if (TowerManager.Instance.selectedTowerPrefab == null)
        {
            Debug.Log("설치할 타워를 선택하세요!");
            return;
        }

        if (GoldManager.Instance == null) return;

        if (GoldManager.Instance.CurrentGold < TowerManager.Instance.selectedTowerCost)
        {
            Debug.Log("골드가 부족합니다!");
            return;
        }

        // 프리뷰 생성
        previewTower = Instantiate(TowerManager.Instance.selectedTowerPrefab);
        previewTower.transform.position = new Vector3(0, 5, 0);
        isDragging = true;

        // 설치 전까지 공격 못하게
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

        int layerMask = ~(1 << LayerMask.NameToLayer("Preview")); // Preview 레이어 무시

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
            Debug.Log("벽 위에서만 설치할 수 있습니다!");
            return;
        }

        if (GoldManager.Instance.SpendGold(TowerManager.Instance.selectedTowerCost))
        {
            // 벽 중앙에 타워 생성
            Instantiate(TowerManager.Instance.selectedTowerPrefab, targetWall.transform.position, Quaternion.identity);

            // 프리뷰 삭제
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

