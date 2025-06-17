using System.Collections;
using System.Collections.Generic;
using NUnit.Framework.Internal.Commands;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI.Table;

public class EnemyManager : MonoBehaviour
{
    [SerializeField]
    Vector3[] wayPoints;

    private WaveManager waveManager;

    private List<SpawnData> currentWave;
    private List<SpawnData> nextWave;

    private int waveIndex = 0;

    private void moveWave()
    {
        currentWave = nextWave;

        nextWave = waveManager.getWave(waveIndex + 1);
        waveIndex++;
    }

    private void Start()
    {

    }

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
