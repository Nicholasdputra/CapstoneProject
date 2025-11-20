using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SkillMultiplierEntry
{
    public string skillID;
    public float multiplier;
}

[System.Serializable]
public class PlayerSave
{
    [Header("Progression Data")]
    public IslandState islandState;
    public int currentIslandIndex;
    public int currentWaveIndex;

    [Header("Permanent Player Stats")]
    public int baseDamagePerClick;
    public float baseCritChance;
    public float baseCritDamage;
    public int baseAutoCleanDamage;
    public float baseAutoCleanSpeed;
    public int baseHarvestRadius;
    public int baseDreamEssenceDropIncrease;
    public int maxSkillTreeSlots;
    public int currentUsedSkillSlots;

    [Header("Inventory / Unlocks")]
    // public List<string> ownedUpgradeIDs;
    public List<string> ownedSkillIDs;
    public List<SkillMultiplierEntry> activeSkillMultipliers;

    [Header("Currencies")]
    public int dreamEssence;
    public int soulEssence;
    public int humanSoul;

    public void ResetData()
    {
        islandState = IslandState.HarvestPhase;
        currentIslandIndex = 0;
        currentWaveIndex = 0;

        // ownedUpgradeIDs = new List<string>();
        ownedSkillIDs = new List<string>();
        activeSkillMultipliers = new List<SkillMultiplierEntry>();

        dreamEssence = 0;
        soulEssence = 0;
        humanSoul = 0;
        maxSkillTreeSlots = 2;
        currentUsedSkillSlots = 0;

        baseDamagePerClick = 1;
        baseCritChance = 0f;
        baseCritDamage = 0f;
        baseAutoCleanDamage = 1;
        baseAutoCleanSpeed = 1f;
        baseHarvestRadius = 0;
        baseDreamEssenceDropIncrease = 0;
    }

    public void RefreshFromRuntime()
    {
        // Update currencies from PlayerDataManager
        dreamEssence = PlayerDataManager.Instance.DreamEssence;
        soulEssence = PlayerDataManager.Instance.SoulEssence;
        humanSoul = PlayerDataManager.Instance.HumanSoul;

        // Update base stats from PlayerDataManager
        baseDamagePerClick = PlayerDataManager.Instance.BaseDamagePerClick;
        baseCritChance = PlayerDataManager.Instance.BaseCritChance;
        baseCritDamage = PlayerDataManager.Instance.BaseCritDamage;
        baseAutoCleanDamage = PlayerDataManager.Instance.BaseAutoCleaningDamage;
        baseAutoCleanSpeed = PlayerDataManager.Instance.BaseAutoCleaningSpeed;
        baseHarvestRadius = PlayerDataManager.Instance.BaseHarvestRadius;
        maxSkillTreeSlots = SkillManager.Instance.maxSkillTreeSlots;
        currentUsedSkillSlots = SkillManager.Instance.unlockedSkillIDs.Count;

        // Get current island and state from IslandManager
        islandState = IslandManager.Instance.currentState;
        currentIslandIndex = IslandManager.Instance.CurrentIslandIndex;

        // Update owned skills and upgrades
        ownedSkillIDs = new List<string>(SkillManager.Instance.unlockedSkillIDs);
        // ownedUpgradeIDs = new List<string>(UpgradeManager.Instance.unlockedUpgradeIDs);

        // Update active skill multipliers from PlayerDataManager
        activeSkillMultipliers.Clear();
        foreach (var entry in PlayerDataManager.Instance.activeSkillMultipliers)
        {
            activeSkillMultipliers.Add(new SkillMultiplierEntry
            {
                skillID = entry.Key,
                multiplier = entry.Value
            });
        }
    }
}

public class PlayerDataManager : MonoBehaviour
{
    public static PlayerDataManager Instance;
    public PlayerSave currentPlayerSave;
    public PlayerHUD playerHUD;

