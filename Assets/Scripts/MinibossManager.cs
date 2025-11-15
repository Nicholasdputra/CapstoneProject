using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinibossManager : MonoBehaviour
{
    public static MinibossManager Instance;
    public MiniBossData minibossDataArray;

    [Header("Events | Listening (For Setup)")]
    public VoidEventChannel OnIslandReadyForMiniboss;

    [Header("Events | Broadcasting")]
    public IntEventChannel OnMinibossCompleted;
    public IntEventChannel OnMinibossFailed;

    public float YOffset;
    public GameObject minibossPrefab;
    public MinibossSO currentMinibossDetails;
    public GameObject currentMinibossInstance;

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

    public void Start()
    {
        CalculateYOffsetForMiniboss();
    }

    private void OnEnable()
    {
        OnIslandReadyForMiniboss.OnEventRaised += HandleIslandReadyForMiniboss;
    }

    private void OnDisable()
    {
        OnIslandReadyForMiniboss.OnEventRaised -= HandleIslandReadyForMiniboss;
    }

    private void HandleIslandReadyForMiniboss()
    {
        // Logic to spawn miniboss goes here
        Debug.Log("Island is ready for miniboss. Spawning miniboss...");
        currentMinibossDetails = minibossDataArray.minibosses[WaveManager.Instance.currentWaveData.waveNumber - 1];
        SpawnMiniboss(currentMinibossDetails);
    }

    private void SpawnMiniboss(MinibossSO minibossData)
    {
        ClickableEntity clickableEntity = minibossPrefab.GetComponent<ClickableEntity>();

        // if (clickableEntity == null)
        // {
        //     Debug.LogError("The miniboss prefab does not have a ClickableEntity component.");
        //     return;
        // }
        // if (GridManager.Instance == null)
        // {
        //     Debug.LogError("GridManager Instance is null. Cannot spawn miniboss.");
        //     return;
        // }

        // Get the base world position (center of tile)
        Vector3 spawnPos = GridManager.Instance.GetRandomFreeTilePosition(
            clickableEntity.XSize,
            clickableEntity.ZSize
        );

        // Apply an offset so pivot = bottom-left corner
        float cellSize = GridManager.Instance.gridCellSize;
        spawnPos.x += (clickableEntity.XSize * cellSize) / 2f - (cellSize / 2f);
        spawnPos.z += (clickableEntity.ZSize * cellSize) / 2f - (cellSize / 2f);
        spawnPos.y += YOffset / 2f;

        // Instantiate
        currentMinibossInstance = Instantiate(minibossPrefab, spawnPos, Quaternion.identity);
        currentMinibossInstance.name = $"Miniboss_{minibossData.minibossName}_Wave{WaveManager.Instance.currentWaveData.waveNumber}";

        // Register in lists
        ClickableEntity spawnedEntityClickableEntityComponent = currentMinibossInstance.GetComponent<ClickableEntity>();

        // Mark occupied cells
        GridManager.Instance.SetUpOccupiedClickableEntityGridPositions(spawnedEntityClickableEntityComponent);

        // Pass along miniboss data to the spawned miniboss
        BaseMiniboss spawnedMinibossComponent = currentMinibossInstance.GetComponent<BaseMiniboss>();
        spawnedMinibossComponent.minibossData = minibossData;
    }

    private void CalculateYOffsetForMiniboss()
    {
        Renderer rend = minibossPrefab.GetComponent<Renderer>();
        if (rend != null)
        {
            if (rend.bounds.size.y > 0f)
            {
                YOffset = Mathf.CeilToInt(rend.bounds.size.y);
            }
            else
            {
                YOffset = 1;
            }
        }
        else
        {
            YOffset = 1;
        }
    }

    
}
