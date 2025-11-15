using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Upgrade_CleaningArea : BaseUpgrade
{
    public int harvestAreaIncrease;

    public void Start()
    {
        upgradeID = "Upgrade_CleaningArea";
        DecideEffect();
        maxUpgradeTier = 5;
    }

    public override void DecideEffect()
    {
        PlayerDataManager pdm = PlayerDataManager.Instance;
        switch (upgradeTier)
        {
            case 0:
                upgradeName = "Cleaning Area I";
                harvestAreaIncrease = 0;
                dreamEssenceCostToNextTier = 10;
                break;
            case 1:
                upgradeName = "Cleaning Area I";
                harvestAreaIncrease = 1;
                
                dreamEssenceCostToNextTier = 12;
                break;
            case 2:
                upgradeName = "Cleaning Area I";
                harvestAreaIncrease = 2;
                dreamEssenceCostToNextTier = 17;
                break;
            case 3:
                upgradeName = "Cleaning Area I";
                harvestAreaIncrease = 3;
                dreamEssenceCostToNextTier = 30;
                break;
            case 4:
                upgradeName = "Cleaning Area I";
                harvestAreaIncrease = 4;
                dreamEssenceCostToNextTier = 62;
                break;
            case 5:
                upgradeName = "Cleaning Area I";
                harvestAreaIncrease = 5;
                dreamEssenceCostToNextTier = -1;
                break;
            default:
                upgradeName = "Cleaning Area I";
                harvestAreaIncrease = 5;
                dreamEssenceCostToNextTier = -1;
                break;
        }
        pdm.currentHarvestRadius = harvestAreaIncrease;

        if (upgradeTier <= 5)
        {
            upgradeDescription = $"Increase the cleaning area by {harvestAreaIncrease} unit(s).";
        }
    }
}
