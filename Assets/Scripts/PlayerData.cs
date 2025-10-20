using UnityEngine;
using System.IO;

// #region Vector3 Wrapper
// [System.Serializable]
// public class SerializableVector3
// {
//     public float x, y, z;

//     public SerializableVector3(float x, float y, float z)
//     {
//         this.x = x;
//         this.y = y;
//         this.z = z;
//     }

//     public SerializableVector3(Vector3 vector)
//     {
//         x = vector.x;
//         y = vector.y;
//         z = vector.z;
//     }

//     public Vector3 ToVector3()
//     {
//         return new Vector3(x, y, z);
//     }
// }
// # endregion

// #region Harvestable Data
// [System.Serializable]
// public class HarvestableData
// {
//     public int[] harvestablesHealths;
//     public SerializableVector3[] harvestablePositions;

//     public HarvestableData()
//     {
//         // Initialize arrays with default values
//         int arraySize = (WaveManager.instance != null) ? WaveManager.instance.harvestableObjectThisWave : 10;
//         harvestablesHealths = new int[arraySize];
//         harvestablePositions = new SerializableVector3[arraySize];
//         for (int i = 0; i < WaveManager.instance.harvestableObjectThisWave; i++)
//         {
//             Debug.Log("Initializing harvestable data arrays with size: " + WaveManager.instance.harvestableObjectThisWave);
//             harvestablesHealths[i] = -1; // -1 means no harvestable at this position
//             Debug.Log("Initialized health at index " + i + " to -1");
//             harvestablePositions[i] = null; // Initialize with 0,0,0
//         }
        
//     }

//     // Method to populate data from current scene
//     public void PopulateFromScene()
//     {
//         HarvestableObject[] harvestableObjects = GameObject.FindObjectsOfType<HarvestableObject>();

//         // First, reset all arrays to default values
//         for (int i = 0; i < harvestablesHealths.Length; i++)
//         {
//             harvestablesHealths[i] = -1;
//             harvestablePositions[i] = null;
//         }

//         // Then populate with actual harvestables (up to array limit)
//         for (int i = 0; i < Mathf.Min(harvestableObjects.Length, harvestablesHealths.Length); i++)
//         {
//             harvestablesHealths[i] = harvestableObjects[i].Health;
//             harvestablePositions[i] = new SerializableVector3(harvestableObjects[i].transform.position);
//         }
//     }
// }
// #endregion

// #region Player Variables
// [System.Serializable]
// public class PlayerVars
// {
//     public int soulEssences;
//     public int soulEssencesGainPerClick;
//     public int damageToHarvestablesPerClick;
//     public int isInMinibossPhase;
//     public int currentWave;
//     public int destroyedHarvestableObjectsCount;

//     public PlayerVars()
//     {
//         soulEssences = 0;
//         soulEssencesGainPerClick = 1;
//         damageToHarvestablesPerClick = 1;
//     }

//     public void GetCurrentVariableAmounts()
//     {
//         soulEssences = PlayerData.instance.SoulEssences;
//         soulEssencesGainPerClick = PlayerData.instance.soulEssencesGainPerClick;
//         damageToHarvestablesPerClick = PlayerData.instance.damageToHarvestablesPerClick;
//         isInMinibossPhase = WaveManager.instance.isInMinibossPhase;
//         currentWave = WaveManager.instance.CurrentWave;
//         destroyedHarvestableObjectsCount = WaveManager.instance.DestroyedHarvestableObjectsCount;
//     }
// }
// #endregion

public class PlayerData : MonoBehaviour
{
    public static PlayerData instance;
    public PlayerHUD playerHUD;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
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

        - Is In Miniboss Phase or Not
        - Current Wave
        - Number of Destroyed Harvestables This Wave
        - Health of All Harvestables
        - Position of All Harvestables
    */

    [Header("Clicking Variables")]
    public int soulEssencesGainPerClick;
    public int damageToHarvestablesPerClick;

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
        playerHUD.soulEssencesText.text = "Soul Essences: " + SoulEssences.ToString();
    }

    // Check if the player has enough Soul Essences before spending
    public bool SpendSoulEssences(int amount)
    {
        if (SoulEssences >= amount)
        {
            SoulEssences -= amount;
            playerHUD.soulEssencesText.text = "Soul Essences: " + SoulEssences.ToString();
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