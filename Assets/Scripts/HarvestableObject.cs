using UnityEngine;

public class HarvestableObject : MonoBehaviour, IClickable
{
    [SerializeField] private int health;
    public int Health
    {
        get => health;
        set => health = Mathf.Max(0, value);
    }

    public Vector2Int myGridCell;

    private void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        Health = health;
    }

    private void Update()
    {
        if (Health <= 0)
        {
            HandleDestroy();
        }
    }

    public void OnClick()
    {
        Health -= PlayerData.instance.damageToHarvestablesPerClick;
        Debug.Log("Harvested! Current Health: " + Health);
    }

    public void OnHover()
    {
        // Change sprite to a hovered one

    }

    public void OnUnhover()
    {
        // Change back to default sprite

    }
    
    public void HandleDestroy()
    {
        GridManager.instance.SetCellOccupied(myGridCell, false);
        WaveManager.instance.OnHarvestableDestroyed();
        Destroy(gameObject);
        // Can add effects here too
    }
}