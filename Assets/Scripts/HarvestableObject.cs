using UnityEngine;

public class HarvestableObject : ClickableEntity, IClickable
{
    bool isCorrupted;
    int clicksToCorrupt;
    int dreamEssenceGain;

    private void Start()
    {
        Initialize();
    }

    public override void Initialize()
    {
        Debug.Log("Initializing Harvestable Object");
        isClickable = true;
        isCorrupted = false;
        CurrentHealth = Random.Range(4,6);
        clicksToCorrupt = 1;
        dreamEssenceGain = 1;
        MaxHealth = CurrentHealth;
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
            Debug.Log("Harvestable Object is not clickable right now.");
            return;
        }

        if (isCorrupted)
        {
            CurrentHealth -= PlayerData.instance.damageToHarvestablesPerClick;
            Debug.Log("Clicked! Current Health: " + CurrentHealth);    
        }
        else
        {
            clicksToCorrupt--;
            Debug.Log("Corrupting... Clicks left to corrupt: " + clicksToCorrupt);
            if (clicksToCorrupt <= 0)
            {
                isCorrupted = true;
                Debug.Log("Object Corrupted! You can now harvest it.");
            }
        }
    }

    public void OnHover()
    {
        // Change sprite to a hovered one
        Debug.Log("Hovering over Harvestable Object");

    }

    public void OnUnhover()
    {
        // Change back to default sprite
        Debug.Log("Stopped hovering over Harvestable Object");
    }
    
    public override void HandleDestroy()
    {
        GridManager.instance.SetCellOccupied(myGridCell, false);
        PlayerData.instance.AddDreamEssences(dreamEssenceGain);
        WaveManager.instance.OnHarvestableDestroyed();
        Destroy(gameObject);
        // Can add effects here too
    }
}