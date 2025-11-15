using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_DustOff : BaseSkill
{
    void Start()
    {
        InitializeSkill();
    }

    void InitializeSkill()
    {
        skillName = "Dust Off";
        skillID = "Skill_DustOff";

        dreamEssenceUnlockCost = 500;

        soulEssenceCost = 50;
        
        baseDuration = 10f;
        skillDescription = "Double the damage dealt per click for " + baseDuration + " seconds.";

        baseMultiplier = 1f;
        skillEffectMultiplier = 2f;
    }
}
