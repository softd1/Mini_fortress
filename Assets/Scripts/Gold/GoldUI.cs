using UnityEngine;
using TMPro;

public class GoldUI : MonoBehaviour
{
    public GoldManager goldManager;
    public TMP_Text goldText;

    void Start()
    {
       
        goldManager.OnGoldChanged += UpdateGold;

    
        UpdateGold(goldManager.CurrentGold);
    }

    void OnDestroy()
    {
        goldManager.OnGoldChanged -= UpdateGold;
    }

    private void UpdateGold(int amount)
    {
        goldText.text = $"Gold: {amount}";
    }
}