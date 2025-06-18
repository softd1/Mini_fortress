using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class RangeVisualizer : MonoBehaviour
{
    [Header("�����Ÿ� (Inspector ���� ���� ����)")]
    public float range = 5f;

    [Header("�� ���� ���� (�������� �Ų�����)")]
    public int segments = 60;

    [Header("���� �β�")]
    public float lineWidth = 0.1f;

    private LineRenderer lr;

    void Awake()
    {
        lr = GetComponent<LineRenderer>();
        lr.loop = true;
        lr.useWorldSpace = false; // ������ǥ(Ÿ�� �߽�)
        lr.widthMultiplier = lineWidth;
        lr.positionCount = segments + 1;
        UpdateCircle();
    }

#if UNITY_EDITOR
    // Inspector ���� �� �ٲ� ������ �ڵ� ����
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