using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerSave
{
    [Header("Progression Data")]
    public IslandState islandState;
    public int currentIsland;
    public int currentWaveIndex;
    
    [Header("Player Stats")]
    public List<string> ownedUpgradeIDs;
    public List<string> ownedSkillIDs;
    public int dreamEssence;
    public int soulEssence;
    public int humanSoul;
    public int damagePerClick;

    public void ResetData()
    {
        islandState = IslandState.HarvestPhase;
        currentIsland = 1;
        currentWaveIndex = 0;
        ownedUpgradeIDs = new List<string>();
        ownedSkillIDs = new List<string>();
        dreamEssence = 0;
        soulEssence = 0;
        humanSoul = 0;
        damagePerClick = 1;
    }
    
    public void RefreshFromRuntime()
    {
        soulEssence = PlayerDataManager.Instance.SoulEssence;
        dreamEssence = PlayerDataManager.Instance.DreamEssence;
        humanSoul = PlayerDataManager.Instance.HumanSoul;

        islandState = IslandManager.Instance.currentState;
        currentIsland = IslandManager.Instance.CurrentIslandIndex;
        currentWaveIndex = WaveManager.Instance.currentWaveData.waveNumber;

        damagePerClick = PlayerDataManager.Instance.DamagePerClick;

        // Note: ownedUpgradeIDs and ownedSkillIDs are not updated here as they are assumed to be managed elsewhere
    }
}

public class PlayerDataManager : MonoBehaviour
{
    public static PlayerDataManager Instance;
    public PlayerSave currentPlayerSave;
    public PlayerHUD playerHUD;

    public VoidEventChannel OnPlayerDataLoaded;

    public IntEventChannel DreamEssenceChangedEvent;
    public IntEventChannel SoulEssenceChangedEvent;
    public IntEventChannel HumanSoulChangedEvent;

    # region Currencies
    private int dreamEssence;
    public int DreamEssence
    {
        get
        {
            return dreamEssence; 
        }
        set
        {
            Debug.Log("Setting Dream Essence to: " + value);
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

    private int soulEssence;
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

    private int humanSoul;
    public int HumanSoul
    {
        get
        {
            return humanSoul;
        }
        set
        {
            Debug.Log("Setting Human Soul to: " + value);
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

    private int damagePerClick = 1;
    public int DamagePerClick
    {
        get
        {
            return damagePerClick;
        }
        set
        {
            damagePerClick = value;
        }
    }

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
        DamagePerClick = currentPlayerSave.damagePerClick;

        IslandManager.Instance.currentState = currentPlayerSave.islandState;
        IslandManager.Instance.CurrentIslandIndex = currentPlayerSave.currentIsland;

        if (currentPlayerSave.ownedUpgradeIDs == null)
        {
            currentPlayerSave.ownedUpgradeIDs = new List<string>();  
        }
        if (currentPlayerSave.ownedSkillIDs == null)
        {
            currentPlayerSave.ownedSkillIDs = new List<string>();
        } 
        
        OnPlayerDataLoaded.RaiseEvent();
    }

    public void UpdateCurrentSaveData()
    {
        currentPlayerSave.RefreshFromRuntime();
    }

    public void SaveToJson()
    {
        string jsonData = JsonUtility.ToJson(currentPlayerSave, true);
        System.IO.File.WriteAllText(Application.persistentDataPath + "/playerSave.json", jsonData);
    }

}
