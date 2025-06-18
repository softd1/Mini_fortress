using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class RangeVisualizer : MonoBehaviour
{
    [Header("사정거리 (Inspector 에서 직접 설정)")]
    public float range = 5f;

    [Header("원 분할 개수 (높을수록 매끄럽게)")]
    public int segments = 60;

    [Header("라인 두께")]
    public float lineWidth = 0.1f;

    private LineRenderer lr;

    void Awake()
    {
        lr = GetComponent<LineRenderer>();
        lr.loop = true;
        lr.useWorldSpace = false; // 로컬좌표(타워 중심)
        lr.widthMultiplier = lineWidth;
        lr.positionCount = segments + 1;
        UpdateCircle();
    }

#if UNITY_EDITOR
    // Inspector 에서 값 바꿀 때마다 자동 갱신
    void OnValidate()
    {
        if (lr == null) lr = GetComponent<LineRenderer>();
        if (lr != null)
        {
            lr.widthMultiplier = lineWidth;
            lr.positionCount = segments + 1;
            UpdateCircle();
        }
    }
#endif

    void UpdateCircle()
    {
        float deltaTheta = 2f * Mathf.PI / segments;
        float theta = 0f;

        for (int i = 0; i <= segments; i++)
        {
            float x = Mathf.Cos(theta) * range;
            float z = Mathf.Sin(theta) * range;
            lr.SetPosition(i, new Vector3(x, 0f, z));
            theta += deltaTheta;
        }
    }
}