using UnityEngine;

public class ForkToy : BaseObject
{
    void Start()
    {
        objectType = ObjectType.ForkToy;
        Initialize();
    }

    public override void Initialize()
    {
        // Debug.Log("Initializing ForkToy");
        MaxHealth = 5;
        DreamEssenceDrop = 5;
        SoulEssenceDrop = 0;
        HumanSoulDrop = 0;
        base.Initialize();
    }
}