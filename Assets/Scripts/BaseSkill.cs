using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public abstract class BaseSkill : MonoBehaviour
{
    [Header("General Info")]
    public string skillName;
    public string skillID;
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
    private Coroutine cooldownRoutine;

    public Image cooldownOverlay; // 1 = can't use, 0 = ready to use

    protected void InitializeButton()
    {
        skillButton = gameObject.GetComponent<Button>();
        if (skillButton != null)
        {
            cooldownOverlay = skillButton.transform.Find("CooldownFill").GetComponent<Image>();
            cooldownOverlay.fillAmount = 0f; // Start as ready
        }
        else
        {
            Debug.LogError("Skill Button component not found on " + skillName);
        }
    }

    public void ToggleSkill()
    {
        Debug.Log("Toggling skill: " + skillName);
        if(skillCoroutine != null)
        {
            StopSkill();
        } 
        else
        {
            if (PlayerDataManager.Instance.SoulEssence >= soulEssenceCost)
            {   
                AudioManager.Instance.PlaySFXOnce(4);
                PlayerDataManager.Instance.ApplySkillMultiplier(skillID, skillEffectMultiplier);
                skillCoroutine = StartCoroutine(DrainSoulEssence(baseDuration));
                if (cooldownRoutine != null)
                StopCoroutine(cooldownRoutine);

                cooldownRoutine = StartCoroutine(CooldownOverlayLoop(baseDuration));
            } 
            else
            {
                Debug.Log("Not enough Soul Essence to activate skill: " + skillName);
            }
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
        PlayerDataManager.Instance.playerHUD.ShowInsufficientSoulEssenceToSpend(soulEssenceCost, PlayerDataManager.Instance.SoulEssence);
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

        if (cooldownRoutine != null)
        {
            StopCoroutine(cooldownRoutine);
            cooldownRoutine = null;
        }

        cooldownOverlay.fillAmount = 0f; // Reset visually
    }

    

    private IEnumerator CooldownOverlayLoop(float duration)
    {
        cooldownOverlay.fillAmount = 0f;   // visually ready at start

        while (true)
        {
            float t = 0f;

            // Animate from 0 → 1 over the tick duration
            while (t < duration)
            {
                t += Time.deltaTime;
                cooldownOverlay.fillAmount = t / duration;
                yield return null;
            }

            cooldownOverlay.fillAmount = 0f;
            yield return null;
        }
    }
}