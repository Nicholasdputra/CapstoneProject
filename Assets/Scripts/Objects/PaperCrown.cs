using UnityEngine;

public class PaperCrown : BaseObject
{
    void Start()
    {
        objectType = ObjectType.PaperCrown;
        Initialize();
    }

    public override void Initialize()
    {
        Debug.Log("Initializing PaperCrown");
        MaxHealth = 60;
        DreamEssenceDrop = 50;
        SoulEssenceDrop = 0;
        HumanSoulDrop = 0;
        base.Initialize();
    }
}
