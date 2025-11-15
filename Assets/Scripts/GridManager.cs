using UnityEngine;
using System.Collections.Generic;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance;

    public float gridCellSize = 1f;
    public int floorXSize;
    public int floorZSize;
    public LayerMask excludedLayers;
    public GameObject floorPlatform;
    public Vector3 floorCenter;
    public Vector3 floorOrigin;

    private HashSet<Vector2Int> allGridPositions = new HashSet<Vector2Int>();
    private HashSet<Vector2Int> occupiedGridPositions = new HashSet<Vector2Int>();
    private HashSet<Vector2Int> unoccupiedGridPositions = new HashSet<Vector2Int>();

    public VoidEventChannel OnGridInitialized;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        CalculateFloorSize();
        InitializeGridPositions();
    }

    private void CalculateFloorSize()
    {
        floorPlatform = GameObject.FindWithTag("FloorPlatform");
        if (floorPlatform == null)
        {
            Debug.LogError("Floor Platform not found!");
            return;
        }
        var renderer = floorPlatform.GetComponent<Renderer>();
        Bounds b = renderer.bounds;
        floorCenter = b.center;
        floorOrigin = b.min; // bottom-left corner of the floor in world space

        floorXSize = Mathf.RoundToInt(b.size.x / gridCellSize);
        floorZSize = Mathf.RoundToInt(b.size.z / gridCellSize);
    }

    private void InitializeGridPositions()
    {
        allGridPositions.Clear();
        for (int x = 0; x < floorXSize; x++)
        {
            for (int z = 0; z < floorZSize; z++)
            {
                allGridPositions.Add(new Vector2Int(x, z));
            }
        }
        unoccupiedGridPositions = new HashSet<Vector2Int>(allGridPositions);

        OnGridInitialized.RaiseEvent();
    }

    public Vector2Int ConvertPosFromWorldToGrid(Vector3 worldPos)
    {
        // Convert a world position to a grid cell index (0..floorXSize-1, 0..floorZSize-1)
        Vector3 local = worldPos - floorOrigin; // position relative to bottom-left corner
        int gx = Mathf.FloorToInt(local.x / gridCellSize);
        int gz = Mathf.FloorToInt(local.z / gridCellSize);

        // optional: clamp to valid grid range
        if (gx < 0) gx = 0;
        if (gz < 0) gz = 0;
        if (gx >= floorXSize) gx = floorXSize - 1;
        if (gz >= floorZSize) gz = floorZSize - 1;

        return new Vector2Int(gx, gz);
    }

    public Vector3 ConvertPosFromGridToWorld(Vector2Int gridPos)
    {
        float x = floorOrigin.x + (gridPos.x * gridCellSize) + (gridCellSize * 0.5f);
        float z = floorOrigin.z + (gridPos.y * gridCellSize) + (gridCellSize * 0.5f);
        float y = DetermineFloorYPosition(x, z);
        return new Vector3(x, y, z);
    }

    private float DetermineFloorYPosition(float x, float z)
    {
        Ray ray = new Ray(new Vector3(x, floorPlatform.transform.position.y + 50f, z), Vector3.down);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 50f, ~excludedLayers))
        {
            // Debug.Log("Raycast hit at Y position: " + hit.point.y);
            // Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.green, 2f);
            return hit.point.y;
        }
        else
        {
            // Debug.LogWarning("Raycast did not hit the ground layer. Using default floor Y position.");
            // Debug.DrawRay(ray.origin, ray.direction * 50f, Color.red, 2f);
            return floorPlatform.transform.localScale.y;
        }
    }

    public Vector3 GetRandomFreeTilePosition(int sizeX, int sizeZ)
    {
        List<Vector2Int> validPositions = new List<Vector2Int>();

        foreach (var cell in unoccupiedGridPositions)
        {
            if (CanFitObjectAt(cell, sizeX, sizeZ))
            {
                validPositions.Add(cell);
            }
        }

        if (validPositions.Count == 0)
        {
            Debug.LogWarning("No valid positions available for the object size.");
            return new Vector3(float.NaN, float.NaN, float.NaN);
        }

        int randomIndex = UnityEngine.Random.Range(0, validPositions.Count);
        return ConvertPosFromGridToWorld(validPositions[randomIndex]);
    }

    private bool CanFitObjectAt(Vector2Int startCell, int sizeX, int sizeZ)
    {
        for (int x = 0; x < sizeX; x++)
        {
            for (int z = 0; z < sizeZ; z++)
            {
                Vector2Int checkCell = new Vector2Int(startCell.x + x, startCell.y + z);

                // Must be inside the unoccupied list
                if (!unoccupiedGridPositions.Contains(checkCell))
                {
                    return false;
                }
            }
        }

        return true;
    }

    public bool IsCellOccupied(Vector2Int gridPos)
    {
        return occupiedGridPositions.Contains(gridPos);
    }

    public void SetCellOccupied(Vector2Int[] gridPositions, bool occupied)
    {
        if (gridPositions == null || gridPositions.Length == 0)
        {
            Debug.LogWarning("SetCellOccupied called with no positions left available.");
            return;
        }

        // Debug.Log($"Initial Set of Occupied Cells = {occupied}: {string.Join(", ", gridPositions)}");

        // We'll collect per-iteration diagnostics then print a summary at the end.
        List<string> perItemLogs = new List<string>();

        for (int i = 0; i < gridPositions.Length; i++)
        {
            Vector2Int gridPos = gridPositions[i];

            // Check that this cell exists on the grid
            bool cellExists = allGridPositions.Contains(gridPos);
            if (!cellExists)
            {
                perItemLogs.Add($"[{gridPos}] NOT in allGridPositions (out of grid?)");
                // still proceed to try to add/remove so you see effect
            }

            if (occupied)
            {
                bool added = occupiedGridPositions.Add(gridPos);
                bool removedFromUnoccupied = unoccupiedGridPositions.Remove(gridPos);
                perItemLogs.Add($"[{gridPos}] Add-> {added}, removedFromUnoccupied-> {removedFromUnoccupied}, occupiedCountAfterAdd->{occupiedGridPositions.Count}");
            }
            else
            {
                bool removed = occupiedGridPositions.Remove(gridPos);
                bool addedToUnoccupied = unoccupiedGridPositions.Add(gridPos);
                perItemLogs.Add($"[{gridPos}] RemovedFromOccupied-> {removed}, addedToUnoccupied-> {addedToUnoccupied}, occupiedCountAfterRemove->{occupiedGridPositions.Count}");
            }
        }

        // One tidy final debug
        // Debug.Log("SetCellOccupied details:\n" + string.Join("\n", perItemLogs));

        // Now show final sorted lists once (so logs are easier to read)
        List<Vector2Int> occupiedList = new List<Vector2Int>(occupiedGridPositions);
        occupiedList.Sort((a, b) =>
        {
            if (a.x == b.x) return a.y.CompareTo(b.y);
            return a.x.CompareTo(b.x);
        });

        List<Vector2Int> unoccupiedList = new List<Vector2Int>(unoccupiedGridPositions);
        unoccupiedList.Sort((a, b) =>
        {
            if (a.x == b.x) return a.y.CompareTo(b.y);
            return a.x.CompareTo(b.x);
        });

        // Debug.Log("Occupied Grid Positions: " + string.Join(", ", occupiedList));
        // Debug.Log("Unoccupied Grid Positions: " + string.Join(", ", unoccupiedList));
    }

    public void SetUpOccupiedClickableEntityGridPositions(ClickableEntity clickableEntity)
    {
        // Convert the object's center to grid coordinates
        Vector2Int centerGrid = ConvertPosFromWorldToGrid(clickableEntity.transform.position);

        int sizeX = clickableEntity.XSize;
        int sizeZ = clickableEntity.ZSize;

        // Compute half sizes to determine coverage area
        int halfX = Mathf.FloorToInt(sizeX / 2f);
        int halfZ = Mathf.FloorToInt(sizeZ / 2f);

        List<Vector2Int> occupied = new List<Vector2Int>();

        // Loop through each cell the object should cover
        for (int gx = centerGrid.x - halfX; gx < centerGrid.x - halfX + sizeX; gx++)
        {
            for (int gz = centerGrid.y - halfZ; gz < centerGrid.y - halfZ + sizeZ; gz++)
            {
                occupied.Add(new Vector2Int(gx, gz));
            }
        }

        clickableEntity.OccupiedGridPositions = occupied.ToArray();

        // Debug.Log($"[{name}] Size: {sizeX}x{sizeZ}, Center Grid: {centerGrid}, Occupies: {string.Join(", ", OccupiedGridPositions)}");

        SetCellOccupied(clickableEntity.OccupiedGridPositions, true);
    }
    
    #if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (floorPlatform == null) return;
        if (floorXSize <= 0 || floorZSize <= 0) return;

        // Make sure we have origin
        if (floorOrigin == Vector3.zero)
        {
            Renderer rend = floorPlatform.GetComponent<Renderer>();
            if (rend != null)
                floorOrigin = rend.bounds.min;
        }

        // Draw each grid cell
        for (int x = 0; x < floorXSize; x++)
        {
            for (int z = 0; z < floorZSize; z++)
            {
                Vector2Int gridPos = new Vector2Int(x, z);
                Vector3 worldCenter = ConvertPosFromGridToWorld(gridPos);
                Vector3 size = new Vector3(gridCellSize, 0.01f, gridCellSize);

                // Pick color depending on occupancy
                if (occupiedGridPositions.Contains(gridPos))
                    Gizmos.color = new Color(1f, 0f, 0f, 0.5f); // red = occupied
                else
                    Gizmos.color = new Color(0f, 1f, 0f, 0.25f); // green = free

                Gizmos.DrawCube(worldCenter, size * 0.95f);
            }
        }

        // Draw the grid border
        Gizmos.color = Color.white;
        Vector3 topLeft = floorOrigin;
        Vector3 topRight = floorOrigin + new Vector3(floorXSize * gridCellSize, 0, 0);
        Vector3 bottomLeft = floorOrigin + new Vector3(0, 0, floorZSize * gridCellSize);
        Vector3 bottomRight = floorOrigin + new Vector3(floorXSize * gridCellSize, 0, floorZSize * gridCellSize);

        Gizmos.DrawLine(topLeft, topRight);
        Gizmos.DrawLine(topLeft, bottomLeft);
        Gizmos.DrawLine(bottomRight, topRight);
        Gizmos.DrawLine(bottomRight, bottomLeft);
    }
#endif
}
