using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Upgrade_SleepTight : BaseUpgrade
{
    public int dreamEssenceDropIncrease;

    public void Start()
    {
        upgradeID = "Upgrade_SleepTight";
        DecideEffect();
        maxUpgradeTier = 9;
    }

    public override void DecideEffect()
    {
        PlayerDataManager pdm = PlayerDataManager.Instance;
        switch (upgradeTier)
        {
            case 0:
                upgradeName = "Sleep Tight I";
                dreamEssenceDropIncrease = 2;
                dreamEssenceCostToNextTier = 25;
                break;
            case 1:
                upgradeName = "Sleep Tight I";
                dreamEssenceDropIncrease = 4;
                dreamEssenceCostToNextTier = 28;
                break;
            case 2:
                upgradeName = "Sleep Tight I";
                dreamEssenceDropIncrease = 6;
                dreamEssenceCostToNextTier = 33;
                break;
            case 3:
                upgradeName = "Sleep Tight I";
                dreamEssenceDropIncrease = 8;
                dreamEssenceCostToNextTier = 44;
                break;
            case 4:
                upgradeName = "Sleep Tight I";
                dreamEssenceDropIncrease = 10;
                dreamEssenceCostToNextTier = 65;
                break;
            case 5:
                upgradeName = "Sleep Tight II";
                dreamEssenceDropIncrease = 13;
                dreamEssenceCostToNextTier = 70;
                break;
            case 6:
                upgradeName = "Sleep Tight II";
                dreamEssenceDropIncrease = 16;
                dreamEssenceCostToNextTier = 77;
                break;
            case 7:
                upgradeName = "Sleep Tight II";
                dreamEssenceDropIncrease = 19;
                dreamEssenceCostToNextTier = 93;
                break;
            case 8:
                upgradeName = "Sleep Tight II";
                dreamEssenceDropIncrease = 22;
                dreamEssenceCostToNextTier = 124;
                break;
            case 9:
                upgradeName = "Sleep Tight II";
                dreamEssenceDropIncrease = 25;
                dreamEssenceCostToNextTier = 165;
                break;
            default:
                upgradeName = "Sleep Tight II";
                dreamEssenceDropIncrease = 25;
                dreamEssenceCostToNextTier = -1;
                break;
        }
        pdm.BaseDreamEssenceDropIncrease = dreamEssenceDropIncrease;
        upgradeDescription = $"Gain an additional {pdm.BaseDreamEssenceDropIncrease} dream essences when cleaning";
    }
}