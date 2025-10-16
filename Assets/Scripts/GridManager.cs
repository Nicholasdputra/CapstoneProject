using UnityEngine;
using System.Collections.Generic;

public class GridManager : MonoBehaviour
{
    public static GridManager instance;

    public float gridCellSize = 1f;
    public LayerMask groundLayer;
    private HashSet<Vector2Int> occupiedCells = new HashSet<Vector2Int>();
    private HashSet<Vector2Int> allCells = new HashSet<Vector2Int>();
    private HashSet<Vector2Int> unoccupiedCells = new HashSet<Vector2Int>();

    public GameObject floorPlatform;
    public int floorXSize;
    public int floorZSize;

    // get y scale, divide by two, spawn at the x,y + yscale/2 + spawnableheight/2 ,z
    
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    # region Helper Methods
    // Converts world position to grid coordinates
    public Vector2Int WorldToGrid(Vector3 worldPos)
    {
        int x = Mathf.RoundToInt(worldPos.x / gridCellSize);
        int z = Mathf.RoundToInt(worldPos.z / gridCellSize);
        return new Vector2Int(x, z);
    }

    //  Converts grid coordinates to world position
    public Vector3 GridToWorld(Vector2Int gridPos)
    {
        return new Vector3(gridPos.x * gridCellSize, 0, gridPos.y * gridCellSize);
    }

    // Checks if a grid cell is occupied
    public bool IsCellOccupied(Vector2Int gridPos)
    {
        return occupiedCells.Contains(gridPos);
    }

    // Marks a grid cell as occupied or free
    public void SetCellOccupied(Vector2Int gridPos, bool occupied)
    {
        if (occupied)
        {
            occupiedCells.Add(gridPos);
        }
        else
        {
            occupiedCells.Remove(gridPos);
        }
    }

    // Calculate floor size in grid cells
    public void CalculateFloorSize()
    {
        floorXSize = Mathf.RoundToInt(floorPlatform.transform.localScale.x / gridCellSize);
        floorZSize = Mathf.RoundToInt(floorPlatform.transform.localScale.z / gridCellSize);
    }

    # endregion

    public Vector3 GetRandomFreeTilePosition(int xsize, int zsize)
    {
        // Update unoccupied cells by finding the difference
        UpdateUnoccupiedCells(xsize, zsize);

        if (unoccupiedCells.Count == 0)
        {
            return Vector3.zero; // No free cells available
        }

        // Convert HashSet to array for random indexing
        Vector2Int[] freeCells = new Vector2Int[unoccupiedCells.Count];
        unoccupiedCells.CopyTo(freeCells);

        // Pick a random free cell
        int randomIndex = Random.Range(0, freeCells.Length);
        return GridToWorld(freeCells[randomIndex]);
    }

    public Vector3 GetGroundY(Vector3 basePos, float raycastHeight)
    {
        // Spawn above the ground and raycast down
        Ray ray = new Ray(basePos + Vector3.up * raycastHeight, Vector3.down);

        // If it hits the ground, return the hit point
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, groundLayer))
        {
            return hit.point;
        }

        //If there's no ground, return the original position
        return basePos;
    }

    private void UpdateUnoccupiedCells(int xsize, int zsize)
    {
        // Clear and repopulate allCells for the given radius
        allCells.Clear();
        for (int x = -xsize; x < xsize; x++)
        {
            for (int z = -zsize; z < zsize; z++)
            {
                allCells.Add(new Vector2Int(x, z));
            }
        }
        
        // Calculate unoccupied cells as the difference
        unoccupiedCells.Clear();
        foreach (Vector2Int cell in allCells)
        {
            if (!occupiedCells.Contains(cell))
            {
                unoccupiedCells.Add(cell);
            }
        }
    }
}