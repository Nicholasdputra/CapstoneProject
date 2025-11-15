using UnityEngine;

public class BossManager : MonoBehaviour
{
    GridManager gridManager;
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
        gridManager = GameObject.FindObjectOfType<GridManager>();
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
        currentBossDetails = bossDataArray.bosses[IslandManager.Instance.CurrentIslandIndex];
        SpawnBoss(currentBossDetails);
    }

    private void SpawnBoss(BossSO bossData)
    {
        ClickableEntity clickableEntity = bossPrefab.GetComponent<ClickableEntity>();

        Vector3 spawnPos = gridManager.GetRandomFreeTilePosition(
            clickableEntity.XSize,
            clickableEntity.ZSize
        );

        float cellSize = gridManager.gridCellSize;
        spawnPos.x += (clickableEntity.XSize * cellSize) / 2f - (cellSize / 2f);
        spawnPos.z += (clickableEntity.ZSize * cellSize) / 2f - (cellSize / 2f);
        spawnPos.y += YOffset;

        currentBossInstance = Instantiate(bossPrefab, spawnPos, Quaternion.identity);
        currentBossInstance.name = $"Boss_{bossData.bossName}_Wave{WaveManager.Instance.currentWaveData.waveNumber}";
        
        BaseBoss spawnedBossComponent = currentBossInstance.GetComponent<BaseBoss>();
        spawnedBossComponent.bossData = bossData;

        currentBossInstance.transform.Find("Square").GetComponent<SpriteRenderer>().sprite = bossData.bossSprite;

        gridManager.SetUpOccupiedClickableEntityGridPositions(spawnedBossComponent);
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
