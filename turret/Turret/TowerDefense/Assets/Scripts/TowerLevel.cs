using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerLevel : MonoBehaviour {
    public TowerGhost towerGhostPrefab;
    public GameObject buildEffectPrefab;
    public TowerLevelData levelData;
    public Vector2 dimensions;
    public int sell
    {
        get { return levelData.sell; }
    }

    /// <summary>
    /// Gets the max health
    /// </summary>
    public int maxHealth
    {
        get { return levelData.maxHealth; }
    }

    /// <summary>
    /// Gets the starting health
    /// </summary>
    public int startingHealth
    {
        get { return levelData.startingHealth; }
    }

    /// <summary>
    /// Gets the tower description
    /// </summary>
    public string description
    {
        get { return levelData.description; }
    }

    /// <summary>
    /// Gets the tower description
    /// </summary>
    public string upgradeDescription
    {
        get { return levelData.upgradeDescription; }
    }
    public virtual void Show()
    {
        if (!gameObject.activeSelf)
        {
            gameObject.SetActive(true);
        }
    }
    public virtual void Create(Vector3 pos, Quaternion rot)
    {
        transform.position = pos;
        transform.rotation = rot;
        GameObject.Instantiate(gameObject, pos, rot);
        GameObject effect = GameObject.Instantiate(buildEffectPrefab, transform);
        Destroy(effect, 5);
    }

    public virtual void Hide()
    {
        gameObject.SetActive(false);
    }
    // Update is called once per frame
    void Update () {
		
	}
}
