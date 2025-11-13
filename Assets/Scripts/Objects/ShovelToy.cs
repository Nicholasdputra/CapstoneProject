using UnityEngine;

public class ShovelToy : BaseObject
{
    void Start()
    {
        objectType = ObjectType.ShovelToy;
        Initialize();
    }

    public override void Initialize()
    {
        // Debug.Log("Initializing ShovelToy");
        MaxHealth = 10;
        DreamEssenceDrop = 10;
        SoulEssenceDrop = 0;
        HumanSoulDrop = 0;
        base.Initialize();
    }
}
