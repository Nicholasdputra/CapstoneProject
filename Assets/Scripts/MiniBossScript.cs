using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MiniBossScript : BossTypeEntity, IClickable
{
    public int soulEssenceGain;
    public int dreamEssenceGain;

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
            // Debug.Log("Damaging Miniboss");
            CurrentHealth -= PlayerData.Instance.damageToBossPerClick;
            healthBar.fillAmount = (float) CurrentHealth / MaxHealth;
            healthText.text = CurrentHealth.ToString() + " / " + MaxHealth.ToString();
        }
        else
        {
            // Debug.Log("Miniboss already at 0 health");
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
        GridManager.Instance.SetCellOccupied(myGridCell, false);
        healthBarObject.SetActive(false);
        timerBarObject.SetActive(false);
        PlayerData.Instance.AddDreamEssences(dreamEssenceGain);
        PlayerData.Instance.AddSoulEssences(soulEssenceGain);
        Destroy(gameObject);
        WaveManager.Instance.WinAgainstMiniboss();
    }
}
