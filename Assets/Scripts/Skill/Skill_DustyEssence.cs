using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_DustyEssence : BaseSkill
{
    void Start()
    {
        InitializeButton();
        InitializeSkill();
    }

    void InitializeSkill()
    {
        skillName = "Dusty Essence";
        skillID = "Skill_DustyEssence";
        
        dreamEssenceUnlockCost = 200;
        
        soulEssenceCost = 30;
        
        baseDuration = 10f;
        
        skillDescription = "Gain double the amount of Soul Essence from defeated enemies for " + baseDuration + " seconds.";
        skillEffectMultiplier = 2.0f;
    }
}
