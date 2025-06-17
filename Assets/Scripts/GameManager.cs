using UnityEngine;
using UnityEngine.SceneManagement;

interface inGameManager
{
    public void initGame();
    public void savedGame(SaveData save);
}

public class GameManager : MonoBehaviour
{
    private SaveManager saveManager;
    private SaveData saveData;

    void Awake()
    {
        saveManager = GetComponent<SaveManager>();
        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += onSceneLoaded;
    }
    public void loadSavedGame()
    {
        saveData = saveManager.load();
    }

    public void onSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (!scene.name.StartsWith("Map")) return;

        GameObject inGameManager = GameObject.Find("InGameManager");

        WaveManager waveManager = inGameManager.GetComponent<WaveManager>();
        TowerManager towerManager = inGameManager.GetComponent<TowerManager>();

        loadSavedGame();

        waveManager.savedGame(saveData);
        // towerManager.savedGame(saveData);
    }
}
