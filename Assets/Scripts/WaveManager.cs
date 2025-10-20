// using System.Numerics;
using UnityEngine;
using UnityEngine.UIElements;

public class WaveManager : MonoBehaviour
{
    public static WaveManager instance;

    private int destroyedHarvestableObjectsCount = 0;
    public int DestroyedHarvestableObjectsCount
    {
        get => destroyedHarvestableObjectsCount;
        set => destroyedHarvestableObjectsCount = Mathf.Clamp(value, 0, harvestableObjectThisWave);
    }

    public int harvestableObjectThisWave;
    public int[] harvestableObjectCountsPerWave;

    public GameObject harvestableObjectPrefab;
    private float spawningHeightOffset;

    public const int WAVESPERISLAND = 10;

    private int currentWave = 0;
    public int CurrentWave
    {
        get => currentWave;
        set => currentWave = Mathf.Clamp(value, 0, WAVESPERISLAND);
    }

    public int isInMinibossPhase;
    public GameObject minibossPrefab;

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

    public void Start()
    {
        DetermineHarvestableHeightOffset();
        
        CheckIfShouldSpawnWave();

    }
    
    public void DetermineHarvestableHeightOffset()
    {
        spawningHeightOffset = GridManager.instance.floorPlatform.transform.localScale.y;
    }

    public void CheckIfShouldSpawnWave()
    {
        // Scan to see if there are harvestable objects in the scene already
        HarvestableObject[] existingObjects = FindObjectsOfType<HarvestableObject>();
        if (existingObjects.Length > 0)
        {
            harvestableObjectThisWave = existingObjects.Length;
        }
        else
        {
            // Check if we're in mini boss phase, if yes, call miniboss spawn function
            if (isInMinibossPhase == 1)
            {
                // Miniboss spawn function here
                SpawnMiniboss();
            }
            else
            {
                harvestableObjectThisWave = harvestableObjectCountsPerWave[CurrentWave];
                SpawnWave();
            }
        }
    }

    // Spawn New Wave
    public void SpawnWave()
    {
        Debug.Log("Spawning Wave " + (CurrentWave + 1) + " with " + harvestableObjectThisWave + " harvestable objects.");
        for (int i = 0; i < harvestableObjectThisWave; i++)
        {
            Vector3 spawnPos = GetSpawnPos();
            GameObject newObj = Instantiate(harvestableObjectPrefab, spawnPos, Quaternion.identity);
            newObj.GetComponent<HarvestableObject>().myGridCell = GridManager.instance.WorldToGrid(spawnPos);
            
            Vector2Int gridCell = GridManager.instance.WorldToGrid(spawnPos);
            GridManager.instance.SetCellOccupied(gridCell, true);
        }
    }

    // When a harvestable object is destroyed
    public void OnHarvestableDestroyed()
    {
        Debug.Log("Harvestable Object Destroyed!");
        
        DestroyedHarvestableObjectsCount++;
        if (DestroyedHarvestableObjectsCount == harvestableObjectThisWave)
        {
            isInMinibossPhase = 1;
            SpawnMiniboss();
        }
    }

    public void SpawnMiniboss()
    {
        Debug.Log("Spawning Miniboss!");

        // Miniboss spawning logic here
        Vector3 spawnPos = GetSpawnPos();
        GameObject newObj = Instantiate(harvestableObjectPrefab, spawnPos, Quaternion.identity);
        newObj.GetComponent<ClickableEntity>().myGridCell = GridManager.instance.WorldToGrid(spawnPos);
        Vector2Int gridCell = GridManager.instance.WorldToGrid(spawnPos);
        GridManager.instance.SetCellOccupied(gridCell, true);
    }

    public Vector3 GetSpawnPos()
    {
        GridManager.instance.CalculateFloorSize();

        Vector3 spawnPos = GridManager.instance.GetRandomFreeTilePosition();
        spawnPos = GridManager.instance.GetGroundY(spawnPos, 20f, spawningHeightOffset);
        return spawnPos;
    }
    
    public void HandleMinibossFailure()
    {
        Debug.Log("Miniboss Failed! Restarting Wave.");

        // Reset destroyed harvestable objects count
        DestroyedHarvestableObjectsCount = 0;

        // Respawn the wave
        isInMinibossPhase = 0;
        SpawnWave();
    }
}