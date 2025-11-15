using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_SurfacePressure : BaseSkill
{
    void Start()
    {
        InitializeSkill();
    }

    void InitializeSkill()
    {
        skillName = "Surface Pressure";
        skillID = "Skill_SurfacePressure";

        dreamEssenceUnlockCost = 550;

        soulEssenceCost = 150;
        
        baseDuration = 15f;

        skillDescription = "Increase auto damage by 50% for " + baseDuration + " seconds.";
        baseMultiplier = 1f;
        skillEffectMultiplier = 1.5f;
    }
}
