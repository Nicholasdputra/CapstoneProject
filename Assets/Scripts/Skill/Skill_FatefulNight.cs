using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_FatefulNight : BaseSkill
{
    void Start()
    {
        InitializeButton();
        InitializeSkill();
    }

    void InitializeSkill()
    {
        skillName = "Fateful Night";
        skillID = "Skill_FatefulNight";

        dreamEssenceUnlockCost = 400;

        soulEssenceCost = 40;
        
        baseDuration = 10f;

        skillDescription = "Increase critical hit chance by 20% for " + baseDuration + " seconds.";

        baseMultiplier = 0f;
        skillEffectMultiplier = 0.2f;
    }
}
