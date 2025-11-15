using UnityEngine;

[CreateAssetMenu(fileName = "New Boss", menuName = "ScriptableObjects/BossSO")]
public class BossSO : ScriptableObject
{
    public string bossName;
    public string dialogue;
    public int health;
    public int dreamEssenceDrop;
    public int soulEssenceDrop;
    public int humanSoulDrop;
    public Sprite bossSprite;
}