using UnityEngine;
using TMPro;

public class PlayerHUD : MonoBehaviour
{
    public TextMeshProUGUI soulEssencesText;
    public TextMeshProUGUI waveText;
    
    void Start()
    {
        soulEssencesText.text = "Soul Essences: " + PlayerData.instance.SoulEssences.ToString();
        waveText.text = "Wave: " + WaveManager.instance.CurrentWave.ToString();
    }
}