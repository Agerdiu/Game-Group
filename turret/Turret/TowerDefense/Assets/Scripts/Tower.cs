using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour {
    public TowerLevel[] levels;
    public Vector3 pos;
    public string towerName;
    public Vector2 dimensions;
    public TowerLevelData levelData;
    public int currentLevel;
    public Vector2 gridPosition;
    public TowerPlacementGrid grid;
    // Use this for initialization
    

    public bool isAtMaxLevel
    {
        get { return currentLevel == levels.Length - 1; }
    }

    public TowerGhost towerGhostPrefab
    {
        get { return levels[currentLevel].towerGhostPrefab; }
    }
    /*
    public virtual bool UpgradeTower()
    {
        if (isAtMaxLevel)
        {
            return false;
        }
        SetLevel(currentLevel + 1);
        return true;
    }

    public virtual bool DowngradeTower()
    {
        if (currentLevel == 0)
        {
            return false;
        }
        SetLevel(currentLevel - 1);
        return true;
    }*/
    public virtual void Initialize(TowerPlacementGrid targetArea, Vector2 destination)
    {
        grid = targetArea;
        gridPosition = destination;

        if (targetArea != null)
        {
            transform.position = grid.GridToWorld(destination, dimensions);
            transform.rotation = grid.transform.rotation;
            targetArea.SetOccupy(destination, dimensions);
        }
    }
  }
