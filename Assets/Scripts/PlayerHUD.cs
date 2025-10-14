using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerHUD : MonoBehaviour
{
    public TextMeshProUGUI soulEssencesText;
    
    void Start()
    {
        soulEssencesText.text = "Soul Essences: " + PlayerData.instance.SoulEssences.ToString();
    }
}