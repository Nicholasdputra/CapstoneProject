using UnityEngine;

public abstract class ClickableEntity : MonoBehaviour
{
    [SerializeField] private int currentHealth;
    [SerializeField]
    public int CurrentHealth
    {
        get => currentHealth;
        set => currentHealth = Mathf.Max(0, value);
    }
    
    [SerializeField] private int maxHealth;
    [SerializeField]
    public int MaxHealth
    {
        get => maxHealth;
        set => maxHealth = value;
    }
    
    public Vector2Int myGridCell;
    [SerializeField] public bool isClickable { get; set; }
    public abstract void Initialize();
    public abstract void OnClick();
    public abstract void HandleDestroy();
}