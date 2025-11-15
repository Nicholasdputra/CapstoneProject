using System;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public static int MAXWAVESPERISLAND = 10;

    public static WaveManager Instance;
    public IntEventChannel WaveChangedEvent;

    public WaveData[] waveDataArray;
    public WaveSO currentWaveData;

    public List<BaseObject> currentAliveEnemies;

    #region Events
    [Header("Events")]
    [Header("Listening")]
    public VoidEventChannel OnIslandReadyForWave;
    [Header("Broadcasting")]
    public VoidEventChannel OnWaveCompleted;
    #endregion

    public GameObject postWaveClearButtons;
    private GameObject goToMinibossButton;
    private GameObject refreshCurrentWaveButton;

    void Awake()
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

    void OnEnable()
    {
        OnIslandReadyForWave.OnEventRaised += SpawnNextWave;    
    }

    private void OnDisable()
    {
        OnIslandReadyForWave.OnEventRaised -= SpawnNextWave;
    }

    private void SpawnNextWave()
    {
        // Debug.Log("Spawning Next Wave: " + (currentWaveData != null ? currentWaveData.waveNumber + 1 : 1));
        if (currentWaveData == null)
        {
            StartWave(1);
            return;
        }
        else
        {
            StartWave(currentWaveData.waveNumber + 1);
        }
    }
    
    void Start()
    {
        Initialize();
    }

    void Initialize()
    {
        currentAliveEnemies = new List<BaseObject>();
        goToMinibossButton = postWaveClearButtons.transform.Find("GoToMinibossButton").gameObject;
        refreshCurrentWaveButton = postWaveClearButtons.transform.Find("RefreshIslandButton").gameObject;
        postWaveClearButtons.SetActive(false);
    }

    public void StartWave(int waveIndex)
    {
        int adjustedIndex = waveIndex - 1;
        if (adjustedIndex < waveDataArray[IslandManager.Instance.CurrentIslandIndex].harvestableWaves.Length)
        {
            currentWaveData = waveDataArray[IslandManager.Instance.CurrentIslandIndex].harvestableWaves[adjustedIndex];
            WaveChangedEvent.RaiseEvent(currentWaveData != null ? currentWaveData.waveNumber : 1);
            SpawnObjectsForWave(currentWaveData);
        }
        else
        {
            Debug.Log("All waves completed!");
        }
    }

    public void SpawnObjectsForWave(WaveSO waveData)
    {
        int totalEnemiesInWave = UnityEngine.Random.Range(waveData.minEnemies, waveData.maxEnemies + 1);
        for (int i = 0; i < totalEnemiesInWave; i++)
        {
            GameObject objectToSpawn = waveData.objectPrefabs[UnityEngine.Random.Range(0, waveData.objectPrefabs.Length)];
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
            spawnedObject.name = $"{objectToSpawn.name}_Wave{waveData.waveNumber}_Harvestable{i + 1}";

            // Register in lists
            BaseObject spawnedBaseObjectComponent = spawnedObject.GetComponent<BaseObject>();
            currentAliveEnemies.Add(spawnedBaseObjectComponent);

            // Mark occupied cells
            GridManager.Instance.SetUpOccupiedClickableEntityGridPositions(spawnedBaseObjectComponent);
        }
    }
    
    public void CheckIfWaveCompleted()
    {
        currentAliveEnemies.RemoveAll(item => item == null);
        if (currentAliveEnemies.Count == 0)
        {
            WaveCompleted();
        }
    }

    public void WaveCompleted()
    {
        currentAliveEnemies.RemoveAll(item => item == null);
        if (currentAliveEnemies.Count == 0)
        {
            Debug.Log("Wave " + currentWaveData.waveNumber + " completed!");
            postWaveClearButtons.SetActive(true);
        }
        else
        {
            Debug.LogWarning("Something is wrong! Not all enemies are defeated yet.");
            Debug.Log("There are still " + currentAliveEnemies.Count + " enemies alive.");
        }
    }

    public void RefreshCurrentWave()
    {
        postWaveClearButtons.SetActive(false);
        SpawnObjectsForWave(currentWaveData);
    }

    public void GoToMiniboss()
    {
        OnWaveCompleted.RaiseEvent();
    }
}