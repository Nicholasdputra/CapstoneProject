using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;

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
    public IntEventChannel OnBossCompleted;

    public IntEventChannel OnMinibossFailed;
    public IntEventChannel OnBossFailed;

    [Header("Broadcasting")]
    public VoidEventChannel OnIslandReadyForWave;
    public VoidEventChannel OnIslandReadyForMiniboss;
    public VoidEventChannel OnIslandRepeatWave;
    public VoidEventChannel OnIslandReadyForBoss;

    #endregion
    GridManager gridManager;

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

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
    
    private void Start()
    {
        AudioManager.Instance.PlayBGMLoop(1);
        CurrentIslandIndex = 0;
    }

    private void OnEnable()
    {
        OnGridInitialized.OnEventRaised += HandleGridReady;
        OnPlayerDataLoaded.OnEventRaised += HandlePlayerDataLoaded;
        OnWaveCompleted.OnEventRaised += HandleWaveStateCompleted;
        OnMinibossCompleted.OnEventRaised += HandleMinibossStateCompleted;
        OnBossCompleted.OnEventRaised += HandleBossStateCompleted;
        OnMinibossFailed.OnEventRaised += HandleMinibossStateFailed;
        OnBossFailed.OnEventRaised += HandleBossStateCompleted;
    }

    private void OnDisable()
    {
        OnGridInitialized.OnEventRaised -= HandleGridReady;
        OnPlayerDataLoaded.OnEventRaised -= HandlePlayerDataLoaded;
        OnWaveCompleted.OnEventRaised -= HandleWaveStateCompleted;
        OnMinibossCompleted.OnEventRaised -= HandleMinibossStateCompleted;
        OnBossCompleted.OnEventRaised -= HandleBossStateCompleted;
        OnMinibossFailed.OnEventRaised -= HandleMinibossStateFailed;
        OnBossFailed.OnEventRaised -= HandleBossStateCompleted;
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
        if (currentState == IslandState.HarvestPhase && WaveManager.Instance.currentWaveData.waveNumber < WaveManager.MAXWAVESPERISLAND)
        {
            // Debug.Log("Transitioning to Miniboss Phase as current wave number is " + WaveManager.Instance.currentWaveData.waveNumber);
            currentState = IslandState.MinibossPhase;
        }
        else if (currentState == IslandState.HarvestPhase && WaveManager.Instance.currentWaveData.waveNumber >= WaveManager.MAXWAVESPERISLAND)
        {
            // Debug.Log("Transitioning to Boss Phase as max waves per island reached.");
            currentState = IslandState.BossPhase;
        }

        CheckandImplementPhase(1);
    }

    private void HandleMinibossStateCompleted(int minibossWaveNumber)
    {
        Debug.Log("Miniboss wave number: " + minibossWaveNumber);
        // Logic to determine next island state based on miniboss completion
        if (currentState == IslandState.MinibossPhase)
        {
            if (minibossWaveNumber >= WaveManager.MAXWAVESPERISLAND)
            {
                currentState = IslandState.BossPhase;
            }
            else
            {
                currentState = IslandState.HarvestPhase;
            }
        }
        
        CheckandImplementPhase(1);
    }

    private void HandleMinibossStateFailed(int minibossWaveNumber)
    {
        Debug.Log("Miniboss wave number: " + minibossWaveNumber);
        // Logic to determine next island state based on miniboss completion
        if (currentState == IslandState.MinibossPhase)
        {
            if (minibossWaveNumber >= WaveManager.MAXWAVESPERISLAND)
            {
                currentState = IslandState.BossPhase;
            }
            else
            {
                currentState = IslandState.HarvestPhase;
            }
        }

        CheckandImplementPhase(0);
    }
    
    private void HandleBossStateCompleted(int waveNumber)
    {
        // Logic to handle island completion after boss is defeated
        Debug.Log("Boss defeated! Continuing to the next resource island...");
        if (currentState == IslandState.BossPhase)
        {
            CurrentIslandIndex += 1;
            currentState = IslandState.HarvestPhase;
        }
        CheckandImplementPhase(1);
    }

    

    private void CheckIfCanAdjustIsland()
    {
        if (PlayerDataIsLoaded && GridIsReady)
        {
            // ReadyToAdjustIsland = true;
            // ReadyForIslandAdjustment.RaiseEvent();
            CheckandImplementPhase(1);
        }
    }

    public void CheckandImplementPhase(int succeeded)
    {
        Debug.Log("Implementing Island Phase: " + currentState.ToString());
        if (currentState == IslandState.HarvestPhase)
        {
            // Debug.Log("Harvest Phase Triggered");
            // Implement Harvest Phase Logic
            
            // Clear grid occupied positions from previous phases if needed
            
            gridManager = GameObject.FindObjectOfType<GridManager>();
            gridManager.ClearAllOccupiedPositions();
            if(succeeded == 1)
            {
                OnIslandReadyForWave.RaiseEvent();
            }
            else if(succeeded == 0)
            {
                OnIslandRepeatWave.RaiseEvent();
            }
        }
        else if (currentState == IslandState.MinibossPhase)
        {
            // Debug.Log("Miniboss Phase Triggered");
            // Implement Miniboss Phase Logic

            gridManager = GameObject.FindObjectOfType<GridManager>();

            gridManager.ClearAllOccupiedPositions();
            OnIslandReadyForMiniboss.RaiseEvent();

        }
        else if (currentState == IslandState.BossPhase)
        {
            // Debug.Log("Boss Phase Triggered");
            // Move to boss island scene
            AudioManager.Instance.PlayBGMLoop(2);
            gridManager = GameObject.FindObjectOfType<GridManager>();

            if (SceneManager.GetActiveScene().name != "BossIsland" + (CurrentIslandIndex + 1).ToString())
            {
                StartCoroutine(LoadBossSceneAndTriggerEvent());
            }
        }
    }

    private IEnumerator LoadBossSceneAndTriggerEvent()
    {
        string sceneName = "BossIsland" + (CurrentIslandIndex + 1).ToString();
        GridManager gridManager = GameObject.FindObjectOfType<GridManager>();
        AudioManager.Instance.PlayBGMLoop(2);
        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);

        // Wait until scene is fully loaded
        while (!op.isDone)
            yield return null;

        // Now the scene is ready → NOW spawn boss
        Debug.Log("Boss Scene Loaded Successfully. Triggering OnIslandReadyForBoss.");
        OnIslandReadyForBoss.RaiseEvent();
    }

    public void DisplayDialogue(string dialogue, TextMeshProUGUI dialogueText, GameObject dialogueObject)
    {
        StartCoroutine(DisplayDialogueCoroutine(dialogue, dialogueText, dialogueObject));
    }   

    public IEnumerator DisplayDialogueCoroutine(string dialogue, TextMeshProUGUI dialogueText, GameObject dialogueObject)
    {
        AudioManager.Instance.PlaySFXOnce(3);
        if (dialogueText == null)
        {
            Debug.LogError("DialogueText UI object NOT FOUND in ObjectsCanvas.");
            yield break;
        }

        // Initialize dialogue display
        Debug.Log("Starting Dialogue Display: " + dialogue);
        dialogueObject.SetActive(true);
        dialogueText.text = "";
        Color originalColor = dialogueText.color;
        originalColor.a = 1f;
        dialogueText.color = originalColor;

        // Type letter by letter
        Debug.Log("Typing out dialogue...");
        float typeSpeed = 0.02f;
        foreach (char letter in dialogue.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typeSpeed);
        }

        // Show the fully typed text for a few seconds
        yield return new WaitForSeconds(3f);

        // Fade out the dialogue
        float fadeDuration = 1f;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            float t = elapsedTime / fadeDuration;
            Color c = dialogueText.color;
            c.a = Mathf.Lerp(1f, 0f, t);
            dialogueText.color = c;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure it's fully transparent at the end
        Debug.Log("Fading out dialogue...");
        Color final = dialogueText.color;
        final.a = 0f;
        dialogueText.color = final;

        // Deactivate and clean up dialogue object
        Debug.Log("Dialogue display complete.");
        if (dialogueObject != null)
        {
            dialogueObject.SetActive(false);
            dialogueText.text = "";
            originalColor.a = 1f;
            dialogueText.color = originalColor;
        }
    }
}