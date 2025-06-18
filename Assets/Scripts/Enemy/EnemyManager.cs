using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI.Table;

public class EnemyManager : MonoBehaviour
{
    [SerializeField]
    Vector3[] wayPoints;

    [SerializeField]
    private GameObject startPoint;

    void Update()
    {

    }

    public void testEnemySpawn()
    {
        
    }

    public Vector3 getWayPoints(int index)
    {
        if (index >= wayPoints.Length)
            return new Vector3Int(0, 0, 0);

        return wayPoints[index];
    }
}