    # region Events
    [Header("Events")]
    public VoidEventChannel OnPlayerDataLoaded;
    public IntEventChannel DreamEssenceChangedEvent;
    public IntEventChannel SoulEssenceChangedEvent;
    public IntEventChannel HumanSoulChangedEvent;
    # endregion

    # region Currencies
    [Header("Currencies")]
    [Header("Dream Essence")]
    [SerializeField] private int dreamEssence;
    public int DreamEssence
    {
        get
        {
            return dreamEssence; 
        }
        set
        {
            // Debug.Log("Setting Dream Essence to: " + value);
            dreamEssence = value; 
            DreamEssenceChangedEvent.RaiseEvent(dreamEssence);
        }
    }

    public void AddDreamEssence(int amount)
    {
        DreamEssence += amount;
    }

    public bool SpendDreamEssence(int amount)
    {
        if (DreamEssence >= amount)
        {
            DreamEssence -= amount;
            return true;
        }
        return false;
    }

    [Header("Soul Essence")]
    [SerializeField] private int soulEssence;
    public int SoulEssence
    {
        get
        {
            return soulEssence;
        }
        set
        {
            soulEssence = value;
            SoulEssenceChangedEvent.RaiseEvent(soulEssence);
        }
    }

    public void AddSoulEssence(int amount)
    {
        SoulEssence += amount;
    }
    
    public bool SpendSoulEssence(int amount)
    {
        if (SoulEssence >= amount)
        {
            SoulEssence -= amount;
            return true;
        }
        return false;
    }

    [Header("Human Soul")]
    [SerializeField] private int humanSoul;
    public int HumanSoul
    {
        get
        {
            return humanSoul;
        }
        set
        {
            // Debug.Log("Setting Human Soul to: " + value);
            humanSoul = value;
            HumanSoulChangedEvent.RaiseEvent(humanSoul);
        }
    }

    public void AddHumanSoul(int amount)
    {
        HumanSoul += amount;
    }

    public bool SpendHumanSoul(int amount)
    {
        if (HumanSoul >= amount)
        {
            HumanSoul -= amount;
            return true;
        }
        return false;
    }
    # endregion

    # region Player Stats | Base Stats
    [Header("Click Damage Stats")]
    [SerializeField] public int baseDreamEssenceDropIncrease = 0;
    public int BaseDreamEssenceDropIncrease
    {
        get
        {
            return baseDreamEssenceDropIncrease;
        }
        set
        {
            baseDreamEssenceDropIncrease = value;
        }
    }
    public int currentDreamEssenceDropIncrease;

    // Damage Per Click
    [SerializeField] private int baseDamagePerClick = 1;
    public int BaseDamagePerClick
    {
        get
        {
            return baseDamagePerClick;
        }
        set
        {
            baseDamagePerClick = value;
        }
    }
    public int currentDamagePerClick;

    // Crit Chance
    [Header("Crit Chance Stats")]
    [SerializeField] private float baseCritChance = 0f;
    public float BaseCritChance
    {
        get
        {
            return baseCritChance;
        }
        set
        {
            baseCritChance = value;
        }
    }
    public float currentCritChance;

    // Crit Damage
    [Header("Crit Damage Stats")]
    

    [SerializeField] private float baseCritDamage = 0f;
    public float BaseCritDamage
    {
        get
        {
            return baseCritDamage;
        }
        set
        {
            baseCritDamage = value;
        }
    }
    public float currentCritDamage;

    // Auto-Cleaning Speed
    [Header("Auto-Cleaning Speed Stats")]
    [SerializeField] private float baseAutoCleaningSpeed = 1f;
    public float BaseAutoCleaningSpeed
    {
        get
        {
            return baseAutoCleaningSpeed;
        }
        set
        {
            baseAutoCleaningSpeed = value;
        }
    }
    public float currentAutoCleaningSpeed;

