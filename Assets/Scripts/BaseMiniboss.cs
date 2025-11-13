using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BaseMiniboss : ClickableEntity
{

    protected Canvas objectsCanvas;
    public string dialogue;

    public MinibossSO minibossData;

    [Header("Health Bar Elements")]
    public string miniBossName;
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

    void Start()
    {
        Initialize();
    }

    public override void Initialize()
    {
        Debug.Log("Initializing Boss Type Entity: " + miniBossName + " with Health: " + CurrentHealth);
        
        // Base Stats
        MaxHealth = minibossData.health;
        CurrentHealth = MaxHealth;
        DreamEssenceDrop = minibossData.dreamEssenceDrop;
        SoulEssenceDrop = minibossData.soulEssenceDrop;
        HumanSoulDrop = minibossData.humanSoulDrop;
        miniBossName = minibossData.minibossName;
        dialogue = minibossData.dialogue;
        
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

            if (child.name == "BossName")
            {
                bossNameText = child.GetComponent<TextMeshProUGUI>();
                bossNameText.text = miniBossName;
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
        }
        StartCoroutine(FillUpHealthBar());
        StartCoroutine(FillUpTimerBar());
    }
    public IEnumerator FillUpHealthBar()
    {
        float fillSpeed = 0.5f; // Adjust this value to change the speed of the fill
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

        GridManager.Instance.SetCellOccupied(OccupiedGridPositions, false);
        healthBarObject.SetActive(false);
        timerBarObject.SetActive(false);

        //Time's up, handle miniboss failure in wave manager
        
    }
    
    public override void OnClick()
    {
        if (!isClickable)
        {
            return;
        }
        CurrentHealth -= PlayerDataManager.Instance.DamagePerClick;
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
        GridManager.Instance.SetCellOccupied(OccupiedGridPositions, false);
        PlayerDataManager.Instance.AddDreamEssence(DreamEssenceDrop);
        PlayerDataManager.Instance.AddSoulEssence(SoulEssenceDrop);
        PlayerDataManager.Instance.AddHumanSoul(HumanSoulDrop);

        // Show Dialogue on top here


        Destroy(gameObject);

        MinibossManager.Instance.OnMinibossCompleted.RaiseEvent(WaveManager.Instance.currentWaveData.waveNumber);
        
        // Can add effects here too
        
    }
}