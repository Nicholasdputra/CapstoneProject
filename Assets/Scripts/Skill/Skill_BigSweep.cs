using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_BigSweep : BaseSkill
{
    void Start()
    {
        InitializeButton();
        InitializeSkill();
    }

    void InitializeSkill()
    {
        skillName = "Big Sweep";
        skillID = "Skill_BigSweep";

        dreamEssenceUnlockCost = 150;

        soulEssenceCost = 10;
        
        baseDuration = 5f;
        skillDescription = "Increases the range of your harvest area per click by 50% for a short duration.";

        baseMultiplier = 1f;
        skillEffectMultiplier = 1.5f;
    }
}
