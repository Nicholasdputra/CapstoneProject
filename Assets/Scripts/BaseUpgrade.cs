using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseUpgrade : MonoBehaviour
{
    public string upgradeName;
    public string upgradeDescription;
    public int isUnlocked;
    public string upgradeID;
    public Sprite upgradeIcon;
    public int upgradeTier;
    public int maxUpgradeTier;
    public int dreamEssenceCostToNextTier;
    public abstract void DecideEffect();
}
