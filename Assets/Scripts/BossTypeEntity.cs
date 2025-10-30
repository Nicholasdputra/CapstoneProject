using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public abstract class BossTypeEntity : ClickableEntity
{
    protected Canvas objectsCanvas;
    public string bossName;
    [SerializeField] protected GameObject healthBarObject;
    [SerializeField] protected TextMeshProUGUI bossNameText;
    [SerializeField] protected Image healthBar;
    [SerializeField] protected TextMeshProUGUI healthText;

    [SerializeField] protected GameObject timerBarObject;
    [SerializeField] protected Image timerBar;

    public int timer;
    public int timeLimit;
    public Coroutine timerCoroutine;

    public override void Initialize()
    {
        MaxHealth = CurrentHealth;
        Debug.Log("Initializing Boss Type Entity: " + bossName + " with Health: " + CurrentHealth);
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
            // Debug.Log("timerBar Fill Amount: " + timerBar.fillAmount);
            yield return new WaitForSeconds(1f);
            timer++;
        }

        GridManager.Instance.SetCellOccupied(myGridCell, false);
        healthBarObject.SetActive(false);
        timerBarObject.SetActive(false);
        
        //Time's up, handle miniboss failure
        WaveManager.Instance.LoseAgainstMiniboss();
    }
}
