using UnityEngine;
using UnityEngine.UI;

public class SettingsScript : MonoBehaviour
{   
    public Slider bgmSlider;
    public Slider sfxSlider;
    public GameObject settingsPanel;

    void Start()
    {
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
        if (settingsPanel != null)
            settingsPanel.SetActive(true);
    }

    // Close settings panel
    public void CloseSettings()
    {
        if (settingsPanel != null)
            settingsPanel.SetActive(false);
    }

    // BGM Slider On Value Changed
    public void SetBGMVolume(float volume)
    {
        float bgmVolume = bgmSlider.value;
        PlayerPrefs.SetFloat("BGMVolume", bgmVolume);
        PlayerPrefs.Save();

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.DetermineVolume(AudioManager.Instance.FindCurrentlyPlayingClip());
        }
    }

    // SFX Slider On Value Changed
    public void SetSFXVolume(float volume)
    {
        float sfxVolume = sfxSlider.value;
        PlayerPrefs.SetFloat("SFXVolume", sfxVolume);
        PlayerPrefs.Save();

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.DetermineVolume(AudioManager.Instance.FindCurrentlyPlayingClip());
        }
    }

    
}