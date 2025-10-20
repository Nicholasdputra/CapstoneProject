using UnityEngine;

public abstract class ClickableEntity : MonoBehaviour
{
    private int health;
    public int Health 
    {
        get => health;
        set => health = Mathf.Max(0, value);
    }
    public Vector2Int myGridCell;
    public abstract void Initialize();
    public abstract void OnClick();
    public abstract void HandleDestroy();
}