using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum TowerFitStatus
{
    Fits,
    Overlaps,
    OutOfBounds
}
public class TowerPlacementGrid : MonoBehaviour {
    public Tile placementTilePrefab;
    public Vector2 dimensions;
    public float gridSize = 1;
    bool[,] AvailableCells;
    Tile[,] Tiles;
    public TowerFitStatus Fits(Vector2 pos,Vector2 size)
    {
        /*out of bound*/
        if (size.x > dimensions.x || size.y > dimensions.y)
            return TowerFitStatus.OutOfBounds;
        Vector2 pos2 = pos + size;
        if (pos.x < 0 || pos.y < 0 ||(pos2.x>dimensions.x)|| (pos2.y > dimensions.y))
            return TowerFitStatus.OutOfBounds;
        for(int i=(int)pos.x;i<pos2.x;i++)
            for(int j = (int)pos.y; j < pos2.y; j++)
            {
                if (!AvailableCells[i, j])
                    return TowerFitStatus.Overlaps;
            }
        return TowerFitStatus.Fits;
    }
    public void SetOccupy(Vector2 pos, Vector2 size)
    {
        if (size.x > dimensions.x || size.y > dimensions.y)
            return ;
        Vector2 pos2 = pos + size;
        if (pos.x < 0 || pos.y < 0 || (pos2.x > dimensions.x) || (pos2.y > dimensions.y))
            return ;
        for (int i =(int)pos.x; i < (int)pos2.x; i++)
            for (int j = (int)pos.y; j < (int)pos2.y; j++)
            {
                AvailableCells[i, j] = false;

                if (Tiles != null && Tiles[i, j] != null)
                    Tiles[i, j].SetState(PlacementTileState.Filled);
            }
    }
    public void Clear(Vector2 pos, Vector2 size)
    {
        if (size.x > dimensions.x || size.y > dimensions.y)
            return;
        Vector2 pos2 = pos + size;
        if (pos.x < 0 || pos.y < 0 || (pos2.x > dimensions.x) || (pos2.y > dimensions.y))
            return;
        for (int i = 0; i < size.x; i++)
            for (int j = 0; j < size.y; j++)
            {
                AvailableCells[i, j] = true;
                if (Tiles != null && Tiles[i, j] != null)
                    Tiles[i, j].SetState(PlacementTileState.Empty);
            }
    }
    public Vector2 WorldToGrid(Vector3 worldLocation, Vector2 sizeOffset)
    {
        Vector3 localLocation = transform.InverseTransformPoint(worldLocation);

        // Scale by inverse grid size
        localLocation *= 1/gridSize;

        // Offset by half size
        var offset = new Vector3(sizeOffset.x * 0.5f, 0.0f, sizeOffset.y * 0.5f);
        localLocation -= offset;

        int xPos = Mathf.RoundToInt(localLocation.x);
        int yPos = Mathf.RoundToInt(localLocation.z);

        return new Vector2(xPos, yPos);
    }
    public Vector3 GridToWorld(Vector2 gridPosition, Vector2 sizeOffset)
    {
        // Calculate scaled local position
        Vector3 localPos = new Vector3(gridPosition.x + (sizeOffset.x * 0.5f), 0, gridPosition.y + (sizeOffset.y * 0.5f)) *
                           gridSize;

        return transform.TransformPoint(localPos);
    }
    protected void SetUpGrid()
    {
        Tile tileToUse = placementTilePrefab;
        if (tileToUse != null)
        {
            Tiles = new Tile[(int)dimensions.x, (int)dimensions.y];
            var tilesParent = new GameObject("Container");
            tilesParent.transform.parent = transform;
            tilesParent.transform.localPosition = Vector3.zero;
            tilesParent.transform.localRotation = Quaternion.identity;
            for (int y = 0; y < dimensions.y; y++)
            {
                for (int x = 0; x < dimensions.x; x++)
                {
                    Vector3 targetPos = GridToWorld(new Vector2(x, y), new Vector2(1, 1));
                    targetPos.y += 0.01f;
                    Tile newTile = Instantiate(tileToUse);
                    newTile.transform.parent = tilesParent.transform;
                    newTile.transform.position = targetPos;
                    newTile.transform.localRotation = Quaternion.identity;
                    newTile.transform.localScale =new Vector3(gridSize,1.0f,gridSize);
                    newTile.gameObject.layer = 9;//Grid
                    Tiles[x, y] = newTile;
                    newTile.SetState(PlacementTileState.Empty);
                    AvailableCells[x, y] = true;
                }
            }
        }
    }
    void Awake()
    {
        AvailableCells = new bool[(int)dimensions.x, (int)dimensions.y];
        this.gameObject.layer = 9;
        SetUpGrid();
    }
}
