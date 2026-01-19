using UnityEngine;

public class MinibossManager : MonoBehaviour
{
    GridManager gridManager;
    public static MinibossManager Instance;
    public MiniBossData[] minibossDataArray;

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
            DontDestroyOnLoad(gameObject);
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
        Debug.Log("Current Island Index: " + IslandManager.Instance.CurrentIslandIndex);
        Debug.Log("Current Wave Number: " + WaveManager.Instance.currentWaveData.waveNumber);
        currentMinibossDetails = minibossDataArray[IslandManager.Instance.CurrentIslandIndex].minibosses[WaveManager.Instance.currentWaveData.waveNumber - 1];
        SpawnMiniboss(currentMinibossDetails);
    }

    private void SpawnMiniboss(MinibossSO minibossData)
    {
        gridManager = GameObject.FindObjectOfType<GridManager>();
        ClickableEntity clickableEntity = minibossPrefab.GetComponent<ClickableEntity>();
        float cellSize = gridManager.gridCellSize;

        // --------- TRUE GRID CENTER (CELL-BASED) ---------
        int centerX = gridManager.floorXSize / 2;
        int centerZ = gridManager.floorZSize / 2;

        // Get the EXACT center cell in world space
        Vector3 spawnPos =
            gridManager.ConvertPosFromGridToWorld(new Vector2Int(centerX, centerZ));

        // Correct for multi-tile boss (VERY IMPORTANT)
        spawnPos.x -= (clickableEntity.XSize - 1) * (cellSize / 2f);
        spawnPos.z -= (clickableEntity.ZSize - 1) * (cellSize / 2f);

        // Lift boss onto platform
        spawnPos.y += YOffset / 2f;
        // ----------------------------------------------

        currentMinibossInstance =
            Instantiate(minibossPrefab, spawnPos, Quaternion.identity);

        currentMinibossInstance.name =
            $"Miniboss_{minibossData.minibossName}_Wave{WaveManager.Instance.currentWaveData.waveNumber}";

        ClickableEntity spawnedEntity =
            currentMinibossInstance.GetComponent<ClickableEntity>();

        gridManager.SetUpOccupiedClickableEntityGridPositions(spawnedEntity);

        BaseMiniboss boss =
            currentMinibossInstance.GetComponent<BaseMiniboss>();

        boss.minibossData = minibossData;

        currentMinibossInstance.transform
            .Find("Square")
            .GetComponent<SpriteRenderer>()
            .sprite = minibossData.minibossSprite;
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