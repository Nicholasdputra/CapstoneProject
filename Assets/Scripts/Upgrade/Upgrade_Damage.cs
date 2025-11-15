using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Upgrade_Damage : BaseUpgrade
{
    public int damageIncrease;

    public void Start()
    {
        DecideEffect();
    }

    public override void DecideEffect()
    {
        PlayerDataManager pdm = PlayerDataManager.Instance;
        switch (upgradeTier)
        {
            case 0:
                upgradeName = "Damage I";
                damageIncrease = 0;
                dreamEssenceCostToNextTier = 5;
                break;
            case 1:
                upgradeName = "Damage I";
                damageIncrease = 2;
                dreamEssenceCostToNextTier = 6;
                break;
            case 2:
                upgradeName = "Damage I";
                damageIncrease = 4;
                dreamEssenceCostToNextTier = 9;
                break;
            case 3:
                upgradeName = "Damage I";
                damageIncrease = 6;
                dreamEssenceCostToNextTier = 15;
                break;
            case 4:
                upgradeName = "Damage I";
                damageIncrease = 8;
                dreamEssenceCostToNextTier = 31;
                break;
            case 5:
                upgradeName = "Damage I";
                damageIncrease = 10;
                dreamEssenceCostToNextTier = 35;
                break;
            case 6:
                upgradeName = "Damage II";
                damageIncrease = 15;
                dreamEssenceCostToNextTier = 42;
                break;
            case 7:
                upgradeName = "Damage II";
                damageIncrease = 20;
                dreamEssenceCostToNextTier = 60;
                break;
            case 8:
                upgradeName = "Damage II";
                damageIncrease = 25;
                dreamEssenceCostToNextTier = 105;
                break;
            case 9:
                upgradeName = "Damage II";
                damageIncrease = 30;
                dreamEssenceCostToNextTier = 217;
                break;
            case 10:
                upgradeName = "Damage II";
                damageIncrease = 35;
                dreamEssenceCostToNextTier = 220;
                break;
            case 11:
                upgradeName = "Damage III";
                damageIncrease = 43;
                dreamEssenceCostToNextTier = 264;
                break;
            case 12:
                upgradeName = "Damage III";
                damageIncrease = 51;
                dreamEssenceCostToNextTier = 380;
                break;
            case 13:
                upgradeName = "Damage III";
                damageIncrease = 59;
                dreamEssenceCostToNextTier = 657;
                break;
            case 14:
                upgradeName = "Damage III";
                damageIncrease = 67;
                dreamEssenceCostToNextTier = 1362;
                break;
            case 15:
                upgradeName = "Damage III";
                damageIncrease = 75;
                dreamEssenceCostToNextTier = -1;
                break;
            default:
                upgradeName = "Damage III";
                damageIncrease = 75;
                dreamEssenceCostToNextTier = -1;
                break;
        }
        pdm.BaseDamagePerClick = 1 + damageIncrease;
    }
}