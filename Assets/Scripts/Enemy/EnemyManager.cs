using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] Vector3[] wayPoints;
    [SerializeField] WaveManager waveManager;

    List<SpawnData> currentWave;
    List<SpawnData> nextWave;

    int waveIndex;
    int listIndex;
    float elapsed;

    readonly List<Enemy> active = new();
    readonly Dictionary<string, GameObject> cache = new();

    void Awake()
    {
        if (waveManager == null) waveManager = FindObjectOfType<WaveManager>();
        waveIndex = 1;
        currentWave = waveManager.getWave(waveIndex);
        nextWave = waveManager.getWave(waveIndex + 1);
    }

    void Update()
    {
        if (currentWave == null) return;

        elapsed += Time.deltaTime;

        while (listIndex < currentWave.Count && elapsed >= currentWave[listIndex].time)
        {
            StartCoroutine(SpawnBatch(currentWave[listIndex]));
            listIndex++;
        }

        if (listIndex == currentWave.Count && active.Count == 0)
            MoveNextWave();
    }

    IEnumerator SpawnBatch(SpawnData d)
    {
        for (int i = 0; i < d.count; i++)
        {
            Spawn(d.prefabName);
            if (i < d.count - 1 && d.space > 0f)
                yield return new WaitForSeconds(d.space * 0.5f);
        }
    }

    void Spawn(string prefabName)
    {
        if (!cache.TryGetValue(prefabName, out var pf))
        {
            pf = Resources.Load<GameObject>($"Prefab/{prefabName.Trim()}");
            if (pf == null) return;
            cache[prefabName] = pf;
        }

        var obj = Instantiate(pf, waveManager.spawnPosition, Quaternion.identity);
        var e = obj.GetComponent<Enemy>();
        if (e) e.setManager(this);
        active.Add(e);
    }

    public void NotifyDeath(Enemy e) => active.Remove(e);

    void MoveNextWave()
    {
        waveIndex++;
        currentWave = nextWave;
        nextWave = waveManager.getWave(waveIndex + 1);
        listIndex = 0;
        elapsed = 0f;
    }

    public Vector3 getWayPoints(int i) =>
        i < wayPoints.Length ? wayPoints[i] : Vector3.zero;
}
