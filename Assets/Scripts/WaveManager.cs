using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public static WaveManager Instance;
    
    [SerializeField] private int destroyedHarvestableObjectsCount = 0;
    public int DestroyedHarvestableObjectsCount
    {
        get => destroyedHarvestableObjectsCount;
        set => destroyedHarvestableObjectsCount = Mathf.Clamp(value, 0, harvestableObjectThisWave);
    }

    public int harvestableObjectThisWave;
    public int[] harvestableObjectCountsPerWave;

    public GameObject harvestableObjectPrefab;

    public const int MAXWAVEINDEX = 9;

    private int currentWaveIndex = 0;
    public int CurrentWaveIndex 
    {
        get => currentWaveIndex;
        set => currentWaveIndex = Mathf.Clamp(value, 0, MAXWAVEINDEX);
    }

    public int isInMinibossPhase;
    [SerializeField] public GameObject minibossPrefab;

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

    public void Start()
    {   
        CheckIfShouldSpawnWave();
    }

    public void CheckIfShouldSpawnWave()
    {
        Debug.Log("Checking if we should spawn wave...");
        if (IslandManager.Instance.isInBossPhase == 1)
        {
            Debug.Log("We are in boss phase, not spawning wave.");
            return;
        }

        // Scan to see if there are harvestable objects in the scene already
        HarvestableObject[] existingObjects = FindObjectsOfType<HarvestableObject>();
        if (existingObjects.Length > 0)
        {
            harvestableObjectThisWave = existingObjects.Length;
        }
        else
        {
            //Check to see if there's a miniboss in scene
            if (FindObjectOfType<MiniBossScript>() != null)
            {
                isInMinibossPhase = 1;
                return;
            }
            else
            {
                isInMinibossPhase = 0;
            }

            // Check if we're in mini boss phase, if yes, call miniboss spawn function
            if (isInMinibossPhase == 1)
            {
                // Miniboss spawn function here
                SpawnMiniboss();
            }
            else
            {
                harvestableObjectThisWave = harvestableObjectCountsPerWave[CurrentWaveIndex];
                SpawnWave();
            }
        }
    }

    public void RefreshIsland()
    {
        Debug.Log("Refreshing Island for Wave: " + CurrentWaveIndex);
        DestroyedHarvestableObjectsCount = 0;
        isInMinibossPhase = 0;
        SpawnWave();
    }

    public void ContinueToBoss()
    {

    }

    // Spawn New Wave
    public void SpawnWave()
    {
        harvestableObjectThisWave = harvestableObjectCountsPerWave[CurrentWaveIndex];

        (int occupiedCells, int totalCells) = GridManager.Instance.MarkGridAvailability();
        
        if (harvestableObjectThisWave > (totalCells - occupiedCells))
        {
            Debug.LogWarning("Not enough free tiles to spawn all objects!");
            harvestableObjectThisWave = totalCells - occupiedCells;
        }
        GridManager.Instance.DetermineHeightOffset();
        for (int i = 0; i < harvestableObjectThisWave; i++)
        {
            Vector3 spawnPos = GridManager.Instance.GetSpawnPos();

            if (spawnPos == Vector3.zero)
            {
                Debug.LogWarning("No free spawn position available, stopping spawn.");
                break;
            }

            GameObject newObj = Instantiate(harvestableObjectPrefab, spawnPos, Quaternion.identity);
            newObj.GetComponent<HarvestableObject>().myGridCell = GridManager.Instance.WorldToGrid(spawnPos);
            
            Vector2Int gridCell = GridManager.Instance.WorldToGrid(spawnPos);
            GridManager.Instance.SetCellOccupied(gridCell, true);
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
        Vector3 spawnPos = GridManager.Instance.GetSpawnPos();
        GameObject newObj = Instantiate(minibossPrefab, spawnPos, Quaternion.identity);
        newObj.GetComponent<ClickableEntity>().myGridCell = GridManager.Instance.WorldToGrid(spawnPos);
        Vector2Int gridCell = GridManager.Instance.WorldToGrid(spawnPos);
        GridManager.Instance.SetCellOccupied(gridCell, true);
    }

    public void LoseAgainstMiniboss()
    {
        Debug.Log("Failed To Defeat Miniboss! Restarting Wave.");

        // Reset destroyed harvestable objects count
        DestroyedHarvestableObjectsCount = 0;

        // Respawn the wave
        isInMinibossPhase = 0;
        SpawnWave();
    }
    
    public void WinAgainstMiniboss()
    {
        Debug.Log("Miniboss Defeated!");
        
        // Reset destroyed harvestable objects count
        DestroyedHarvestableObjectsCount = 0;

        // Proceed to next wave
        isInMinibossPhase = 0;

        Debug.Log("From wave number: " + CurrentWaveIndex);
        if (CurrentWaveIndex != MAXWAVEINDEX)
        {
            CurrentWaveIndex++;
            PlayerData.Instance.playerHUD.UpdateCurrentWaveIndexText();
            Debug.Log("We will refresh the island since it is not yet the 10th wave, Next Wave: " + (CurrentWaveIndex) + "Max Wave: " + MAXWAVEINDEX);
            SpawnWave();
        }
        else
        {
            Debug.Log("We will go fight the final boss, as it is wave: " + (CurrentWaveIndex));
            //Bring to new island to fight boss
            IslandManager.Instance.GoToBossIsland();
        }
    }
}