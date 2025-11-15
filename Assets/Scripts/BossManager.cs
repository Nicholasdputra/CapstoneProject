using UnityEngine;

public class BossManager : MonoBehaviour
{
    public static BossManager Instance;
    public BossDataSO bossDataArray;

    [Header("Events | Listening (For Setup)")]
    public VoidEventChannel OnIslandReadyForBoss;
    [Header("Events | Broadcasting")]
    public IntEventChannel OnBossCompleted;
    public IntEventChannel OnBossFailed;

    public float YOffset;
    public GameObject bossPrefab;
    public BossSO currentBossDetails;
    public GameObject currentBossInstance;

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

    void Start()
    {
        CalculateYOffsetForBoss();
    }

    private void OnEnable()
    {
        OnIslandReadyForBoss.OnEventRaised += HandleIslandReadyForBoss;
    }

    private void OnDisable()
    {
        OnIslandReadyForBoss.OnEventRaised -= HandleIslandReadyForBoss;
    }

    private void HandleIslandReadyForBoss()
    {
        // Logic to spawn boss goes here
        Debug.Log("Island is ready for boss. Spawning boss...");
        currentBossDetails = bossDataArray.bosses[WaveManager.Instance.currentWaveData.waveNumber - 1];
        SpawnBoss(currentBossDetails);
    }

    private void SpawnBoss(BossSO bossData)
    {
        ClickableEntity clickableEntity = bossPrefab.GetComponent<ClickableEntity>();

        // if (clickableEntity == null)
        // {
        //     Debug.LogError("Boss prefab does not have a ClickableEntity component.");
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
        currentBossInstance = Instantiate(bossPrefab, spawnPos, Quaternion.identity);
        currentBossInstance.name = $"Boss_{bossData.bossName}_Wave{WaveManager.Instance.currentWaveData.waveNumber}";

        // Register in lists
        ClickableEntity spawnedEntityClickableEntityComponent = currentBossInstance.GetComponent<ClickableEntity>();

        // Mark occupied cells
        GridManager.Instance.SetUpOccupiedClickableEntityGridPositions(spawnedEntityClickableEntityComponent);

        // Pass along miniboss data to the spawned miniboss
        BaseBoss spawnedBossComponent = currentBossInstance.GetComponent<BaseBoss>();
        spawnedBossComponent.bossData = bossData;
    }

    private void CalculateYOffsetForBoss()
    {
        Renderer bossRenderer = bossPrefab.GetComponent<Renderer>();
        if (bossRenderer != null)
        {
            if (bossRenderer.bounds.size.y > 0f)
            {
                YOffset = Mathf.CeilToInt(bossRenderer.bounds.size.y);
            }
            else
            {
                Debug.LogWarning("Boss renderer height is zero. Using default YOffset of 0.");
                YOffset = 1f;
            }
        }
        else
        {
            Debug.LogWarning("Boss prefab does not have a Renderer component. Using default YOffset of 0.");
            YOffset = 1f;
        }
    }
}
