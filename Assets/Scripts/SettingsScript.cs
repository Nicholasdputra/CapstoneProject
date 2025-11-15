using UnityEngine;
using UnityEngine.UI;

public class SettingsScript : MonoBehaviour
{   
    public Slider bgmSlider;
    public Slider sfxSlider;
    public GameObject settingsPanel;

    void Start()
    {
        if (bgmSlider == null || sfxSlider == null)
        {
            Debug.LogError("BGM or SFX Slider is not assigned in the inspector.");
            return;
        }
        
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(false);
        }

        LoadSettings();
    }

    // Load saved settings
    public void LoadSettings()
    {
        // Load BGM volume (default to 1.0 if no saved value)
        float savedBGMVolume = PlayerPrefs.GetFloat("BGMVolume", 1.0f);
        bgmSlider.value = savedBGMVolume;

        // Load SFX volume (default to 1.0 if no saved value)
        float savedSFXVolume = PlayerPrefs.GetFloat("SFXVolume", 1.0f);
        sfxSlider.value = savedSFXVolume;
    }

    // Open settings panel
    public void OpenSettings()
    {
        AudioManager.Instance.PlaySFXOnce(0);
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(true);
        }
    }

    // Close settings panel
    public void CloseSettings()
    {
        AudioManager.Instance.PlaySFXOnce(0);
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(false);
        }
    }

    // BGM Slider On Value Changed
    public void SetBGMVolume()
    {
        // Debug.Log("Slider.value directly: " + bgmSlider.value);
        // Debug.Log("Slider min: " + bgmSlider.minValue + ", max: " + bgmSlider.maxValue);
        float bgmVolume = bgmSlider.value;
        PlayerPrefs.SetFloat("BGMVolume", bgmVolume);
        PlayerPrefs.Save();
        // Debug.Log("BGM Volume set to: " + PlayerPrefs.GetFloat("BGMVolume"));
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.DetermineVolume(AudioManager.Instance.FindCurrentlyPlayingClip());
        }
    }

    // SFX Slider On Value Changed
    public void SetSFXVolume()
    {
        // Debug.Log("Slider.value directly: " + sfxSlider.value);
        // Debug.Log("Slider min: " + sfxSlider.minValue + ", max: " + sfxSlider.maxValue);
        float sfxVolume = sfxSlider.value;
        PlayerPrefs.SetFloat("SFXVolume", sfxVolume);
        PlayerPrefs.Save();
        // Debug.Log("SFX Volume set to: " + PlayerPrefs.GetFloat("SFXVolume"));

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.DetermineVolume(AudioManager.Instance.FindCurrentlyPlayingClip());
        }
    }
}