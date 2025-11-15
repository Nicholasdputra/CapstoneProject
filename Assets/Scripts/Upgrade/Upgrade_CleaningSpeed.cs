using UnityEngine;

public class Upgrade_CleaningSpeed : BaseUpgrade
{
    public float cleaningSpeedIncrease;

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
                upgradeName = "Cleaning Speed I";
                cleaningSpeedIncrease = 0.05f;
                dreamEssenceCostToNextTier = 10;
                break;
            case 1:
                upgradeName = "Cleaning Speed I";
                cleaningSpeedIncrease = 0.1f;
                dreamEssenceCostToNextTier = 12;
                break;
            case 2:
                upgradeName = "Cleaning Speed I";
                cleaningSpeedIncrease = 0.15f;
                dreamEssenceCostToNextTier = 17;
                break;
            case 3:
                upgradeName = "Cleaning Speed I";
                cleaningSpeedIncrease = 0.2f;
                dreamEssenceCostToNextTier = 30;
                break;
            case 4:
                upgradeName = "Cleaning Speed I";
                cleaningSpeedIncrease = 0.25f;
                dreamEssenceCostToNextTier = 62;
                break;
            case 5:
                upgradeName = "Cleaning Speed I";
                cleaningSpeedIncrease = 0.3f;
                dreamEssenceCostToNextTier = 65;
                break;
            case 6:
                upgradeName = "Cleaning Speed II";
                cleaningSpeedIncrease = 0.37f;
                dreamEssenceCostToNextTier = 78;
                break;
            case 7:
                upgradeName = "Cleaning Speed II";
                cleaningSpeedIncrease = 0.44f;
                dreamEssenceCostToNextTier = 112;
                break;
            case 8:
                upgradeName = "Cleaning Speed II";
                cleaningSpeedIncrease = 0.51f;
                dreamEssenceCostToNextTier = 194;
                break;
            case 9:
                upgradeName = "Cleaning Speed I";
                cleaningSpeedIncrease = 0.58f;
                dreamEssenceCostToNextTier = 402;
                break;
            case 10:
                upgradeName = "Cleaning Speed II";
                cleaningSpeedIncrease = 0.65f;
                dreamEssenceCostToNextTier = -1;
                break;
            default:
                upgradeName = "Cleaning Speed II";
                cleaningSpeedIncrease = 0.65f;
                dreamEssenceCostToNextTier = -1;
                break;
        }
        pdm.BaseAutoCleaningSpeed = 1 + cleaningSpeedIncrease;
    }
}