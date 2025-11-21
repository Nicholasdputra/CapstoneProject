using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class SkillManager : MonoBehaviour
{
    public static SkillManager Instance;

    [Header("Skill IDs")]
    public List<string> allSkillIDs = new List<string>();
    public List<string> unlockedSkillIDs = new List<string>();
    public List<string> lockedSkillIDs = new List<string>();

    [Header("Skill Objects In-Scene")]
    public List<BaseSkill> skillObjectsInScene = new List<BaseSkill>();
    private Dictionary<string, BaseSkill> skillDict = new Dictionary<string, BaseSkill>();

    [Header("Shop Buttons")]
    public List<SkillShopButton> allShopButtons = new List<SkillShopButton>();

    public int maxSkillTreeSlots;
    public int currentUsedSkillSlots = 0;

    public TextMeshProUGUI skillSlotsText;

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

        FindSkillObjectsInScene();  
        BuildSkillDictionary();
    }

    public void UpdateSkillSlotsUI()
    {
        if (skillSlotsText != null)
        {
            skillSlotsText.text = $"{currentUsedSkillSlots} / {maxSkillTreeSlots}";
        }
    }

    public void RefreshAllShopButtons()
    {
        foreach (var btn in allShopButtons)
            if (btn != null)
                btn.RefreshButtonState();
    }


    public void RegisterShopButton(SkillShopButton btn)
    {
        if (!allShopButtons.Contains(btn))
            allShopButtons.Add(btn);
    }


    private void Start()
    {
        // Make this triggered by an event after loading player data instead of at start
        RefreshLockedSkills(); 
        ApplySkillActivationState();
    }

    private void FindSkillObjectsInScene()
    {
        skillObjectsInScene.Clear();
        GameObject HUDCanvas = GameObject.Find("HUDCanvas");
        if (HUDCanvas == null)
        {
            Debug.LogError("SkillManager: Could not find HUDCanvas.");
            return;
        }
        Transform skillButtons = HUDCanvas.transform.Find("SkillButtons");
        if (skillButtons == null)
        {
            Debug.LogError("SkillManager: Could not find SkillButtons under HUDCanvas.");
            return;
        }


        // includeInactive = true ensures components on disabled GameObjects are found
        BaseSkill[] skillsInScene = skillButtons.GetComponentsInChildren<BaseSkill>(true);
        skillObjectsInScene.AddRange(skillsInScene);
        // Debug.Log($"SkillManager: Found {skillsInScene.Length} BaseSkill components in scene.");
    }


    private void BuildSkillDictionary()
    {
        skillDict.Clear();

        if (skillObjectsInScene.Count == 0)
        {
            Debug.LogWarning("No skills found in scene!");
        } 
        else
        {
            Debug.Log($"Found {skillObjectsInScene.Count} skills in scene.");

            // 1. Log each skill's ID
            foreach (BaseSkill skill in skillObjectsInScene)
            {
                Debug.Log($"Skill found: {skill.skillID}");
            }
        }

        foreach (BaseSkill skill in skillObjectsInScene)
        {
            Debug.Log($"Registering skill ID: {skill.skillID}");
            if (!skillDict.ContainsKey(skill.skillID))
            {
                skillDict.Add(skill.skillID, skill);
                if (!allSkillIDs.Contains(skill.skillID))
                {
                    allSkillIDs.Add(skill.skillID);
                }
            }
        }
    }

    public void RefreshLockedSkills()
    {
        lockedSkillIDs.Clear();

        foreach (string id in allSkillIDs)
        {
            if (!unlockedSkillIDs.Contains(id))
            {
                lockedSkillIDs.Add(id);
            }
        }
    }

    public void ApplySkillActivationState()
    {
        foreach (var pair in skillDict)
        {
            bool isUnlocked = unlockedSkillIDs.Contains(pair.Key);
            pair.Value.gameObject.SetActive(isUnlocked);
        }
    }

    public void UnlockSkill(string skillID)
    {
        if (unlockedSkillIDs.Contains(skillID))
            return;

        if (currentUsedSkillSlots >= maxSkillTreeSlots)
        {
            Debug.Log("SkillManager: Cannot unlock. No skill slots available.");
            return;
        }

        unlockedSkillIDs.Add(skillID);
        currentUsedSkillSlots = unlockedSkillIDs.Count;

        RefreshLockedSkills();
        ApplySkillActivationState();
        UpdateSkillSlotsUI();

        RefreshAllShopButtons();
    }


    public void LockSkill(string skillID)
    {
        if (!unlockedSkillIDs.Contains(skillID))
            return;

        unlockedSkillIDs.Remove(skillID);
        currentUsedSkillSlots = unlockedSkillIDs.Count;

        RefreshLockedSkills();
        ApplySkillActivationState();
        UpdateSkillSlotsUI();

        RefreshAllShopButtons();
    }

    public BaseSkill GetSkillByID(string id)
    {
        if (skillDict.TryGetValue(id, out BaseSkill skill))
            return skill;

        Debug.LogWarning($"SkillManager: No skill found with ID: {id}");
        return null;
    }

}