    // Auto-Cleaning Damage
    [Header("Auto-Cleaning Damage Stats")]
    [SerializeField] private int baseAutoCleaningDamage = 1;
    public int BaseAutoCleaningDamage
    {
        get
        {
            return baseAutoCleaningDamage;
        }
        set
        {
            baseAutoCleaningDamage = value;
        }
    }
    public int currentAutoCleaningDamage;

    // Harvest Radius
    [Header("Harvest Radius Stats")]
    [SerializeField] private int baseHarvestRadius = 0;
    public int BaseHarvestRadius
    {
        get
        {
            return baseHarvestRadius;
        }
        set
        {
            baseHarvestRadius = value;
        }
    }
    public int currentHarvestRadius;
    # endregion

    # region Player Stats | Multipliers
    // Dropped Dream Essence Multiplier
    [Header("Dropped Dream Essence Multipliers")]
    [SerializeField] private float baseDroppedDreamEssenceMultiplier = 1f;
    public float BaseDroppedDreamEssenceMultiplier
    {
        get
        {
            return baseDroppedDreamEssenceMultiplier;
        }
        set
        {
            baseDroppedDreamEssenceMultiplier = value;
        }
    }
    public float currentDroppedDreamEssenceMultiplier;

    // Dropped Soul Essence Multiplier
    [Header("Dropped Soul Essence Multiplier")]
    [SerializeField] private float baseDroppedSoulEssenceMultiplier = 1f;
    public float BaseDroppedSoulEssenceMultiplier
    {
        get
        {
            return baseDroppedSoulEssenceMultiplier;
        }
        set
        {
            baseDroppedSoulEssenceMultiplier = value;
        }
    }
    public float currentDroppedSoulEssenceMultiplier;

    // Dropped Human Soul Multiplier
    [Header("Dropped Human Soul Multiplier")]
    [SerializeField] private float baseDroppedHumanSoulMultiplier = 1f;
    public float BaseDroppedHumanSoulMultiplier
    {
        get
        {
            return baseDroppedHumanSoulMultiplier;
        }
        set
        {
            baseDroppedHumanSoulMultiplier = value;
        }
    }
    public float currentDroppedHumanSoulMultiplier;

    // Harvest Radius Multiplier
    [Header("Harvest Radius Multiplier")]
    [SerializeField] private float baseDamagePerClickMultiplier = 1f;
    public float BaseDamagePerClickMultiplier
    {
        get
        {
            return baseDamagePerClickMultiplier;
        }
        set
        {
            baseDamagePerClickMultiplier = value;
        }
    }
    public float currentDamagePerClickMultiplier;

    // Crit Chance Multiplier
    [Header("Crit Chance Multiplier")]
    [SerializeField] private float baseCritChanceMultiplier = 1f;
    public float BaseCritChanceMultiplier
    {
        get
        {
            return baseCritChanceMultiplier;
        }
        set
        {
            baseCritChanceMultiplier = value;
        }
    }
    public float currentCritChanceMultiplier;

    // Crit Damage Multiplier
    [Header("Crit Damage Multiplier")]
    [SerializeField] private float baseCritDamageMultiplier = 1f;
    public float BaseCritDamageMultiplier
    {
        get
        {
            return baseCritDamageMultiplier;
        }
        set
        {
            baseCritDamageMultiplier = value;
        }
    }
    public float currentCritDamageMultiplier;

    // Auto-Cleaning Speed Multiplier
    [Header("Auto-Cleaning Speed Multiplier")]
    [SerializeField] private float baseAutoCleaningSpeedMultiplier = 1f;
    public float BaseAutoCleaningSpeedMultiplier
    {
        get
        {
            return baseAutoCleaningSpeedMultiplier;
        }
        set
        {
            baseAutoCleaningSpeedMultiplier = value;
        }
    }
    public float currentAutoCleaningSpeedMultiplier;

