using UnityEngine;
using System.Collections.Generic;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;
    public int detectionRadius = 0;
    public LayerMask excludedLayers;

    private Camera cam;

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
        cam = Camera.main;
    }

    private void Update()
    {
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

        List<ClickableEntity> entities = GetEntitiesInRadius(hoveredGrid.Value, detectionRadius);

        foreach (var entity in entities)
        {
            entity.OnHover(); // Call the hover event
        }
    }

    void HandleClick()
    {
        if (!Input.GetMouseButtonDown(0))
            return;

        Vector2Int? hoveredGrid = GetHoveredGridCell();
        // Debug.Log("Clicked at grid: " + (hoveredGrid.HasValue ? hoveredGrid.Value.ToString() : "null"));
        if (hoveredGrid == null)
            return;

        List<ClickableEntity> entities = GetEntitiesInRadius(hoveredGrid.Value, detectionRadius);

        foreach (var entity in entities)
        {
            entity.OnClick(); // Call the click event
        }
    }

    Vector2Int? GetHoveredGridCell()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 200f, ~excludedLayers))
        {
            return GridManager.Instance.ConvertPosFromWorldToGrid(hit.point);
        }

        return null;
    }

    List<ClickableEntity> GetEntitiesInRadius(Vector2Int center, int radius)
    {
        List<ClickableEntity> results = new List<ClickableEntity>();

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
        //List all the entities found in radius for debugging
        // Debug.Log("Entities in radius: " + results.Count);
        // Debug.Log("Entities: " + string.Join(", ", results));

        return results;
    }
}
