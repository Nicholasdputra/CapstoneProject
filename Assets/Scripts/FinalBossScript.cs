using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class FinalBossScript : BossTypeEntity, IClickable
{
    public int soulEssenceGain;
    public int dreamEssenceGain;
    public int humanSoulGain;
        
    void Start()
    {
        Initialize();
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
            Debug.Log("Damaging Boss");
            CurrentHealth -= PlayerData.Instance.damageToBossPerClick;
            healthBar.fillAmount = (float) CurrentHealth / MaxHealth;
            healthText.text = CurrentHealth.ToString() + " / " + MaxHealth.ToString();
        }
        else
        {
            Debug.Log("Boss already at 0 health");
        }
    }

    public void OnHover()
    {
        
    }

    public void OnUnhover()
    {
        
    }
    
    public override void HandleDestroy()
    {
        if (timerCoroutine != null)
        {
            StopCoroutine(timerCoroutine);
        }
        GridManager.Instance.SetCellOccupied(myGridCell, false);
        PlayerData.Instance.AddDreamEssences(dreamEssenceGain);
        PlayerData.Instance.AddSoulEssences(soulEssenceGain);
        PlayerData.Instance.AddHumanSouls(humanSoulGain);
        IslandManager.Instance.WinAgainstBoss();
        Destroy(gameObject);
    }
}
