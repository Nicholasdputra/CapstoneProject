using System.Collections;

public class Upgrade_CriticalChance : BaseUpgrade
{
    public float criticalChance;

    public void Start()
    {
        upgradeID = "Upgrade_CriticalChance";
        DecideEffect();
        maxUpgradeTier = 9;
    }

    public override void DecideEffect()
    {
        PlayerDataManager pdm = PlayerDataManager.Instance;
        switch (upgradeTier)
        {
            case 0:
                upgradeName = "Critical Chance I";
                criticalChance = 0.04f;
                dreamEssenceCostToNextTier = 20;
                break;
            case 1:
                upgradeName = "Critical Chance I";
                criticalChance = 0.08f;
                dreamEssenceCostToNextTier = 24;
                break;
            case 2:
                upgradeName = "Critical Chance I";
                criticalChance = 0.12f;
                dreamEssenceCostToNextTier = 35;
                break;
            case 3:
                upgradeName = "Critical Chance I";
                criticalChance = 0.16f;
                dreamEssenceCostToNextTier = 60;
                break;
            case 4:
                upgradeName = "Critical Chance I";
                criticalChance = 0.2f;
                dreamEssenceCostToNextTier = 124;
                break;
            case 5:
                upgradeName = "Critical Chance II";
                criticalChance = 0.26f;
                dreamEssenceCostToNextTier = 130;
                break;
            case 6:
                upgradeName = "Critical Chance II";
                criticalChance = 0.32f;
                dreamEssenceCostToNextTier = 156;
                break;
            case 7:
                upgradeName = "Critical Chance II";
                criticalChance = 0.38f;
                dreamEssenceCostToNextTier = 225;
                break;
            case 8:
                upgradeName = "Critical Chance II";
                criticalChance = 0.44f;
                dreamEssenceCostToNextTier = 388;
                break;
            case 9:
                upgradeName = "Critical Chance II";
                criticalChance = 0.5f;
                dreamEssenceCostToNextTier = 805;
                break;
            default:
                upgradeName = "Critical Chance II";
                criticalChance = 0.5f;
                dreamEssenceCostToNextTier = -1;
                break;
        }
        pdm.BaseCritChance = criticalChance;

        upgradeDescription = $"Set critical chance for every click to {criticalChance*100}%";
    }
}