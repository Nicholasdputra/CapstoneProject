using UnityEngine;

[CreateAssetMenu(fileName = "New Miniboss", menuName = "ScriptableObjects/MinibossSO")]
public class MinibossSO : ScriptableObject
{
    public string minibossName;
    public string dialogue;
    public int health;
    public int dreamEssenceDrop = 0;
    public int soulEssenceDrop;
    public int humanSoulDrop = 0;
    public Sprite minibossSprite;
}