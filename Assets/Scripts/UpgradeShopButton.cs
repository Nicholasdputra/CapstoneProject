using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UpgradeShopButton : MonoBehaviour
{
    [Header("Link")]
    public string upgradeID;               // set in inspector (must match BaseUpgrade.upgradeID)

    // runtime
    private BaseUpgrade linkedUpgrade;

    [Header("UI")]
    public TextMeshProUGUI upgradeTitleText;
    public TextMeshProUGUI upgradeDescriptionText; 
    public TextMeshProUGUI upgradeStageText;      // was "Stage"
    public TextMeshProUGUI upgradeCostText;
    public Button upgradeButton;
    public TextMeshProUGUI buttonText;

    void Start()
    {
        Initialize();
    }

    void Initialize()
    {
        // if you prefer to assign in inspector, remove these finds
        if (upgradeTitleText == null)
        {
            upgradeTitleText = transform.Find("Upgrade Name")?.GetComponent<TextMeshProUGUI>();  
        } 
        if (upgradeDescriptionText == null)
        {
            upgradeDescriptionText = transform.Find("UpgradeDesc")?.GetComponent<TextMeshProUGUI>();
        } 
        if (upgradeStageText == null)
        {
            upgradeStageText = transform.Find("Stage")?.GetComponent<TextMeshProUGUI>();
        } 
        if (upgradeButton == null)
        {
            upgradeButton = transform.Find("Buy Upgrade Button")?.GetComponent<Button>();
        } 
        if (buttonText == null && upgradeButton != null)
        {
            buttonText = upgradeButton.transform.Find("DreamEssenceCost")?.GetComponent<TextMeshProUGUI>();
        } 
        if (upgradeCostText == null)
        {
            upgradeCostText = transform.Find("Cost")?.GetComponent<TextMeshProUGUI>();
        } 

        if (string.IsNullOrEmpty(upgradeID))
        {
            Debug.LogError($"{name}: upgradeID is empty. Set it in the inspector.");
            gameObject.SetActive(false);
            return;
        }

        if (UpgradeManager.Instance == null)
        {
            Debug.LogError($"{name}: UpgradeManager.Instance is null.");
            return;
        }

        linkedUpgrade = UpgradeManager.Instance.GetUpgrade<BaseUpgrade>(upgradeID);

        if (linkedUpgrade == null)
        {
            Debug.LogError($"{name}: No upgrade found for ID '{upgradeID}'");
            gameObject.SetActive(false);
            return;
        }

        // add listener once
        if (upgradeButton != null)
        {
            upgradeButton.onClick.RemoveAllListeners();
            upgradeButton.onClick.AddListener(TryBuying);
        }

        RefreshUI();
    }

    public void RefreshUI()
    {
        if (linkedUpgrade == null) return;

        // Title & description
        if (upgradeTitleText != null)
        {
            upgradeTitleText.text = linkedUpgrade.upgradeName;    
        } 
        if (upgradeDescriptionText != null) upgradeDescriptionText.text = linkedUpgrade.upgradeDescription;

        // Stage / tier display e.g. "Tier 2 / 5"
        if (upgradeStageText != null)
        {
            upgradeStageText.text = $"{linkedUpgrade.upgradeTier+1} / {linkedUpgrade.maxUpgradeTier+1}";
        }

        // If maxed
        bool isMax = linkedUpgrade.upgradeTier >= linkedUpgrade.maxUpgradeTier;

        if (isMax)
        {
            // show MAX instead of cost
            if (upgradeCostText != null)
            {
                upgradeCostText.text = "MAX";
            } 
            if (buttonText != null)
            {
                buttonText.text = "MAXED";
            } 
            if (upgradeButton != null)
            {
                upgradeButton.interactable = false;
            } 
        }
        else
        {
            // Show next tier number and cost properly (no modulo)
            int nextTier = linkedUpgrade.upgradeTier + 1;
            if (upgradeCostText != null)
            {
                upgradeCostText.text = $"Next: {nextTier}";
            } 
            if (buttonText != null)
            {
                buttonText.text = linkedUpgrade.dreamEssenceCostToNextTier.ToString();
            } 

            // Interactable depends on player's currency and skill slots
            if (upgradeButton != null)
            {
                // Condition 1: Player has enough currency
                bool canAfford = PlayerDataManager.Instance != null &&
                                PlayerDataManager.Instance.DreamEssence >= linkedUpgrade.dreamEssenceCostToNextTier;

                // Condition 2: Player has spare skill slot
                bool hasSpareSkillSlots = SkillManager.Instance.currentUsedSkillSlots <
                                        SkillManager.Instance.maxSkillTreeSlots;

                // Final
                upgradeButton.interactable = canAfford && hasSpareSkillSlots;
            }
        }
    }

    public void TryBuying()
    {
        if (linkedUpgrade == null) return;

        // Use UpgradeManager to handle validation, cost, saving, etc.
        bool success = UpgradeManager.Instance.TryUpgrade(upgradeID);

        if (success)
        {
            // upgrade succeeded; refresh this button
            RefreshUI();
            PlayerDataManager.Instance.RecalculateFinalStats();
            // optionally refresh whole shop so other buttons that depend on currency update
            // e.g. tell ShopManager to call RefreshAllUpgradeButtons()
            if (ShopManager.Instance != null) ShopManager.Instance.RefreshAllUpgradeButtons();
        }
        else
        {
            // you could show a "not enough" feedback here
            // Debug.Log("Purchase failed.");
        }
    }
}
