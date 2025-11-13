using UnityEngine;

public class SandCastle : BaseObject
{
    void Start()
    {
        objectType = ObjectType.SandCastle;
        Initialize();
    }

    public override void Initialize()
    {
        Debug.Log("Initializing SandCastle");
        MaxHealth = 30;
        DreamEssenceDrop = 50;
        SoulEssenceDrop = 0;
        HumanSoulDrop = 0;
        base.Initialize();
    }
}
