using UnityEngine;
using TMPro;
using System.ComponentModel;

public class PlayerHUD : MonoBehaviour
{
    public TextMeshProUGUI dreamEssencesText;
    public TextMeshProUGUI soulEssencesText;
    public TextMeshProUGUI humanSoulText;
    public TextMeshProUGUI waveText;
    // public Button ;

    void Start()
    {
        dreamEssencesText.text = PlayerData.Instance.DreamEssences.ToString();
        soulEssencesText.text = PlayerData.Instance.SoulEssences.ToString();
        humanSoulText.text = PlayerData.Instance.HumanSouls.ToString();
        waveText.text = (WaveManager.Instance.CurrentWave + 1).ToString() + " / " + (WaveManager.MAXWAVEINDEX + 1).ToString();
    }

    public void UpdateCurrentWaveText()
    {
        waveText.text = (WaveManager.Instance.CurrentWave + 1).ToString() + " / " + (WaveManager.MAXWAVEINDEX + 1).ToString();
    }
}