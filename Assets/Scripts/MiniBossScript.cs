using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MiniBossScript : ClickableEntity, IClickable
{
    private Canvas objectsCanvas;
    public string miniBossName;
    [SerializeField] private GameObject healthBarObject;
    [SerializeField] private TextMeshProUGUI miniBossNameText;
    [SerializeField] private Image healthBar;
    [SerializeField] private TextMeshProUGUI healthText;

    [SerializeField] private GameObject timerBarObject;
    [SerializeField] private Image timerBar;

    public int timer;
    public int timeLimit;
    public Coroutine timerCoroutine;

    public int soulEssenceGain;
    public int dreamEssenceGain;

    void Start()
    {
        Initialize();
    }

    public override void Initialize()
    {
        MaxHealth = CurrentHealth;
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

            if (child.name == "MiniBossName")
            {
                miniBossNameText = child.GetComponent<TextMeshProUGUI>();
                miniBossNameText.text = miniBossName;
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
            Debug.Log("timerBar Fill Amount: " + timerBar.fillAmount);
            yield return new WaitForSeconds(1f);
            timer++;
        }

        //Time's up, handle miniboss failure
        WaveManager.instance.LoseAgainstMiniboss();
    }

    void Update()
    {
        if (CurrentHealth <= 0)
        {
            HandleDestroy();
        }
    }


    public override void OnClick()
    {
        // Debug.Log("MiniBoss Clicked");
        if (!isClickable)
        {
            // Debug.Log("Miniboss is not clickable right now.");
            return;
        }
        else
        {
            // Debug.Log("Miniboss is clickable.");
        }

        if (CurrentHealth > 0)
        {
            Debug.Log("Damaging Miniboss");
            CurrentHealth -= PlayerData.instance.damageToMinibossPerClick;
            healthBar.fillAmount = (float) CurrentHealth / MaxHealth;
            healthText.text = CurrentHealth.ToString() + " / " + MaxHealth.ToString();
        }
        else
        {
            Debug.Log("Miniboss already at 0 health");
        }
    }

    public void OnHover()
    {
        // Change sprite to a hovered one
        // Debug.Log("Hovering over MiniBoss Object");
    }

    public void OnUnhover()
    {
        // Change back to default sprite
        // Debug.Log("Stopped hovering over MiniBoss Object");
    }

    public override void HandleDestroy()
    {
        if (timerCoroutine != null)
        {
            StopCoroutine(timerCoroutine);
        }
        GridManager.instance.SetCellOccupied(myGridCell, false);
        healthBarObject.SetActive(false);
        timerBarObject.SetActive(false);
        WaveManager.instance.WinAgainstMiniboss();
        PlayerData.instance.AddDreamEssences(dreamEssenceGain);
        PlayerData.instance.AddSoulEssences(soulEssenceGain);
        Destroy(gameObject);
    }
}
