using UnityEngine;

// #region Harvestable Data

// #endregion

#region Player Variables
[System.Serializable]
public class PlayerVars
{
    public int soulEssences;
    public int soulEssencesGainPerClick;
    public int damageToHarvestablesPerClick;
    public int isInMinibossPhase;
    public int isInBossPhase;

    public int currentWaveIndex;
    public int currentIslandIndex;
    public int destroyedHarvestableObjectsCount;

    public PlayerVars()
    {
        soulEssences = 0;
        soulEssencesGainPerClick = 1;
        damageToHarvestablesPerClick = 1;
    }

    public void GetCurrentVariableAmounts()
    {
        soulEssences = PlayerData.Instance.SoulEssences;
        soulEssencesGainPerClick = PlayerData.Instance.soulEssencesGainPerClick;
        damageToHarvestablesPerClick = PlayerData.Instance.damageToHarvestablesPerClick;
        isInMinibossPhase = WaveManager.Instance.isInMinibossPhase;

        currentWaveIndex = WaveManager.Instance.CurrentWaveIndex;
        currentIslandIndex = IslandManager.Instance.CurrentIslandIndex;
        destroyedHarvestableObjectsCount = WaveManager.Instance.DestroyedHarvestableObjectsCount;
    }
}
#endregion

public class PlayerData : MonoBehaviour
{
    public static PlayerData Instance;
    public PlayerHUD playerHUD;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            // LoadData();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /* What To Save and Load:
        - Soul Essences
        - Soul Essences Gain Per Click
        - Damage To Harvestables Per Click
        - Damage To Boss Per Click

        - Is In Miniboss Phase or Not
        - Is In Boss Phase or Not
        - Current Wave
        - Current Island
        - Number of Destroyed Harvestables This Wave
    */

    [Header("Clicking Variables")]
    public int soulEssencesGainPerClick;
    public int damageToHarvestablesPerClick;
    public int damageToBossPerClick;

    #region Soul Essences
    [SerializeField] private int soulEssences;
    public int SoulEssences
    {
        get => soulEssences;
        set => soulEssences = Mathf.Max(0, value);
    }

    public void AddSoulEssences(int amount)
    {
        SoulEssences += amount;
        // SaveData();
        playerHUD.soulEssencesText.text = SoulEssences.ToString();
    }

    // Check if the player has enough Soul Essences before spending
    public bool SpendSoulEssences(int amount)
    {
        if (SoulEssences >= amount)
        {
            SoulEssences -= amount;
            playerHUD.soulEssencesText.text = SoulEssences.ToString();
            // SaveData();
            return true;
        }
        else
        {
            return false;
        }
    }
    #endregion

    # region Dream Essences
    [SerializeField] private int dreamEssences;
    public int DreamEssences
    {
        get => dreamEssences;
        set => dreamEssences = Mathf.Max(0, value);
    }

    public void AddDreamEssences(int amount)
    {
        DreamEssences += amount;
        // SaveData();
        playerHUD.dreamEssencesText.text = DreamEssences.ToString();
    }

    // Check if the player has enough Soul Essences before spending
    public bool SpendDreamEssences(int amount)
    {
        if (DreamEssences >= amount)
        {
            DreamEssences -= amount;
            playerHUD.soulEssencesText.text = DreamEssences.ToString();
            // SaveData();
            return true;
        }
        else
        {
            return false;
        }
    }
    # endregion

    # region Human Souls
    [SerializeField] private int humanSouls;
    public int HumanSouls
    {
        get => humanSouls;
        set => humanSouls = Mathf.Max(0, value);
    }

    public void AddHumanSouls(int amount)
    {
        HumanSouls += amount;
        // SaveData();
        playerHUD.humanSoulText.text = HumanSouls.ToString();
    }

    // Check if the player has enough Soul Essences before spending
    public bool SpendHumanSouls(int amount)
    {
        if (HumanSouls >= amount)
        {
            HumanSouls -= amount;
            playerHUD.soulEssencesText.text = HumanSouls.ToString();
            // SaveData();
            return true;
        }
        else
        {
            return false;
        }
    }
    #endregion

    //     [ContextMenu("Clear All Data")]
    //     public void ClearAllData()
    //     {
    //         //Reset values to baseline
    //         soulEssences = 0;
    //         soulEssencesGainPerClick = 1;
    //         damageToHarvestablesPerClick = 1;

    //         DeleteHarvestableData();
    //         DeletePlayerVars();

    //         //Reset HUD
    //         if (playerHUD != null)
    //             playerHUD.soulEssencesText.text = "Soul Essences: 0";
    //         playerHUD.waveText.text = "Waves: 1 / 10";
    //     }

    //     #region Save       
    //     private static string GetPath(string fileName)
    //     {
    //         return Path.Combine(Application.persistentDataPath, fileName + ".json");
    //     }

    //     public static void SaveHarvestableData(HarvestableData data, string fileName = "harvestables")
    //     {
    //         string json = JsonUtility.ToJson(data, true);
    //         File.WriteAllText(GetPath(fileName), json);
    // #if UNITY_EDITOR
    //         Debug.Log($"[SaveSystem] Saved harvestable data to {GetPath(fileName)}");
    // #endif
    //     }

