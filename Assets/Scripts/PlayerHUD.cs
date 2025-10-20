using UnityEngine;
using TMPro;

public class PlayerHUD : MonoBehaviour
{
    public TextMeshProUGUI soulEssencesText;
    public TextMeshProUGUI waveText;
    
    void Start()
    {
        soulEssencesText.text = PlayerData.instance.SoulEssences.ToString();
        waveText.text = WaveManager.instance.CurrentWave.ToString() + " / " + WaveManager.WAVESPERISLAND.ToString();
    }
}