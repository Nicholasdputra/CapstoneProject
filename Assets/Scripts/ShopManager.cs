using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public static ShopManager Instance;

    [Header("Canvases")]
    public GameObject HUDCanvas;
    public GameObject ShopCanvas;

    public TextMeshProUGUI dreamEssenceText;
    public TextMeshProUGUI soulEssenceText;
    public TextMeshProUGUI humanSoulText;

    public void Awake()
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
    }

    public void OpenShop()
    {
        AudioManager.Instance.PlaySFXOnce(6);
        if(HUDCanvas == null)
        {
            // Find in scene
            HUDCanvas = GameObject.Find("HUDCanvas");
        }
        // Disable HUD
        HUDCanvas.SetActive(false);
        if(ShopCanvas == null)
        {
            // Find in scene
            ShopCanvas = GameObject.Find("Shop/SkillTree");
        }
        
        if(dreamEssenceText == null)
        {
            // Debug.Log("Finding Dream Essence Text");    
            dreamEssenceText = ShopCanvas.transform.Find("Dream Essence").GetComponentInChildren<TextMeshProUGUI>();
            // Debug.Log("Found: " + dreamEssenceText);
        }

        if(soulEssenceText == null)
        {
            // Debug.Log("Finding Soul Essence Text");
            soulEssenceText = ShopCanvas.transform.Find("Soul Essence").GetComponentInChildren<TextMeshProUGUI>();
            // Debug.Log("Found: " + soulEssenceText);
        }

        if(humanSoulText == null)
        {
            // Debug.Log("Finding Human Soul Text");
            humanSoulText = ShopCanvas.transform.Find("Human Soul").GetComponentInChildren<TextMeshProUGUI>();
            // Debug.Log("Found: " + humanSoulText);
        }
        ShopCanvas.SetActive(true);

        dreamEssenceText.text = PlayerDataManager.Instance.DreamEssence.ToString();
        soulEssenceText.text = PlayerDataManager.Instance.SoulEssence.ToString();
        humanSoulText.text = PlayerDataManager.Instance.HumanSoul.ToString();

        SkillManager.Instance.skillSlotsText = ShopCanvas.transform.Find("SellActiveSkills")?.Find("SkillPoints").GetComponentInChildren<TextMeshProUGUI>();
        Debug.Log("Skill Slots Text: " + SkillManager.Instance.skillSlotsText);
        SkillManager.Instance.UpdateSkillSlotsUI();
    }

    public void UpdateCurrencyUI()
    {
        if (dreamEssenceText != null)
            dreamEssenceText.text = PlayerDataManager.Instance.DreamEssence.ToString();

        if (soulEssenceText != null)
            soulEssenceText.text = PlayerDataManager.Instance.SoulEssence.ToString();

        if (humanSoulText != null)
            humanSoulText.text = PlayerDataManager.Instance.HumanSoul.ToString();
    }


    public void CloseShop()
    {
        if(HUDCanvas == null)
        {
            // Find in scene
            HUDCanvas = GameObject.Find("HUDCanvas");
        }
        // Disable HUD
        HUDCanvas.SetActive(true);
        if(ShopCanvas == null)
        {
            // Find in scene
            ShopCanvas = GameObject.Find("Shop/SkillTree");
        }
        ShopCanvas.SetActive(false);
    }

    public void RefreshAllUpgradeButtons()
{
    // find all UpgradeButton components under the shop canvas
    if (ShopCanvas == null) return;
    UpgradeShopButton[] buttons = ShopCanvas.GetComponentsInChildren<UpgradeShopButton>(true);
    foreach (var b in buttons) b.RefreshUI();
}

}