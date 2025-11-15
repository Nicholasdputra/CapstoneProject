using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_HelpingHand : BaseSkill
{
    void Start()
    {
        InitializeButton();
        InitializeSkill();
    }

    void InitializeSkill()
    {
        skillName = "Helping Hand";
        skillID = "Skill_HelpingHand";

        dreamEssenceUnlockCost = 1000;

        soulEssenceCost = 70;
        
        baseDuration = 15f;

        skillDescription = "Double auto cleaning speed for " + baseDuration + " seconds.";
        baseMultiplier = 1f;
        skillEffectMultiplier = 2f;
    }
}
