using UnityEngine;

public class BoatToy : BaseObject
{
    void Start()
    {
        objectType = ObjectType.BoatToy;
        Initialize();
    }

    public override void Initialize()
    {
        Debug.Log("Initializing BoatToy");
        MaxHealth = 20;
        DreamEssenceDrop = 45;
        SoulEssenceDrop = 0;
        HumanSoulDrop = 0;
        base.Initialize();
    }
}
