using UnityEngine;

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
            LoadData();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /* What to save and load:
        - Soul Essences
    */

    #region Soul Essences
    private int soulEssences;
    public int SoulEssences
    {
        get => soulEssences; 
        set => soulEssences = Mathf.Max(0, value);
    }

    public void AddSoulEssences(int amount)
    {
        SoulEssences += amount;
        SaveData();
        playerHUD.soulEssencesText.text = "Soul Essences: " + SoulEssences.ToString();
    }

    // Check if the player has enough Soul Essences before spending
    public bool SpendSoulEssences(int amount)
    {
        if (SoulEssences >= amount)
        {
            SoulEssences -= amount;
            playerHUD.soulEssencesText.text = "Soul Essences: " + SoulEssences.ToString();
            SaveData();
            return true;
        }
        else
        {
            return false;
        }
    }
    #endregion


    [ContextMenu("Clear All Data")]
    public void ClearAllData()
    {
        //Clear PlayerPrefs 
        PlayerPrefs.DeleteKey("SoulEssences");

        PlayerPrefs.Save();

        //Reset values
        soulEssences = 0;

        //Reset HUD
        if (playerHUD != null)
            playerHUD.soulEssencesText.text = "Soul Essences: 0";
        
    }

    void SaveData()
    {
        PlayerPrefs.SetInt("SoulEssences", soulEssences);

    }

    void LoadData()
    {
        soulEssences = PlayerPrefs.GetInt("SoulEssences", 0);
    }
}
