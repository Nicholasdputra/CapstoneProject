using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_HugeBlow : BaseSkill
{
    void Start()
    {
        InitializeSkill();
    }

    void InitializeSkill()
    {
        skillName = "Huge Blow";
        skillID = "Skill_HugeBlow";

        dreamEssenceUnlockCost = 350;

        soulEssenceCost = 100;
        
        baseDuration = 10f;

        skillDescription = "Increase critical damage dealt per click by 50% for " + baseDuration + " seconds.";
        baseMultiplier = 0f;
        skillEffectMultiplier = 0.5f;
    }
}
