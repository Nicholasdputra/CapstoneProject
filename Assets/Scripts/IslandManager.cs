using UnityEngine;

public enum IslandState
{
    HarvestPhase,
    MinibossPhase,
    BossPhase
}

public class IslandManager : MonoBehaviour
{
    public static IslandManager Instance;

    # region Variables
    [SerializeField]
    private int currentIslandIndex;
    public int CurrentIslandIndex
    {
        get { return currentIslandIndex; }
        set { currentIslandIndex = value; }
    }

    public IslandState currentState;
    #endregion

    # region Events
    [Header("Events | Listening (For Setup)")]
    public VoidEventChannel OnGridInitialized;
    public bool GridIsReady = false;
    public VoidEventChannel OnPlayerDataLoaded;
    public bool PlayerDataIsLoaded = false;
    
    public VoidEventChannel OnWaveCompleted;
    public IntEventChannel OnMinibossCompleted;
    public VoidEventChannel OnBossCompleted;

    [Header("Broadcasting")]
    public VoidEventChannel OnIslandReadyForWave;
    public VoidEventChannel OnIslandReadyForMiniboss;
    public VoidEventChannel OnIslandReadyForBoss;

    #endregion
    

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

    private void OnEnable()
    {
        OnGridInitialized.OnEventRaised += HandleGridReady;
        OnPlayerDataLoaded.OnEventRaised += HandlePlayerDataLoaded;
        OnWaveCompleted.OnEventRaised += HandleWaveStateCompleted;
    }

    private void OnDisable()
    {
        OnGridInitialized.OnEventRaised -= HandleGridReady;
        OnPlayerDataLoaded.OnEventRaised -= HandlePlayerDataLoaded;
    }

    private void HandleGridReady()
    {
        GridIsReady = true;
        CheckIfCanAdjustIsland();
    }

    private void HandlePlayerDataLoaded()
    {
        PlayerDataIsLoaded = true;
        CheckIfCanAdjustIsland();
    }

    private void HandleWaveStateCompleted()
    {
        // Logic to determine next island state based on wave completion
        if (currentState == IslandState.HarvestPhase)
        {
            currentState = IslandState.MinibossPhase;
        }

        CheckandImplementPhase();
    }

    private void CheckIfCanAdjustIsland()
    {
        if (PlayerDataIsLoaded && GridIsReady)
        {
            // ReadyToAdjustIsland = true;
            // ReadyForIslandAdjustment.RaiseEvent();
            CheckandImplementPhase();
        }
    }

    public void CheckandImplementPhase()
    {
        if (currentState == IslandState.HarvestPhase)
        {
            // Debug.Log("Harvest Phase Triggered");
            // Implement Harvest Phase Logic
            OnIslandReadyForWave.RaiseEvent();
        }
        else if (currentState == IslandState.MinibossPhase)
        {
            // Debug.Log("Miniboss Phase Triggered");
            // Implement Miniboss Phase Logic
            OnIslandReadyForMiniboss.RaiseEvent();

        }
        else if (currentState == IslandState.BossPhase)
        {
            // Debug.Log("Boss Phase Triggered");
            // Implement Boss Phase Logic
            OnIslandReadyForBoss.RaiseEvent();
        }
    }
}