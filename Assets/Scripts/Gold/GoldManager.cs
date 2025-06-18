using UnityEngine;
using System;

public class GoldManager : MonoBehaviour
{

    public static GoldManager Instance { get; private set; }

    public int CurrentGold { get; private set; }

    public event Action<int> OnGoldChanged;

    void Awake()
    {

        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        CurrentGold = 100;
        OnGoldChanged?.Invoke(CurrentGold);
    }

    public void AddGold(int amount)
    {
        CurrentGold += amount;
        OnGoldChanged?.Invoke(CurrentGold);
    }

    public bool SpendGold(int cost)
    {
        if (CurrentGold < cost) return false;
        CurrentGold -= cost;
        OnGoldChanged?.Invoke(CurrentGold);
        return true;
    }
}