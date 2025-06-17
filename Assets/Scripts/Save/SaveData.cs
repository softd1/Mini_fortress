using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class SaveData
{
    public SaveData()
    {
        HP = 100;
        wave = 1;
        gold = 600;
        mapIndex = 0;
        towerData = null;
    }

    public int HP;
    public int wave;
    public int gold;
    public int mapIndex;
    public List<TowerSaveData> towerData;
}