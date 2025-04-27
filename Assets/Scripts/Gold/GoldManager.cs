using UnityEngine;

public class GoldManager : MonoBehaviour
{
    public static GoldManager Instance; 
    public int CurrentGold = 100; 
    private void Awake()
    {
        // ½Ì±ÛÅæ ÃÊ±âÈ­
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);  // ÀÌ¹Ì Á¸ÀçÇÏ¸é »èÁ¦
        }
        else
        {
            Instance = this;  // ÇöÀç ÀÎ½ºÅÏ½º¸¦ ½Ì±ÛÅæÀ¸·Î ¼³Á¤
        }
    }
    
    public bool SpendGold(int amount)
    {
        if (CurrentGold >= amount)
        {
            CurrentGold -= amount;
            return true;
        }
        return false;
    }

    // °ñµå È¹µæ ¸Þ¼­µå
    public void AddGold(int amount)
    {
        CurrentGold += amount;
    }
}
