using UnityEngine;

public class GameManager : MonoBehaviour
{
    private SaveManager saveManager;

    void Awake()
    {
        saveManager = GetComponent<SaveManager>();
        DontDestroyOnLoad(gameObject);
    }
}
