using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class PlayerHUD : MonoBehaviour
{
    public TextMeshProUGUI dreamEssencesText;
    public TextMeshProUGUI soulEssencesText;
    public TextMeshProUGUI humanSoulText;

    public IntEventChannel DreamEssenceChangedEvent;
    public IntEventChannel SoulEssenceChangedEvent;
    public IntEventChannel HumanSoulChangedEvent;    
    public IntEventChannel WaveChangedEvent;

    public TextMeshProUGUI waveText;
    public Button continueToBossButton;
    public Button refreshIslandButton;

    public static PlayerHUD Instance;

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
        DreamEssenceChangedEvent.OnEventRaised += UpdateDreamEssence;
        SoulEssenceChangedEvent.OnEventRaised += UpdateSoulEssence;
        HumanSoulChangedEvent.OnEventRaised += UpdateHumanSoul;
        WaveChangedEvent.OnEventRaised += UpdateWave;
    }

    private void OnDisable()
    {
        DreamEssenceChangedEvent.OnEventRaised -= UpdateDreamEssence;
        SoulEssenceChangedEvent.OnEventRaised -= UpdateSoulEssence;
        HumanSoulChangedEvent.OnEventRaised -= UpdateHumanSoul;
        WaveChangedEvent.OnEventRaised -= UpdateWave;
    }

    void UpdateDreamEssence(int value)
    {
        dreamEssencesText.text = value.ToString();
    }

    void UpdateSoulEssence(int value)
    {                
        soulEssencesText.text = value.ToString();
    }

    void UpdateHumanSoul(int value)
    {
        humanSoulText.text = value.ToString();
    }
    
    void UpdateWave(int value)
    {
        if (WaveManager.Instance.currentWaveData == null)
        {
            waveText.text = "Wave 0 / " + WaveManager.MAXWAVESPERISLAND.ToString();
            return;
        } 
        waveText.text = "Wave " + WaveManager.Instance.currentWaveData.waveNumber.ToString() + " / " + WaveManager.MAXWAVESPERISLAND.ToString();
    }

    public void CallShowDamageNumber(Vector3 position, int damageAmount)
    {
        StartCoroutine(ShowDamageNumber(position, damageAmount));
    }

    public IEnumerator ShowDamageNumber(Vector3 position, int damageAmount)
    {
        GameObject damageTextObj = new GameObject("DamageText");
        damageTextObj.transform.SetParent(this.transform);

        TextMeshProUGUI damageText = damageTextObj.AddComponent<TextMeshProUGUI>();
        damageText.fontSize = 36;
        damageText.color = Color.black;
        damageText.alignment = TextAlignmentOptions.Center;
        damageText.text = "-" + damageAmount.ToString();

        Vector3 screenPosition = Camera.main.WorldToScreenPoint(position);
        damageTextObj.transform.position = screenPosition;

        float duration = 1.0f;
        float elapsed = 0f;
        Vector3 startPos = damageTextObj.transform.position;
        Vector3 endPos = startPos + new Vector3(0, 50, 0);

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            damageTextObj.transform.position = Vector3.Lerp(startPos, endPos, t);
            damageText.alpha = 1.0f - t;
            yield return null;
        }

        Destroy(damageTextObj);
    }

    public IEnumerator ShowInsufficientSoulEssenceToSpend(int requiredAmount, int currentAmount)
    {
        GameObject warningTextObj = new GameObject("InsufficientSoulEssenceText");
        warningTextObj.transform.SetParent(this.transform);

        TextMeshProUGUI warningText = warningTextObj.AddComponent<TextMeshProUGUI>();
        warningText.fontSize = 28;
        warningText.color = Color.red;
        warningText.alignment = TextAlignmentOptions.Center;
        warningText.text = $"Insufficient Soul Essence! Need {requiredAmount}, have {currentAmount}.";

        warningTextObj.transform.position = new Vector3(Screen.width / 2, Screen.height / 2, 0);

        float duration = 2.0f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            yield return null;
        }

        Destroy(warningTextObj);
    }

}