using UnityEngine;

public class HarvestableObject : ClickableEntity, IClickable
{
    bool isCorrupted;
    int clicksToCorrupt;
    int soulEssenceGain;

    private void Start()
    {
        Initialize();
    }

    public override void Initialize()
    {
        Health = Random.Range(4,6);
        clicksToCorrupt = 1;
    }

    private void Update()
    {
        if (Health <= 0)
        {
            HandleDestroy();
        }
    }

    public override void OnClick()
    {
        if (isCorrupted)
        {
            Health -= PlayerData.instance.damageToHarvestablesPerClick;
            Debug.Log("Clicked! Current Health: " + Health);    
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
        soulEssenceGain++;
        PlayerData.instance.AddSoulEssences(1);
        WaveManager.instance.OnHarvestableDestroyed();
        Destroy(gameObject);
        // Can add effects here too
    }
}