using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class SaveData
{
    public int HP;
    public int wave;
    public int gold;
    public int mapIndex;
    public List<TowerSaveData> towerData;
}