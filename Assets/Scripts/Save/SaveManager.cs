using UnityEngine;
using System.IO;
public class SaveManager : MonoBehaviour
{
    private const string FILE = "save.json";
    public void save(SaveData data)
    {
        string json = JsonUtility.ToJson(data, prettyPrint: false);
        // 확인할거면 prettyPrint true로 수정
        File.WriteAllText(Path.Combine(Application.persistentDataPath, FILE), json);
    }

    public bool tryLoad(out SaveData data)
    {
        string path = Path.Combine(Application.persistentDataPath, FILE);

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            data = JsonUtility.FromJson<SaveData>(json);
            return true;
        }
        data = null;
        return false;
    }

    public bool isExist()
    {
        string path = Path.Combine(Application.persistentDataPath, FILE);

        return File.Exists(path);
    }

    public void Delete()
    {
        string path = Path.Combine(Application.persistentDataPath, FILE);
        if (File.Exists(path)) File.Delete(path);
    }
}
