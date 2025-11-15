using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UpgradeData
{
    public string upgradeId;
    public int currentTier;
}

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager Instance;

    [Header("Upgrade IDs")]
    public List<string> allUpgradeIDs = new List<string>();
    
    
    [Header("Runtime Upgrades")]
    public Dictionary<string, BaseUpgrade> upgradesDict = new Dictionary<string, BaseUpgrade>();

    public const string SavePrefix = "upgradeTier_";
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        BuildUpgradeDictionary();
    }

    private void BuildUpgradeDictionary()
    {
        upgradesDict.Clear();

        BaseUpgrade[] upgradesInScene = FindObjectsOfType<BaseUpgrade>();
        foreach (BaseUpgrade upgrade in upgradesInScene)
        {
            if (!upgradesDict.ContainsKey(upgrade.upgradeID))
            {
                upgradesDict.Add(upgrade.upgradeID, upgrade);
                if (!allUpgradeIDs.Contains(upgrade.upgradeID))
                {
                    allUpgradeIDs.Add(upgrade.upgradeID);
                }
            }
        }
    }

    public bool TryUpgrade(string id)
    {
        if (!upgradesDict.ContainsKey(id))
        {
            Debug.LogError($"Upgrade ID {id} not found.");
            return false;
        }

        BaseUpgrade up = upgradesDict[id];

        if (up.dreamEssenceCostToNextTier == -1)
        {
            Debug.Log("Max tier reached.");
            return false;
        }

        int cost = up.dreamEssenceCostToNextTier;

        if (PlayerDataManager.Instance.DreamEssence < cost)
        {
            Debug.Log("Not enough Dream Essence.");
            return false;
        }

        PlayerDataManager.Instance.SpendDreamEssence(cost);

        up.upgradeTier++;
        up.DecideEffect();

        PlayerDataManager.Instance.SaveUpgrades();  // << SAVE AFTER UPGRADING

        Debug.Log($"Upgraded {id} to tier {up.upgradeTier}!");

        return true;
    }

    public T GetUpgrade<T>(string id) where T : BaseUpgrade
    {
        if (upgradesDict.ContainsKey(id))
            return upgradesDict[id] as T;
        return null;
    }
}
