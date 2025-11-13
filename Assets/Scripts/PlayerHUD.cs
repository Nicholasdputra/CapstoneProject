using UnityEngine;
using TMPro;
using UnityEngine.UI;

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
}