    // Auto-Cleaning Damage Multiplier
    [Header("Auto-Cleaning Damage Multiplier")]
    [SerializeField] private float baseAutoCleaningDamageMultiplier = 1f;
    public float BaseAutoCleaningDamageMultiplier
    {
        get
        {
            return baseAutoCleaningDamageMultiplier;
        }
        set
        {
            baseAutoCleaningDamageMultiplier = value;
        }
    }
    public float currentAutoCleaningDamageMultiplier;

    // Harvest Radius Multiplier
    [Header("Harvest Radius Multiplier")]
    [SerializeField] private float baseHarvestRadiusMultiplier = 1f;
    public float BaseHarvestRadiusMultiplier
    {
        get
        {
            return baseHarvestRadiusMultiplier;
        }
        set
        {
            baseHarvestRadiusMultiplier = value;
        }
    }
    public float currentHarvestRadiusMultiplier;
    # endregion
    
    public Dictionary<string, float> activeSkillMultipliers = new Dictionary<string, float>();
    
    # region Skill Multiplier Management
    public void ApplySkillMultiplier(string skillID, float multiplier)
    {
        if (string.IsNullOrEmpty(skillID))
        {
            Debug.LogWarning("Tried to apply skill multiplier with empty skill ID.");
            return;
        }

        if (float.IsNaN(multiplier) || float.IsInfinity(multiplier))
        {
            Debug.LogWarning($"Invalid multiplier for skill {skillID}. Resetting to 1.");
            multiplier = 1f;
        }

        activeSkillMultipliers[skillID] = multiplier;
        RecalculateFinalStats();
    }

    public void RemoveSkillMultiplier(string skillID)
    {
        if (activeSkillMultipliers.ContainsKey(skillID))
            activeSkillMultipliers.Remove(skillID);

        RecalculateFinalStats();
    }

