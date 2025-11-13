using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ClickableEntity : MonoBehaviour, IClickable
{
    public bool isClickable { get; set; }
    [SerializeField] private int xSize;
    public int XSize
    {
        get { return xSize; }
        set { xSize = value; } 
    }
    [SerializeField] private int zSize;
    public int ZSize
    {
        get { return zSize; }
        set { zSize = value; }
    }
    [SerializeField] private int yOffset;
    public int YOffset
    {
        get { return yOffset; }
        set { yOffset = value; }
    }

    [SerializeField] private Vector2Int[] occupiedGridPositions;
    public Vector2Int[] OccupiedGridPositions
    {
        get => occupiedGridPositions;
        set => occupiedGridPositions = value;
    }
    
    # region Health
    // Health properties
    [SerializeField] protected int currentHealth;
    public int CurrentHealth
    {
        get => currentHealth;
        set => currentHealth = Mathf.Clamp(value, 0, MaxHealth);
    }
    [SerializeField] protected int maxHealth;
    public int MaxHealth
    {
        get => maxHealth;
        set => maxHealth = Mathf.Max(1, value);
    }
    #endregion
    # region Resource Gains
    // Resource gains
    protected int dreamEssenceDrop;
    [SerializeField]
    public int DreamEssenceDrop
    { 
        get => dreamEssenceDrop;
        set => dreamEssenceDrop = Mathf.Max(0, value);
    }
    protected int soulEssenceDrop;
    [SerializeField]
    public int SoulEssenceDrop
    {
        get => soulEssenceDrop;
        set => soulEssenceDrop = Mathf.Max(0, value);
    }
    protected int humanSoulDrop;
    [SerializeField]
    public int HumanSoulDrop
    {
        get => humanSoulDrop;
        set => humanSoulDrop = Mathf.Max(0, value);
    }
    #endregion
    
    // Abstract methods to be implemented by derived classes
    public abstract void Initialize();
    public abstract void OnHover();
    public abstract void OnUnhover();
    public abstract void OnClick();
    public abstract void HandleDestroy();
    // public abstract void SetUpOccupiedGridPositions();
}