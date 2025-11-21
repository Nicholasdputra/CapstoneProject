using UnityEngine;

public class Knife : BaseObject
{
    void Start()
    {
        objectType = ObjectType.Knife;
        Initialize();
    }

    public override void Initialize()
    {
        Debug.Log("Initializing Knife");
        MaxHealth = 100;
        DreamEssenceDrop = 15;
        SoulEssenceDrop = 0;
        HumanSoulDrop = 0;
        base.Initialize();
    }
}