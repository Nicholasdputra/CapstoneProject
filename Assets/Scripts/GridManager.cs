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
        Vector3 floorCenter = floorPlatform.transform.position;
            int x = Mathf.RoundToInt((worldPos.x - floorCenter.x) / gridCellSize);
            int z = Mathf.RoundToInt((worldPos.z - floorCenter.z) / gridCellSize);
            return new Vector2Int(x, z);
    }

    //  Converts grid coordinates to world position
    public Vector3 GridToWorld(Vector2Int gridPos)
    {
        Vector3 floorCenter = floorPlatform.transform.position;
        return new Vector3(
            floorCenter.x + (gridPos.x * gridCellSize), 
            floorCenter.y, 
            floorCenter.z + (gridPos.y * gridCellSize)
        );
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

    public Vector3 GetRandomFreeTilePosition()
    {
        // Update unoccupied cells by finding the difference
        UpdateUnoccupiedCells();

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

    public Vector3 GetGroundY(Vector3 basePos, float raycastHeight, float offset)
    {
        // Spawn above the ground and raycast down
        Ray ray = new Ray(basePos + Vector3.up * raycastHeight, Vector3.down);

        // If it hits the ground, return the hit point
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, groundLayer))
        {
            return hit.point + new Vector3(0,offset,0);
        }

        //If there's no ground, return the original position
        return basePos  + new Vector3(0,offset,0);;
    }

    private void UpdateUnoccupiedCells()
    {
        CalculateFloorSize();

        // Clear and repopulate allCells for the given radius
        allCells.Clear();
        int halfX = (floorXSize-1) / 2;
        int halfZ = (floorZSize-1) / 2;

        for (int x = -halfX; x < halfX; x++)
        {
            for (int z = -halfZ; z < halfZ; z++)
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
    
    #if UNITY_EDITOR
    void OnDrawGizmos()
    {
        if (floorPlatform == null) return;

        // Calculate floor bounds
        Vector3 floorCenter = floorPlatform.transform.position;
        Vector3 floorScale = floorPlatform.transform.localScale;
        
        float halfWidth = floorScale.x * 0.5f;
        float halfHeight = floorScale.z * 0.5f;
        
        // Draw grid lines
        Gizmos.color = Color.red;
        
        // Vertical lines (along Z-axis)
        for (float x = -halfWidth; x <= halfWidth; x += gridCellSize)
        {
            Vector3 start = new Vector3(floorCenter.x + x, floorCenter.y + 0.49f, floorCenter.z - halfHeight);
            Vector3 end = new Vector3(floorCenter.x + x, floorCenter.y + 0.49f, floorCenter.z + halfHeight);
            Gizmos.DrawLine(start, end);
        }
        
        // Horizontal lines (along X-axis)
        for (float z = -halfHeight; z <= halfHeight; z += gridCellSize)
        {
            Vector3 start = new Vector3(floorCenter.x - halfWidth, floorCenter.y + 0.49f, floorCenter.z + z);
            Vector3 end = new Vector3(floorCenter.x + halfWidth, floorCenter.y + 0.49f, floorCenter.z + z);
            Gizmos.DrawLine(start, end);
        }
        
        // Draw occupied cells
        Gizmos.color = Color.blue;
        foreach (Vector2Int occupiedCell in occupiedCells)
        {
            Vector3 cellCenter = GridToWorld(occupiedCell);
            cellCenter.y = floorCenter.y + 0.5f; // Slightly above the floor
            Gizmos.DrawCube(cellCenter, new Vector3(gridCellSize, 0.1f, gridCellSize));
        }
    }
    #endif
}