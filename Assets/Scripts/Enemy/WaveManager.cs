using System.Collections.Generic;
using UnityEngine;

public class SpawnData
{
    public float time;
    public string prefabName;
    public int count;
    public float space;
}

public class WaveManager : MonoBehaviour, inGameManager
{
    public Vector3 spawnPosition = Vector3.zero;
    private const string csvRoot = "Waves/Wave";
    private int waveCount = 1;

    private const float SPACE_UNIT_SEC = 0.5f;

    public List<SpawnData> getWave(int index)
    {
        string filePath = $"{csvRoot}{index:D2}";
        TextAsset csvFile = Resources.Load<TextAsset>(filePath);

        if (csvFile == null)
        {
            Debug.Log("no csv");
        }

       return Parse(csvFile.text);
    }

    private List<SpawnData> Parse(string csv)
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
    public void initGame()
    {
        waveCount = 1;
    }

    public void savedGame(SaveData save)
    {
        waveCount = save.wave;
    }
}
