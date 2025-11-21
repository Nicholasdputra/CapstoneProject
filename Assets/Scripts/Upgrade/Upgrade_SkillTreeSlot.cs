using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Upgrade_SkillTreeSlot : BaseUpgrade
{
    public int addedSkillTreeSlots;

    public void Start()
    {
        upgradeID = "Upgrade_SkillTreeSlot";
        DecideEffect();
        maxUpgradeTier = 4;
    }

    public override void DecideEffect()
    {
        SkillManager sm = SkillManager.Instance;
        switch (upgradeTier)
        {
            case 0:
                upgradeName = "Skill Tree Slot";
                addedSkillTreeSlots = 1;
                dreamEssenceCostToNextTier = 100;
                break;
            case 1:
                upgradeName = "Skill Tree Slot";
                addedSkillTreeSlots = 2;
                dreamEssenceCostToNextTier = 110;
                break;
            case 2:
                upgradeName = "Skill Tree Slot";
                addedSkillTreeSlots = 3;
                dreamEssenceCostToNextTier = 133;
                break;
            case 3:
                upgradeName = "Skill Tree Slot";
                addedSkillTreeSlots = 4;
                dreamEssenceCostToNextTier = 177;
                break;
            case 4:
                upgradeName = "Skill Tree Slot";
                addedSkillTreeSlots = 5;
                dreamEssenceCostToNextTier = 259;
                break;
            default:
                upgradeName = "Skill Tree Slot";
                addedSkillTreeSlots = 5;
                dreamEssenceCostToNextTier = -1;
                break;
        }
        sm.maxSkillTreeSlots = 2 + addedSkillTreeSlots;
        upgradeDescription = $"Set max skill tree slots to {sm.maxSkillTreeSlots}";
    }
}