    //     public static void SavePlayerVars(PlayerVars data, string fileName = "playerVariables")
    //     {
    //         string json = JsonUtility.ToJson(data, true);
    //         File.WriteAllText(GetPath(fileName), json);
    // #if UNITY_EDITOR
    //         Debug.Log($"[SaveSystem] Saved player variables data to {GetPath(fileName)}");
    // #endif
    //     }

    //     void SaveData()
    //     {
    //         // Save Harvestable Data
    //         HarvestableData harvestableData = new HarvestableData();
    //         harvestableData.PopulateFromScene();
    //         SaveHarvestableData(harvestableData);

    //         // Save Player Variables
    //         PlayerVars playerVars = new PlayerVars();
    //         playerVars.GetCurrentVariableAmounts();
    //         SavePlayerVars(playerVars);
    //     }

    //     #endregion

    //     #region Load
    //     public static HarvestableData LoadHarvestableData(string fileName = "harvestables")
    //     {
    //         string path = GetPath(fileName);

    //         if (File.Exists(path))
    //         {
    //             string json = File.ReadAllText(path);
    //             HarvestableData data = JsonUtility.FromJson<HarvestableData>(json);
    //             return data;
    //         }
    //         else
    //         {
    // #if UNITY_EDITOR
    //             Debug.LogWarning($"[SaveSystem] No save file found at {path}");
    // #endif
    //             return new HarvestableData();
    //         }
    //     }

    //     public static PlayerVars LoadPlayerVars(string fileName = "playerVariables")
    //     {
    //         string path = GetPath(fileName);

    //         if (File.Exists(path))
    //         {
    //             string json = File.ReadAllText(path);
    //             PlayerVars data = JsonUtility.FromJson<PlayerVars>(json);
    //             return data;
    //         }
    //         else
    //         {
    // #if UNITY_EDITOR
    //             Debug.LogWarning($"[SaveSystem] No save file found at {path}");
    // #endif
    //             return new PlayerVars(); 
    //         }
    //     }

    //     void LoadData()
    //     {
    //         //Load
    //         HarvestableData loadedHarvestableData = LoadHarvestableData();
    //         PlaceHarvestablesFromData(loadedHarvestableData);
    //         PlayerVars loadedPlayerVars = LoadPlayerVars();
    //         ApplyPlayerVars(loadedPlayerVars);

    //         //Update HUD
    //         if (playerHUD != null)
    //             playerHUD.soulEssencesText.text = "Soul Essences: " + SoulEssences.ToString();
    //     }


    //     void PlaceHarvestablesFromData(HarvestableData data)
    //     {
    //         // Clear existing harvestables
    //         HarvestableObject[] existingHarvestables = GameObject.FindObjectsOfType<HarvestableObject>();
    //         foreach (var obj in existingHarvestables)
    //         {
    //             Destroy(obj.gameObject);
    //         }

    //         GameObject harvestablePrefab = WaveManager.instance.harvestableObjectPrefab;

    //         // Place harvestables based on loaded data
    //         for (int i = 0; i < data.harvestablesHealths.Length; i++)
    //         {
    //             int health = data.harvestablesHealths[i];

    //             // Instantiate at their previous location
    //             if (health > 0 && data.harvestablePositions[i] != null) // Only place if there's a valid health
    //             {

    //                 Vector3 spawnPosition = data.harvestablePositions[i].ToVector3();
    //                 GameObject newHarvestable = Instantiate(harvestablePrefab, spawnPosition, Quaternion.identity);
    //                 HarvestableObject harvestableObj = newHarvestable.GetComponent<HarvestableObject>();
    //                 Debug.Log($"Placing {newHarvestable.transform.name} at {spawnPosition} with health {health}");

    //                 // then set its health and mark that position as marked in the grid
    //                 if (harvestableObj != null)
    //                 {
    //                     harvestableObj.Health = health;

    //                     if (GridManager.instance != null)
    //                     {
    //                         Vector2Int gridPos = GridManager.instance.WorldToGrid(spawnPosition);
    //                         GridManager.instance.SetCellOccupied(gridPos, true);
    //                     }
    //                 }
    //             }
    //         }
    //     }

    //     void ApplyPlayerVars(PlayerVars vars)
    //     {
    //         instance.soulEssences = vars.soulEssences;
    //         instance.soulEssencesGainPerClick = vars.soulEssencesGainPerClick;
    //         instance.damageToHarvestablesPerClick = vars.damageToHarvestablesPerClick;

    //         if (WaveManager.instance != null)
    //         {
    //             WaveManager.instance.isInMinibossPhase = vars.isInMinibossPhase;
    //             WaveManager.instance.CurrentWave = vars.currentWave;
    //             WaveManager.instance.DestroyedHarvestableObjectsCount = vars.destroyedHarvestableObjectsCount;
    //         }
    //     }
    //     #endregion

    //     public static void DeleteHarvestableData(string fileName = "harvestables")
    //     {
    //         string path = GetPath(fileName);
    //         if (File.Exists(path))
    //             File.Delete(path);
    //     }

    //     public static void DeletePlayerVars(string fileName = "playerVariables")
    //     {
    //         string path = GetPath(fileName);
    //         if (File.Exists(path))
    //             File.Delete(path);
    //     }

    //     private void OnApplicationQuit()
    //     {
    //         SaveData();
    //     }
}