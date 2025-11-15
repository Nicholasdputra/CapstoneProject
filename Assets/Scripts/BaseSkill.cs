using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public abstract class BaseSkill : MonoBehaviour
{
    [Header("General Info")]
    public string skillName;
    public string skillID;
    public Sprite skillIcon;
    public Button skillButton;
    public string skillDescription;
    
    [Header("Unlock Info")]
    public int dreamEssenceUnlockCost;

    [Header("Skill Info")]
    public int soulEssenceCost;
    public float baseDuration;
    public float baseMultiplier;
    public float skillEffectMultiplier;
    public Coroutine skillCoroutine;

    public void ToggleSkill()
    {
        if(skillCoroutine != null)
        {
            StopSkill();
        } 
        else
        {
            PlayerDataManager.Instance.ApplySkillMultiplier(skillID, skillEffectMultiplier);
            skillCoroutine = StartCoroutine(DrainSoulEssence(baseDuration));
        }
    }

    public virtual IEnumerator DrainSoulEssence(float currentDuration)
    {
        while (PlayerDataManager.Instance.SoulEssence >= soulEssenceCost)
        {
            if (currentDuration == baseDuration)
            {
                PlayerDataManager.Instance.SpendSoulEssence(soulEssenceCost);
                Debug.Log("Draining Soul Essence for skill: " + skillName);
            }

            yield return new WaitForSeconds(currentDuration);
            currentDuration = baseDuration;
        }
        StopSkill();
    }

    public void StopSkill()
    {
        if (skillCoroutine != null)
        {
            StopCoroutine(skillCoroutine);
            skillCoroutine = null;
            PlayerDataManager.Instance.RemoveSkillMultiplier(skillID);
        }
    }
}