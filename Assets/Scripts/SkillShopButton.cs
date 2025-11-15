using UnityEngine;
using UnityEngine.UI;

public class SkillShopButton : MonoBehaviour
{
    [Header("Link")]
    public string skillID; // fill manually
    private BaseSkill linkedSkill;

    [Header("UI")]
    public Button unlockButton;
    public Button lockButton;
    public GameObject inGameSkillButton;

    void Start()
    {
        SkillManager.Instance.RegisterShopButton(this);
        AutoAssign();
        RefreshButtonState();
    }

    void AutoAssign()
    {
        // 1. Get BaseSkill from SkillManager
        linkedSkill = SkillManager.Instance.GetSkillByID(skillID);

        if (linkedSkill == null)
        {
            Debug.LogError($"SkillShopButton: No BaseSkill found with ID '{skillID}'");
            return;
        }

        // 2. Auto-assign UI buttons
        unlockButton = transform.Find("Unlock")?.GetComponent<Button>();
        lockButton = transform.Find("Lock")?.GetComponent<Button>();

        if (unlockButton == null || lockButton == null)
        {
            Debug.LogWarning($"SkillShopButton: Missing unlock/lock Button components under {name}");
        }

        // 3. Auto-assign in-game skill button
        inGameSkillButton = linkedSkill.gameObject;

        // 4. AUTO-ASSIGN button click events
        if (unlockButton != null)
            unlockButton.onClick.AddListener(OnUnlockPressed);

        if (lockButton != null)
            lockButton.onClick.AddListener(OnLockPressed);
    }

    public void OnUnlockPressed()
    {
        int before = SkillManager.Instance.currentUsedSkillSlots;

        SkillManager.Instance.UnlockSkill(skillID);

        // if nothing changed, unlock failed (no slot)
        if (before == SkillManager.Instance.currentUsedSkillSlots)
        {
            Debug.Log("Shop: Cannot unlock, no available skill slots.");
            return;
        }

        RefreshButtonState();
    }

    public void OnLockPressed()
    {
        SkillManager.Instance.LockSkill(skillID);
        RefreshButtonState();
    }

    public void RefreshButtonState()
    {
        bool isUnlocked = SkillManager.Instance.unlockedSkillIDs.Contains(skillID);
        bool slotsAvailable = SkillManager.Instance.currentUsedSkillSlots < SkillManager.Instance.maxSkillTreeSlots;

        // Unlock button logic
        if (unlockButton != null)
        {
            // Show unlock button only if:
            // - skill is currently locked
            // - AND there are free slots available
            unlockButton.gameObject.SetActive(!isUnlocked && slotsAvailable);
        }

        // Lock button logic
        if (lockButton != null)
        {
            // Lock button only shows for unlocked skills
            lockButton.gameObject.SetActive(isUnlocked);
        }

        // In-game skill button active only if unlocked
        if (inGameSkillButton != null)
            inGameSkillButton.SetActive(isUnlocked);
    }

}