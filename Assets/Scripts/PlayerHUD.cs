using UnityEngine;
using TMPro;

public class PlayerHUD : MonoBehaviour
{
    public TextMeshProUGUI dreamEssencesText;
    public TextMeshProUGUI soulEssencesText;
    public TextMeshProUGUI humanSoulText;
    public TextMeshProUGUI waveText;
    
    void Start()
    {
        soulEssencesText.text = PlayerData.instance.SoulEssences.ToString();
        dreamEssencesText.text = PlayerData.instance.DreamEssences.ToString();
        humanSoulText.text = PlayerData.instance.HumanSouls.ToString();
        waveText.text = WaveManager.instance.CurrentWave+1.ToString() + " / " + WaveManager.WAVESPERISLAND.ToString();
    }
}