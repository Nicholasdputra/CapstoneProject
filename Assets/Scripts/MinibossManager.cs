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

    public GameObject minibossPrefab;
    public MinibossSO currentMinibossDetails;

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
        GameObject objectToSpawn = minibossPrefab;
        BaseObject baseObjectComponent = objectToSpawn.GetComponent<BaseObject>();

        // Get the base world position (center of tile)
        Vector3 spawnPos = GridManager.Instance.GetRandomFreeTilePosition(
            baseObjectComponent.XSize,
            baseObjectComponent.ZSize
        );

        // Apply an offset so pivot = bottom-left corner
        float cellSize = GridManager.Instance.gridCellSize; // or whatever your tile size var is called
        spawnPos.x += (baseObjectComponent.XSize * cellSize) / 2f - (cellSize / 2f);
        spawnPos.z += (baseObjectComponent.ZSize * cellSize) / 2f - (cellSize / 2f);

        // Instantiate
        GameObject spawnedObject = Instantiate(objectToSpawn, spawnPos, Quaternion.identity);
        spawnedObject.name = $"Miniboss_{minibossData.minibossName}_Wave{WaveManager.Instance.currentWaveData.waveNumber}";

        // Register in lists
        BaseObject spawnedBaseObjectComponent = spawnedObject.GetComponent<BaseObject>();

        // Mark occupied cells
        GridManager.Instance.SetUpOccupiedClickableEntityGridPositions(spawnedBaseObjectComponent);

        // Pass along miniboss data to the spawned miniboss
        BaseMiniboss spawnedMinibossComponent = spawnedObject.GetComponent<BaseMiniboss>();
        spawnedMinibossComponent.minibossData = minibossData;
    }
}