    public void RecalculateFinalStats()
    {
        // Set all the variables that start with current to their base values
        Debug.Log("Base Dream Essence Drop Increase: " + baseDreamEssenceDropIncrease);
        currentDreamEssenceDropIncrease = baseDreamEssenceDropIncrease;
        Debug.Log("Current Dream Essence Drop Increase reset to base: " + baseDreamEssenceDropIncrease);
        
        Debug.Log("base Harvest Radius: " + baseHarvestRadius); 
        currentHarvestRadius = baseHarvestRadius;
        Debug.Log("Current Harvest Radius reset to base: " + baseHarvestRadius);
        Debug.Log("Base Damage Per Click: " + BaseDamagePerClick);

        currentDamagePerClick = baseDamagePerClick;
        
        currentCritChance = baseCritChance;
        currentCritDamage = baseCritDamage;

        currentAutoCleaningSpeed = baseAutoCleaningSpeed;
        currentAutoCleaningDamage = baseAutoCleaningDamage;
        
        currentHarvestRadiusMultiplier = baseHarvestRadiusMultiplier;
        currentDamagePerClickMultiplier = baseDamagePerClickMultiplier;
        
        currentCritChanceMultiplier = baseCritChanceMultiplier;
        currentCritDamageMultiplier = baseCritDamageMultiplier;
        
        currentAutoCleaningSpeedMultiplier = baseAutoCleaningSpeedMultiplier;
        currentAutoCleaningDamageMultiplier = baseAutoCleaningDamageMultiplier;
        
        currentDroppedDreamEssenceMultiplier = baseDroppedDreamEssenceMultiplier;
        currentDroppedSoulEssenceMultiplier = baseDroppedSoulEssenceMultiplier;
        currentDroppedHumanSoulMultiplier = baseDroppedHumanSoulMultiplier;

        // Apply all active skill multipliers
        foreach (var entry in activeSkillMultipliers)
        {
            string skillID = entry.Key;
            float multiplier = entry.Value;

            switch (skillID)
            {
                case "Skill_BigSweep":
                    currentHarvestRadiusMultiplier *= multiplier;
                    break;
                case "Skill_DustOff":
                    currentDamagePerClickMultiplier *= multiplier;
                    break;
                case "Skill_DustyEssence":
                    Debug.Log("Applying Dropped Dream Essence Multiplier: " + multiplier);
                    currentDroppedDreamEssenceMultiplier *= multiplier;
                    Debug.Log("New Dropped Dream Essence Multiplier: " + currentDroppedDreamEssenceMultiplier);
                    break;
                case "Skill_FatefulNight":
                    currentCritChance += multiplier;
                    break;
                case "Skill_HelpingHand":
                    currentAutoCleaningSpeedMultiplier *= multiplier;
                    break;
                case "Skill_HugeBlow":
                    currentCritDamage += multiplier;
                    break;
                case "Skill_SurfacePressure":
                    currentAutoCleaningDamageMultiplier *= multiplier;
                    break;
            }
        }
        
        // Debug.Log("Sanity Checks");
        currentDamagePerClickMultiplier = Mathf.Max(0f, currentDamagePerClickMultiplier);
        currentCritChanceMultiplier = Mathf.Max(0f, currentCritChanceMultiplier);
        currentCritDamageMultiplier = Mathf.Max(0f, currentCritDamageMultiplier);
        currentAutoCleaningSpeedMultiplier = Mathf.Max(0f, currentAutoCleaningSpeedMultiplier);
        currentAutoCleaningDamageMultiplier = Mathf.Max(0f, currentAutoCleaningDamageMultiplier);
        currentHarvestRadiusMultiplier = Mathf.Max(0f, currentHarvestRadiusMultiplier);

        currentDroppedDreamEssenceMultiplier = Mathf.Max(0.1f, currentDroppedDreamEssenceMultiplier);
        currentDroppedSoulEssenceMultiplier = Mathf.Max(0.1f, currentDroppedSoulEssenceMultiplier);
        currentDroppedHumanSoulMultiplier = Mathf.Max(0.1f, currentDroppedHumanSoulMultiplier);
    
        currentDamagePerClick = Mathf.Max(1, currentDamagePerClick);
        currentAutoCleaningDamage = Mathf.Max(1, currentAutoCleaningDamage);
        currentAutoCleaningSpeed = Mathf.Max(0.1f, currentAutoCleaningSpeed);
        currentHarvestRadius = Mathf.Max(0, currentHarvestRadius);
        currentCritDamage = Mathf.Max(0f, currentCritDamage);
        currentCritChance = Mathf.Clamp(currentCritChance, 0f, 1f);
        currentDreamEssenceDropIncrease = Mathf.Max(0, currentDreamEssenceDropIncrease);

        
        // Debug.Log("Recalculated Stats:");
        currentDreamEssenceDropIncrease = Mathf.Max(0, currentDreamEssenceDropIncrease);
        currentDamagePerClick = Mathf.RoundToInt(currentDamagePerClick * currentDamagePerClickMultiplier);
        currentCritChance *= currentCritChanceMultiplier;
        currentCritDamage *= currentCritDamageMultiplier;
        currentAutoCleaningSpeed *= currentAutoCleaningSpeedMultiplier;
        currentAutoCleaningDamage = Mathf.RoundToInt(currentAutoCleaningDamage * currentAutoCleaningDamageMultiplier);
        currentHarvestRadius = Mathf.RoundToInt(currentHarvestRadius * currentHarvestRadiusMultiplier);
        currentDroppedDreamEssenceMultiplier = Mathf.Max(0f, currentDroppedDreamEssenceMultiplier);
        currentDroppedSoulEssenceMultiplier = Mathf.Max(0f, currentDroppedSoulEssenceMultiplier);
        currentDroppedHumanSoulMultiplier = Mathf.Max(0f, currentDroppedHumanSoulMultiplier);
    }
    # endregion

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
        }
    }

    public void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        playerHUD = FindObjectOfType<PlayerHUD>();
        LoadFromJson();
    }

    public void LoadFromJson()
    {
        string filePath = Application.persistentDataPath + "/playerSave.json";

        if (System.IO.File.Exists(filePath))
        {
            string jsonData = System.IO.File.ReadAllText(filePath);
            currentPlayerSave = JsonUtility.FromJson<PlayerSave>(jsonData);
        }
        else
        {
            currentPlayerSave = new PlayerSave();
            currentPlayerSave.ResetData();
        }

        CopyValuesToRuntimeVariables();
    }

    void CopyValuesToRuntimeVariables()
    {
        DreamEssence = currentPlayerSave.dreamEssence;
        SoulEssence = currentPlayerSave.soulEssence;
        HumanSoul = currentPlayerSave.humanSoul;
        BaseDamagePerClick = currentPlayerSave.baseDamagePerClick;
        BaseCritChance = currentPlayerSave.baseCritChance;
        BaseCritDamage = currentPlayerSave.baseCritDamage;
        BaseAutoCleaningDamage = currentPlayerSave.baseAutoCleanDamage;
        BaseAutoCleaningSpeed = currentPlayerSave.baseAutoCleanSpeed;
        BaseHarvestRadius = currentPlayerSave.baseHarvestRadius;
        BaseDreamEssenceDropIncrease = currentPlayerSave.baseDreamEssenceDropIncrease;
        
        SkillManager.Instance.maxSkillTreeSlots = currentPlayerSave.maxSkillTreeSlots;

        activeSkillMultipliers = new Dictionary<string, float>();

        foreach (var entry in currentPlayerSave.activeSkillMultipliers)
        {
            if (string.IsNullOrEmpty(entry.skillID))
            {
                continue;  
            } 
            if (float.IsNaN(entry.multiplier) || float.IsInfinity(entry.multiplier))
            {
                continue;
            }

            activeSkillMultipliers[entry.skillID] = Mathf.Max(0f, entry.multiplier);
        }

        IslandManager.Instance.currentState = currentPlayerSave.islandState;
        IslandManager.Instance.CurrentIslandIndex = currentPlayerSave.currentIslandIndex;

        // if (currentPlayerSave.ownedUpgradeIDs == null)
        // {
        //     currentPlayerSave.ownedUpgradeIDs = new List<string>();  
        // }
        if (currentPlayerSave.ownedSkillIDs == null)
        {
            currentPlayerSave.ownedSkillIDs = new List<string>();
        } 
        
        RecalculateFinalStats();
        LoadUpgrades(); 
        ApplyLoadedTiers();
        OnPlayerDataLoaded.RaiseEvent();
    }

    public void SaveUpgrades()
    {
        foreach (var kvp in UpgradeManager.Instance.upgradesDict)
        {
            PlayerPrefs.SetInt(UpgradeManager.SavePrefix + kvp.Key, kvp.Value.upgradeTier);
        }
        PlayerPrefs.Save();
    }

    public void LoadUpgrades()
    {
        foreach (var kvp in UpgradeManager.Instance.upgradesDict)
        {
            int savedTier = PlayerPrefs.GetInt(UpgradeManager.SavePrefix + kvp.Key, 0);
            kvp.Value.upgradeTier = savedTier;
        }
    }

    public void ApplyLoadedTiers()
    {
        foreach (var kvp in UpgradeManager.Instance.upgradesDict)
        {
            kvp.Value.DecideEffect();
        }
    }

    public void UpdateCurrentSaveData()
    {
        currentPlayerSave.activeSkillMultipliers = new List<SkillMultiplierEntry>();
        currentPlayerSave.RefreshFromRuntime();

        foreach (var entry in activeSkillMultipliers)
        {
            currentPlayerSave.activeSkillMultipliers.Add(new SkillMultiplierEntry
            {
                skillID = entry.Key,
                multiplier = entry.Value
            });
        }
    }

    public void SaveToJson()
    {
        string jsonData = JsonUtility.ToJson(currentPlayerSave, true);
        System.IO.File.WriteAllText(Application.persistentDataPath + "/playerSave.json", jsonData);
    }
}