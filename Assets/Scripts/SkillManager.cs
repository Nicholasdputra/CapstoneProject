using UnityEngine;
using System.Collections.Generic;

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

    public int maxSkillTreeSlots;

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

        BuildSkillDictionary();
        FindSkillObjectsInScene();  
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
        BaseSkill[] skillsInScene = FindObjectsOfType<BaseSkill>();
        skillObjectsInScene.AddRange(skillsInScene);
    }


    private void BuildSkillDictionary()
    {
        skillDict.Clear();

        foreach (BaseSkill skill in skillObjectsInScene)
        {
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
        if (!unlockedSkillIDs.Contains(skillID))
        {
            unlockedSkillIDs.Add(skillID);
            RefreshLockedSkills();
            ApplySkillActivationState();
        }
    }
}
