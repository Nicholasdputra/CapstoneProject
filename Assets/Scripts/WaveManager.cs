// using System.Numerics;
using UnityEngine;

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

    public GameObject harvestableObjectPrefab;

    public const int WAVESPERISLAND = 10;

    private int currentWave = 0;
    public int CurrentWave
    {
        get => currentWave;
        set => currentWave = Mathf.Clamp(value, 0, WAVESPERISLAND);
    }

    public int isInMinibossPhase;

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

    // Spawn New Wave
    public void SpawnWave()
    {
        for (int i = 0; i < harvestableObjectThisWave; i++)
        {
            GridManager.instance.CalculateFloorSize();

            Vector3 spawnPos = GridManager.instance.GetRandomFreeTilePosition(GridManager.instance.floorXSize, GridManager.instance.floorZSize);
            spawnPos = GridManager.instance.GetGroundY(spawnPos, 20f);

            GameObject newObj = Instantiate(harvestableObjectPrefab, spawnPos, Quaternion.identity);
            newObj.GetComponent<HarvestableObject>().myGridCell = GridManager.instance.WorldToGrid(spawnPos);

            // Mark cell as occupied
            Vector2Int gridCell = GridManager.instance.WorldToGrid(spawnPos);
            GridManager.instance.SetCellOccupied(gridCell, true);
        }
    }

    // When a harvestable object is destroyed
    public void OnHarvestableDestroyed()
    {
        DestroyedHarvestableObjectsCount++;
        if (DestroyedHarvestableObjectsCount == harvestableObjectThisWave)
        {
            DestroyedHarvestableObjectsCount = 0;
            CurrentWave++;
            if (CurrentWave < WAVESPERISLAND)
            {
                SpawnWave();
            }
            else // Miniboss phase
            {
                isInMinibossPhase = 1; //set to true
                CurrentWave = 0;
                
                // However the miniboss thingy works
                
            }
        }
    }
}