using UnityEngine;
using System.Collections.Generic;

public class InputManager : MonoBehaviour
{
    GridManager gridManager;
    public static InputManager Instance;
    public LayerMask excludedLayers;

    private Camera cam;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);  
        }
    }

    private void Start()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        if (!cam)
            cam = Camera.main;
        HandleHover();
        HandleClick();
    }

    void HandleHover()
    {
        Vector2Int? hoveredGrid = GetHoveredGridCell();
        if (hoveredGrid == null)
        {
            return;
        }

        List<ClickableEntity> entities = GetEntitiesInRadius(hoveredGrid.Value, PlayerDataManager.Instance.currentHarvestRadius);

        foreach (var entity in entities)
        {
            entity.OnHover(); // Call the hover event
        }
    }

    void HandleClick()
    {
        AudioManager.Instance.PlaySFXOnce(1);
        if (!Input.GetMouseButtonDown(0))
            return;
        Debug.Log("Mouse Click Detected");
        Vector2Int? hoveredGrid = GetHoveredGridCell();
        // Debug.Log("Clicked at grid: " + (hoveredGrid.HasValue ? hoveredGrid.Value.ToString() : "null"));
        if (hoveredGrid == null)
        {
            return;
        }
        int radius = PlayerDataManager.Instance.currentHarvestRadius;
        Debug.Log("Click radius: " + radius);
        List<ClickableEntity> entities = GetEntitiesInRadius(hoveredGrid.Value, radius);
        Debug.Log("Entities list members: " + string.Join(", ", entities));
        foreach (var entity in entities)
        {
            Debug.Log("Clicking on entity: " + entity.name);
            entity.OnClick(); // Call the click event
        }
    }

    Vector2Int? GetHoveredGridCell()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 200f, ~excludedLayers))
        {

            gridManager = GameObject.FindObjectOfType<GridManager>();

            return gridManager.ConvertPosFromWorldToGrid(hit.point);
        }

        return null;
    }

    List<ClickableEntity> GetEntitiesInRadius(Vector2Int center, int radius)
    {
        List<ClickableEntity> results = new List<ClickableEntity>();
        if (IslandManager.Instance.currentState == IslandState.HarvestPhase)
        {
            results = HarvestableDetectionMethod(center, radius, results);
        }
        else if (IslandManager.Instance.currentState == IslandState.MinibossPhase)
        {
            results = MinibossDetectionMethod(center, radius, results);
        }
        else if (IslandManager.Instance.currentState == IslandState.BossPhase)
        {
            results = BossDetectionMethod(center, radius, results);
        }

        // List all the entities found in radius for debugging
        // Debug.Log("Entities in radius: " + results.Count);
        // Debug.Log("Entities: " + string.Join(", ", results));
        // Debug.Log("Results list members: " + string.Join(", ", results));
        return results;
    }

    List<ClickableEntity> HarvestableDetectionMethod(Vector2Int center, int radius, List<ClickableEntity> results)
    {
        // Implement harvestable-specific detection logic here
        foreach (var enemy in WaveManager.Instance.currentAliveEnemies)
        {
            foreach (var gridPos in enemy.OccupiedGridPositions)
            {
                int dx = Mathf.Abs(gridPos.x - center.x);
                int dz = Mathf.Abs(gridPos.y - center.y);

                if (dx <= radius && dz <= radius)
                {
                    if (!results.Contains(enemy))
                        results.Add(enemy);

                    break;
                }
            }
        }

        return results;
    }

    List<ClickableEntity> MinibossDetectionMethod(Vector2Int center, int radius, List<ClickableEntity> results)
    {
        // Implement miniboss-specific detection logic here
        ClickableEntity miniBoss = MinibossManager.Instance.currentMinibossInstance.GetComponent<ClickableEntity>();
        Vector2Int[] miniBossGridPositions = miniBoss.OccupiedGridPositions;

        foreach (Vector2Int gridPos in miniBossGridPositions)
        {
            int dx = Mathf.Abs(gridPos.x - center.x);
            int dz = Mathf.Abs(gridPos.y - center.y);

            if (dx <= radius && dz <= radius)
            {
                if (!results.Contains(miniBoss))
                {
                    // Debug.Log("Miniboss detected in radius at grid position: " + gridPos);
                    results.Add(miniBoss);
                }
                
                break;
            }
        }
        return results;
    }
    
    List<ClickableEntity> BossDetectionMethod(Vector2Int center, int radius, List<ClickableEntity> results)
    {
        // Implement boss-specific detection logic here
        return results;
    }
}
