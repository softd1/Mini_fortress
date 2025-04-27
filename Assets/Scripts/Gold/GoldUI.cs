using TMPro;
using UnityEngine;
public class GoldUI : MonoBehaviour
{
    public TextMeshProUGUI goldText;

    private void Update()
    {
        if (GoldManager.Instance != null)
        {
            goldText.text = "Gold: " + GoldManager.Instance.CurrentGold.ToString();
        }
    }
}
