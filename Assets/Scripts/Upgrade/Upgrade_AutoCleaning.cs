using UnityEngine;

public class Upgrade_AutoCleaning : BaseUpgrade
{
    public float autoDamage;

    public void Start()
    {
        upgradeID = "Upgrade_AutoCleaning";
        maxUpgradeTier = 15;
        DecideEffect();
    }

    public override void DecideEffect()
    {
        PlayerDataManager pdm = PlayerDataManager.Instance;
        switch (upgradeTier)
        {
            case 0:
                upgradeName = "Auto Cleaning I";
                autoDamage = 0f;
                pdm.BaseAutoCleaningDamage += (int) autoDamage;
                dreamEssenceCostToNextTier = 15;
                break;
            case 1:
                upgradeName = "Auto Cleaning I";
                autoDamage = 2f;
                dreamEssenceCostToNextTier = 18;
                break;
            case 2:
                upgradeName = "Auto Cleaning I";
                autoDamage = 4f;
                dreamEssenceCostToNextTier = 26;
                break;
            case 3:
                upgradeName = "Auto Cleaning I";
                autoDamage = 6f;
                dreamEssenceCostToNextTier = 30;
                break;
            case 4:
                upgradeName = "Auto Cleaning I";
                autoDamage = 8f;
                dreamEssenceCostToNextTier = 36;
                break;
            case 5:
                upgradeName = "Auto Cleaning I";
                autoDamage = 10f;
                dreamEssenceCostToNextTier = 45;
                break;
            case 6:
                upgradeName = "Auto Cleaning II";
                autoDamage = 13f;
                dreamEssenceCostToNextTier = 52;
                break;
            case 7:
                upgradeName = "Auto Cleaning II";
                autoDamage = 16f;
                dreamEssenceCostToNextTier = 90;
                break;
            case 8:
                upgradeName = "Auto Cleaning II";
                autoDamage = 19f;
                dreamEssenceCostToNextTier = 93;
                break;
            case 9:
                upgradeName = "Auto Cleaning II";
                autoDamage = 22f;
                dreamEssenceCostToNextTier = 186;
                break;
            case 10:
                upgradeName = "Auto Cleaning II";
                autoDamage = 25f;
                dreamEssenceCostToNextTier = 200;
                break;
            case 11:
                upgradeName = "Auto Cleaning III";
                autoDamage = 30f;
                dreamEssenceCostToNextTier = 240;
                break;
            case 12:
                upgradeName = "Auto Cleaning III";
                autoDamage = 35f;
                dreamEssenceCostToNextTier = 346;
                break;
            case 13:
                upgradeName = "Auto Cleaning III";
                autoDamage = 40f;
                dreamEssenceCostToNextTier = 597;
                break;
            case 14:
                upgradeName = "Auto Cleaning III";
                autoDamage = 45f;
                dreamEssenceCostToNextTier = 1238;
                break;
            case 15:
                upgradeName = "Auto Cleaning III";
                autoDamage = 50f;
                dreamEssenceCostToNextTier = -1;
                break;
            default:
                upgradeName = "Auto Cleaning II";
                autoDamage = 50f;
                dreamEssenceCostToNextTier = -1;
                break;
        }
        pdm.BaseAutoCleaningDamage = 1 + (int)autoDamage;

        if (upgradeTier < 6)
        {
            upgradeDescription = "Automatically deal " + autoDamage.ToString("F0") + " damage per second to the mess.";
        }
        else if (upgradeTier < 11)
        {
            upgradeDescription = "Automatically deal " + autoDamage.ToString("F0") + " damage per second to the mess.";
        }
        else
        {
            upgradeDescription = "Automatically deal " + autoDamage.ToString("F0") + " damage per second to the mess.";
        }
    }
}