using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "TowerData.asset", menuName = "TowerDefense/Tower Configuration", order = 1)]
public class TowerLevelData : ScriptableObject {
    public string description;
    public string upgradeDescription;
    public int cost;
    public int sell;
    public int maxHealth;
    public int startingHealth;
    public Sprite icon;
}
