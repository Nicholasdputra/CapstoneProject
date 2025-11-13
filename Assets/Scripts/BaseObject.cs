using UnityEngine;
using System.Collections.Generic;

public abstract class BaseObject : ClickableEntity
{
    public ObjectType objectType;
    protected PlayerSave currentPlayerSave;

    public override void Initialize()
    {
        isClickable = true;
        CurrentHealth = MaxHealth;
        SetUpOffset();
        currentPlayerSave = PlayerDataManager.Instance.currentPlayerSave;
    }

    private void SetUpOffset()
    {
        Renderer rend = GetComponent<Renderer>();
        if (rend != null)
        {
            if (rend.bounds.size.y > 0f)
            {
                YOffset = Mathf.CeilToInt(rend.bounds.size.y);
            }
            else
            {
                YOffset = 1;
            }
        }
        else
        {
            YOffset = 1;
        }

        // Alter y position based on YOffset
        Vector3 pos = transform.position;
        pos.y += YOffset / 2f;
        transform.position = pos;
    }

    private void Update()
    {
        if (CurrentHealth <= 0)
        {
            HandleDestroy();
        }
    }

    public override void OnClick()
    {
        if (!isClickable)
        {
            return;
        }

        CurrentHealth -= currentPlayerSave.damagePerClick;
    }

    public override void OnHover()
    {
        // Change sprite to a hovered one
        // Debug.Log("Hovering over Harvestable Object");
    }

    public override void OnUnhover()
    {
        // Change back to default sprite
        // Debug.Log("Stopped hovering over Harvestable Object");
    }

    public override void HandleDestroy()
    {
        if (GridManager.Instance != null)
            GridManager.Instance.SetCellOccupied(OccupiedGridPositions, false);
        else
            Debug.LogWarning("GridManager Instance is null when destroying BaseObject.");
        PlayerDataManager.Instance.AddDreamEssence(DreamEssenceDrop);
        // Debug.Log("Added Dream Essence: " + DreamEssenceDrop);
        PlayerDataManager.Instance.AddSoulEssence(SoulEssenceDrop);
        // Debug.Log("Added Soul Essence: " + SoulEssenceDrop);
        PlayerDataManager.Instance.AddHumanSoul(HumanSoulDrop);
        // Debug.Log("Added Human Soul: " + HumanSoulDrop);
        WaveManager.Instance.currentAliveEnemies.Remove(this);
        WaveManager.Instance.CheckIfWaveCompleted();
        Destroy(gameObject);
        // Can add effects here too
    }
}
