using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Upgrade_CriticalSweep : BaseUpgrade
{
    public float criticalSweepChanceIncrease;

    public void Start()
    {
        upgradeID = "Upgrade_CriticalSweep";
        DecideEffect();
        maxUpgradeTier = 10;
    }

    public override void DecideEffect()
    {
        PlayerDataManager pdm = PlayerDataManager.Instance;
        switch (upgradeTier)
        {
            case 0:
                upgradeName = "Critical Sweep I";
                criticalSweepChanceIncrease = 0f;
                dreamEssenceCostToNextTier = 25;
                break;
            case 1:
                upgradeName = "Critical Sweep I";
                criticalSweepChanceIncrease = 0.05f;
                dreamEssenceCostToNextTier = 30;
                break;
            case 2:
                upgradeName = "Critical Sweep I";
                criticalSweepChanceIncrease = 0.1f;
                dreamEssenceCostToNextTier = 43;
                break;
            case 3:
                upgradeName = "Critical Sweep I";
                criticalSweepChanceIncrease = 0.15f;
                dreamEssenceCostToNextTier = 75;
                break;
            case 4:
                upgradeName = "Critical Sweep I";
                criticalSweepChanceIncrease = 0.2f;
                dreamEssenceCostToNextTier = 155;
                break;
            case 5:
                upgradeName = "Critical Sweep I";
                criticalSweepChanceIncrease = 0.25f;
                dreamEssenceCostToNextTier = 180;
                break;
            case 6:
                upgradeName = "Critical Sweep II";
                criticalSweepChanceIncrease = 0.32f;
                dreamEssenceCostToNextTier = 216;
                break;
            case 7:
                upgradeName = "Critical Sweep II";
                criticalSweepChanceIncrease = 0.39f;
                dreamEssenceCostToNextTier = 311;
                break;
            case 8:
                upgradeName = "Critical Sweep II";
                criticalSweepChanceIncrease = 0.46f;
                dreamEssenceCostToNextTier = 537;
                break;
            case 9:
                upgradeName = "Critical Sweep II";
                criticalSweepChanceIncrease = 0.53f;
                dreamEssenceCostToNextTier = 1115;
                break;
            case 10:
                upgradeName = "Critical Sweep II";
                criticalSweepChanceIncrease = 0.6f;
                dreamEssenceCostToNextTier = -1;
                break;
            default:
                upgradeName = "Critical Sweep II";
                criticalSweepChanceIncrease = 0.6f;
                dreamEssenceCostToNextTier = -1;
                break;
        }
        pdm.BaseCritDamage = criticalSweepChanceIncrease;
        upgradeDescription = $"Set click critical damage to {(1+criticalSweepChanceIncrease)*100}%";
    }
}
