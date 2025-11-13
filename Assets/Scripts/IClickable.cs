using UnityEngine;

public interface IClickable
{
    Vector2Int[] OccupiedGridPositions { get; set; }

    bool isClickable{ get; set; }
    int CurrentHealth { get; set; }
    int MaxHealth { get; set; }

    int DreamEssenceDrop { get; set; }
    int SoulEssenceDrop { get; set; }
    int HumanSoulDrop { get; set; }

    void OnHover();
    void OnUnhover();
    void OnClick();

    void HandleDestroy();
}