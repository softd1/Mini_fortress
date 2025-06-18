using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor.SearchService;

public class MainMenuManager : MonoBehaviour
{
    private GameManager gameManager;
    private SaveManager save;

    [SerializeField]
    private Button continueButton;

    void Start()
    {
        save = GameObject.Find("GameManager").GetComponent<SaveManager>();

        checkSaveExist();
    }
    public void checkSaveExist()
    {
        continueButton.interactable = save.isExist();
    }

    public void startNewGame()
    {
        SceneManager.LoadScene("Map1");
    }

    public void resumeGame()
    {

    }
    public void exitGame()
    {
        Application.Quit();
        Debug.Log("Game Closed");
    }

    void Update()
    {
        
    }
}
