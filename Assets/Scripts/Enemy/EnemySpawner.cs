using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public Vector3 spawnPosition = Vector3.zero;
    public string csvRoot = "Waves/Wave";
    public int waveCount = 1;

    private const float SPACE_UNIT_SEC = 0.5f;

    class SpawnData
    {
        public float time;
        public string prefabName;
        public int count;
        public float space;
    }

    void Start()
    {
        string filePath = $"{csvRoot}{waveCount:D2}";
        TextAsset csvFile = Resources.Load<TextAsset>(filePath);

        if (csvFile == null)
        {
            Debug.Log("no csv");
        }

        List<SpawnData> list = Parse(csvFile.text);
        StartCoroutine(RunSchedule(list));
    }

    List<SpawnData> Parse(string csv)
    {
        List<SpawnData> list = new();
        string[] lines = csv.Split('\n');

        for (int i = 1; i < lines.Length; i++)
        {
            if (string.IsNullOrWhiteSpace(lines[i])) continue;

            string[] cols = lines[i].Split(',');
            if (cols.Length < 4) continue;

            float t = float.Parse(cols[0]);
            string name = cols[1].Trim();
            int cnt = int.Parse(cols[2]);
            float space = float.Parse(cols[3]);

            list.Add(new SpawnData { time = t, prefabName = name, count = cnt, space = space });
        }

        list.Sort((a, b) => a.time.CompareTo(b.time));
        return list;
    }

    IEnumerator RunSchedule(List<SpawnData> data)
    {
        float timeline = 0f;

        foreach (var d in data)
        {
            float wait = d.time - timeline;
            if (wait > 0f) yield return new WaitForSeconds(wait);

            for (int i = 0; i < d.count; i++)
            {
                Spawn(d.prefabName);
                if (i < d.count - 1)
                    yield return new WaitForSeconds(d.space * SPACE_UNIT_SEC);
            }

            timeline = d.time + (d.count - 1) * d.space * SPACE_UNIT_SEC;
        }
    }

    void Spawn(string prefabName)
    {

        GameObject prefab = Resources.Load<GameObject>(("Prefab/" + prefabName.Trim()));
        if (prefab == null) return;

            GameObject obj = Instantiate(prefab, spawnPosition, Quaternion.identity);

        Enemy enemy = obj.GetComponent<Enemy>();
        if (enemy != null)
            enemy.setManager(GetComponent<EnemyManager>());
    }

}
