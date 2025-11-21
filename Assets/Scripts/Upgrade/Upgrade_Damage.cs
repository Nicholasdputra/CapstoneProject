using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Upgrade_Damage : BaseUpgrade
{
    public int damageIncrease;

    public void Start()
    {
        upgradeID = "Upgrade_Damage";
        DecideEffect();
        maxUpgradeTier = 14;
    }

    public override void DecideEffect()
    {
        PlayerDataManager pdm = PlayerDataManager.Instance;
        switch (upgradeTier)
        {
            case 0:
                upgradeName = "Damage I";
                damageIncrease = 2;
                dreamEssenceCostToNextTier = 20;
                break;
            case 1:
                upgradeName = "Damage I";
                damageIncrease = 4;
                dreamEssenceCostToNextTier = 25;
                break;
            case 2:
                upgradeName = "Damage I";
                damageIncrease = 6;
                dreamEssenceCostToNextTier = 45;
                break;
            case 3:
                upgradeName = "Damage I";
                damageIncrease = 8;
                dreamEssenceCostToNextTier = 100;
                break;
            case 4:
                upgradeName = "Damage I";
                damageIncrease = 10;
                dreamEssenceCostToNextTier = 275;
                break;
            case 5:
                upgradeName = "Damage II";
                damageIncrease = 13;
                dreamEssenceCostToNextTier = 400;
                break;
            case 6:
                upgradeName = "Damage II";
                damageIncrease = 16;
                dreamEssenceCostToNextTier = 520;
                break;
            case 7:
                upgradeName = "Damage II";
                damageIncrease = 19;
                dreamEssenceCostToNextTier = 880;
                break;
            case 8:
                upgradeName = "Damage II";
                damageIncrease = 21;
                dreamEssenceCostToNextTier = 1900;
                break;
            case 9:
                upgradeName = "Damage II";
                damageIncrease = 24;
                dreamEssenceCostToNextTier = 5500;
                break;
            case 10:
                upgradeName = "Damage III";
                damageIncrease = 29;
                dreamEssenceCostToNextTier = 8000;
                break;
            case 11:
                upgradeName = "Damage III";
                damageIncrease = 34;
                dreamEssenceCostToNextTier = 10400;
                break;
            case 12:
                upgradeName = "Damage III";
                damageIncrease = 39;
                dreamEssenceCostToNextTier = 17500;
                break;
            case 13:
                upgradeName = "Damage III";
                damageIncrease = 44;
                dreamEssenceCostToNextTier = 38600;
                break;
            case 14:
                upgradeName = "Damage III";
                damageIncrease = 49;
                dreamEssenceCostToNextTier = 110300;
                break;
            default:
                upgradeName = "Damage III";
                damageIncrease = 49;
                dreamEssenceCostToNextTier = -1;
                break;
        }
        pdm.BaseDamagePerClick = 1 + damageIncrease;
        upgradeDescription = $"Set base damage per click to {pdm.BaseDamagePerClick}";
    }
}