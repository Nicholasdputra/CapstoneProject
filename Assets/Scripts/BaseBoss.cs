using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BaseBoss : ClickableEntity
{
    protected Canvas objectsCanvas;
    
    public BossSO bossData;

    [Header("Health Bar Elements")]
    public string bossName;
    [SerializeField] protected GameObject healthBarObject;
    [SerializeField] protected TextMeshProUGUI bossNameText;
    [SerializeField] protected Image healthBar;
    [SerializeField] protected TextMeshProUGUI healthText;
    
    [Header("Timer Bar Elements")]
    [SerializeField] protected GameObject timerBarObject;
    [SerializeField] protected Image timerBar;
    public int timer;
    public int timeLimit;
    public Coroutine timerCoroutine;

    [Header("Dialogue Elements")]
    [SerializeField] protected GameObject dialogueObject;
    public string dialogue;
    public Coroutine dialogueCoroutine;
    protected TextMeshProUGUI dialogueText;
    
    GridManager gridManager;
    void Start()
    {
        Initialize();
    }

    public override void Initialize()
    {
        gridManager = GameObject.FindObjectOfType<GridManager>();
        Debug.Log("Initializing Boss Type Entity with data: " + bossData);
        // Base Stats
        MaxHealth = bossData.health;
        CurrentHealth = MaxHealth;
        DreamEssenceDrop = bossData.dreamEssenceDrop;
        SoulEssenceDrop = bossData.soulEssenceDrop;
        HumanSoulDrop = bossData.humanSoulDrop;
        bossName = bossData.bossName;
        dialogue = bossData.dialogue;
        
        // Find object named ObjectsCanvas
        objectsCanvas = GameObject.Find("ObjectsCanvas").GetComponent<Canvas>();

        Transform[] allChildren = objectsCanvas.GetComponentsInChildren<Transform>(true);
        foreach (Transform child in allChildren)
        {
            if (child.name == "HealthBarObject")
            {
                healthBarObject = child.gameObject;
                healthBarObject.SetActive(true);
                continue;
            }

            if (child.name == "TimerBarObject")
            {
                timerBarObject = child.gameObject;
                timerBarObject.SetActive(true);
                continue;
            }

            if (child.name == "DialogueObject")
            {
                dialogueObject = child.gameObject;
                dialogueObject.SetActive(false);
                continue;
            }

            if (child.name == "BossName")
            {
                bossNameText = child.GetComponent<TextMeshProUGUI>();
                bossNameText.text = bossName;
                continue;
            }

            if (child.name == "HealthFill")
            {
                healthBar = child.GetComponent<Image>();
                healthBar.fillAmount = 0f;
                continue;
            }

            if (child.name == "TimerFill")
            {
                timerBar = child.GetComponent<Image>();
                timerBar.fillAmount = 0f;
                continue;
            }

            if (child.name == "HealthText")
            {
                healthText = child.GetComponent<TextMeshProUGUI>();
                healthText.text = CurrentHealth.ToString() + " / " + MaxHealth.ToString();
                healthText.gameObject.SetActive(false);
                healthBarObject.SetActive(true);
                continue;
            }

            if(child.name == "DialogueText")
            {
                dialogueText = child.GetComponent<TextMeshProUGUI>();
                dialogueText.text = "";
                continue;
            }
        }
        StartCoroutine(FillUpHealthBar());
        StartCoroutine(FillUpTimerBar());
    }

    public IEnumerator FillUpHealthBar()
    {
        float fillSpeed = 0.5f;
        float targetFill = 1f;

        while (healthBar.fillAmount < targetFill)
        {
            healthBar.fillAmount += fillSpeed * Time.deltaTime;
            yield return null;
        }

        healthBar.fillAmount = targetFill;
        // Debug.Log("Health Bar Filled");
        healthText.gameObject.SetActive(true);
        isClickable = true;

        // Start the timer coroutine once the health bar is fulLly filled
        timerCoroutine = StartCoroutine(TimerCoroutine());
    }

    public IEnumerator FillUpTimerBar()
    {
        float fillSpeed = 0.5f;
        float targetFill = 1f;

        while (timerBar.fillAmount < targetFill)
        {
            timerBar.fillAmount += fillSpeed * Time.deltaTime;
            yield return null;
        }

        timerBar.fillAmount = targetFill;
    }

    public IEnumerator TimerCoroutine()
    {
        while (timer < timeLimit)
        {
            timerBar.fillAmount = (float)(timeLimit - timer) / timeLimit;
            yield return new WaitForSeconds(1f);
            timer++;
        }
        gridManager = GameObject.FindObjectOfType<GridManager>();
        gridManager.SetCellOccupied(OccupiedGridPositions, false);
        healthBarObject.SetActive(false);
        timerBarObject.SetActive(false);

        //Time's up, handle boss failure in wave manager
        BossManager.Instance.OnBossFailed.RaiseEvent(WaveManager.Instance.currentWaveData.waveNumber - 2);
        Failedboss();
    }
    
    public override void OnClick()
    {
        if (!isClickable)
        {
            Debug.Log("Boss is not clickable yet.");
            return;
        }

        if (CurrentHealth > 0)
        {
            Debug.Log("Damaging boss");
            // Debug.Log("Damaging boss");
            CurrentHealth -= CalculateClickDamage();
            healthBar.fillAmount = (float) CurrentHealth / MaxHealth;
            healthText.text = CurrentHealth.ToString() + " / " + MaxHealth.ToString();
        }
        
        if (CurrentHealth <= 0)
        {
            // Debug.Log("Boss already at 0 health");
            HandleDestroy();
        }
    }

    public int CalculateClickDamage()
    {
        int baseDamage = PlayerDataManager.Instance.currentDamagePerClick;
        // See if it crits or not
        float critChance = PlayerDataManager.Instance.currentCritChance;

        // Random from 0 to 1
        float roll = Random.Range(0f, 1f);
        if (roll <= critChance)
        {
            // Debug.Log("Critical Hit!");
            float critDmgMultiplier = PlayerDataManager.Instance.currentCritDamageMultiplier;
            int critDamage =  baseDamage + (int) (baseDamage *  critDmgMultiplier);
            return critDamage;
        }
        // Debug.Log("Normal Hit");
        return baseDamage;
    }

    public override void OnHover()
    {
        // Change sprite to a hovered one
    }

    public override void OnUnhover()
    {
        // Change back to default sprite
    }

    public override void HandleDestroy()
    {
        if (isClickable)
        {
            if (timerCoroutine != null)
            {
                StopCoroutine(timerCoroutine);
            }
            gridManager = GameObject.FindObjectOfType<GridManager>();
            gridManager.SetCellOccupied(OccupiedGridPositions, false);
            int totalDreamEssenceDrop = PlayerDataManager.Instance.currentDreamEssenceDropIncrease + DreamEssenceDrop;
            PlayerDataManager.Instance.AddDreamEssence(totalDreamEssenceDrop);
            PlayerDataManager.Instance.AddSoulEssence(SoulEssenceDrop);
            PlayerDataManager.Instance.AddHumanSoul(HumanSoulDrop);

            healthBarObject.SetActive(false);
            timerBarObject.SetActive(false);
            
            // Show Dialogue on top here
            Destroy(gameObject);
            BossManager.Instance.OnBossCompleted.RaiseEvent(WaveManager.Instance.currentWaveData.waveNumber);
            IslandManager.Instance.DisplayDialogue(dialogue, dialogueText, dialogueObject);
            // Can add effects here too
        }
    }

    public void Failedboss()
    {
        if (isClickable)
        {
            if (timerCoroutine != null)
            {
                StopCoroutine(timerCoroutine);
            }
            gridManager = GameObject.FindObjectOfType<GridManager>();
            gridManager.SetCellOccupied(OccupiedGridPositions, false);
            healthBarObject.SetActive(false);
            timerBarObject.SetActive(false);
            AudioManager.Instance.PlaySFXOnce(2);
            Destroy(gameObject);
        }
    }
}
