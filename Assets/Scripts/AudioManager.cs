using UnityEngine;
using System.Collections;

[System.Serializable]
public class ModifiedAudioClips
{
    [Header("Clip Settings")]
    public AudioClip clip;
    public float startTime = 0f;
    public float endTime = -1f; // -1 means play till the end
    public float clipVolumeMultiplier = 1f;

    [HideInInspector] public AudioSource audioSource;
    private Coroutine playCoroutine;

    public void Initialize(AudioSource source)
    {
        audioSource = source;
    }

    // Play the clip once
    public void PlayOnce()
    {
        if (playCoroutine != null)
        {
            AudioManager.Instance.StopCoroutine(playCoroutine);
        }
        playCoroutine = AudioManager.Instance.StartCoroutine(PlayClipCoroutine(1));
    }

    // Loop the clip a specified number of times
    public void PlayTimes(int times)
    {
        if (playCoroutine != null)
        {
            AudioManager.Instance.StopCoroutine(playCoroutine);
        }
        playCoroutine = AudioManager.Instance.StartCoroutine(PlayClipCoroutine(times));
    }

    // Plays the clip in an infinite loop
    public void PlayLoop()
    {
        if (playCoroutine != null)
        {
            AudioManager.Instance.StopCoroutine(playCoroutine);
        }
        playCoroutine = AudioManager.Instance.StartCoroutine(PlayClipCoroutine(-1)); // -1 for infinite loop
    }

    public void Stop()
    {
        if (playCoroutine != null)
        {
            AudioManager.Instance.StopCoroutine(playCoroutine);
            playCoroutine = null;
        }
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }

    private IEnumerator PlayClipCoroutine(int repeatCount)
    {
        // Check to see if the audiosource it wants to play from exists and check to see if the clip exists
        if (clip == null || audioSource == null)
        {
            yield break;
        }

        // Determine how long to play the clip
        float actualEndTime;
        if (endTime < 0)
        {
            // Play to the end of the clip
            actualEndTime = clip.length;
        }
        else
        {
            // Play to the specified end time, but don't exceed clip length
            actualEndTime = Mathf.Min(endTime, clip.length);
        }
        float duration = actualEndTime - startTime;

        // If duration is zero or negative, exit coroutine
        if (duration <= 0) yield break;
        
        // Loop to play the clip the specified number of times
        int playedTimes = 0;        
        while (repeatCount == -1 || playedTimes < repeatCount) // -1 means infinite loop
        {
            // Set the clip and volume
            audioSource.clip = clip;
            
            // Start playing from the specified start time
            audioSource.time = startTime;
            audioSource.Play();
            
            // Wait for the duration between start and end time
            yield return new WaitForSeconds(duration);
            
            // Stop the audio
            audioSource.Stop();
            
            playedTimes++;
            
            // Small delay between repeats if looping for infinte times or if there's still loops left
            if (repeatCount != 1 && (repeatCount == -1 || playedTimes < repeatCount))
            {
                yield return new WaitForSeconds(0.1f);
            }
        }

        playCoroutine = null;
    }
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public AudioSource bgmSource;
    public AudioSource sfxSource;

    public ModifiedAudioClips[] bgmClips;
    public ModifiedAudioClips[] sfxClips;

    private const string BGM_VOLUME_KEY = "BGMVolume";
    private const string SFX_VOLUME_KEY = "SFXVolume";

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeClips();
        }
        else
        {
            Destroy(gameObject);
        }

        Initialize();
    }

    void Initialize()
    {
        // Find child object named "BGMSource" and get its AudioSource component
        if (bgmSource == null)
        {
            Transform bgmTransform = transform.Find("BGMSource");
            if (bgmTransform != null)
            {
                bgmSource = bgmTransform.GetComponent<AudioSource>();
            }
        }
        
        // Find child object named "SFXSource" and get its AudioSource component
        if (sfxSource == null)
        {
            Transform sfxTransform = transform.Find("SFXSource");
            if (sfxTransform != null)
            {
                sfxSource = sfxTransform.GetComponent<AudioSource>();
            }
        }
    }

    // Assign audio sources to each clip
    void InitializeClips()
    {
        foreach (var bgm in bgmClips)
        {
            bgm.Initialize(bgmSource);
        }
        foreach (var sfx in sfxClips)
        {
            sfx.Initialize(sfxSource);
        }
    }

    // Set BGM volume
    public void SetBGMVolume(float volume)
    {
        bgmSource.volume = volume;
    }

    // Set SFX volume
    public void SetSFXVolume(float volume)
    {
        sfxSource.volume = volume;
    }

    // Determine final volume to play the clip at
    public void DetermineVolume(ModifiedAudioClips clip)
    {
        if (clip == null)
        {
            return;
        }
        float audioSourceMultiplier;

        if (clip.audioSource == bgmSource)
        {
            audioSourceMultiplier = PlayerPrefs.GetFloat(BGM_VOLUME_KEY, 1.0f);
        }
        else
        {
            audioSourceMultiplier = PlayerPrefs.GetFloat(SFX_VOLUME_KEY, 1.0f);
        }

        float finalVolume = clip.clipVolumeMultiplier * audioSourceMultiplier;

        if (clip.audioSource != null)
        {
            if (clip.audioSource == bgmSource)
            {
                bgmSource.volume = finalVolume;
            }
            else
            {
                sfxSource.volume = finalVolume;
            }
        }
    }

    // Play a BGM clip once
    public void PlayBGMOnce(int index)
    {
        if (index >= 0 && index < bgmClips.Length)
        {
            DetermineVolume(bgmClips[index]);
            bgmClips[index].PlayOnce();
        }
    }

    // Play a BGM clip a specified number of times
    public void PlayBGMTimes(int index, int times)
    {
        if (index >= 0 && index < bgmClips.Length)
        {
            DetermineVolume(bgmClips[index]);
            bgmClips[index].PlayTimes(times);
        }
    }

    // Play a BGM clip in a loop
    public void PlayBGMLoop(int index)
    {
        if (index >= 0 && index < bgmClips.Length)
        {
            DetermineVolume(bgmClips[index]);
            bgmClips[index].PlayLoop();
        }
    }

    // Play a SFX clip once
    public void PlaySFXOnce(int index)
    {
        if (index >= 0 && index < sfxClips.Length)
        {
            DetermineVolume(sfxClips[index]);
            sfxClips[index].PlayOnce();
        }
    }

    // Play a SFX clip a specified number of times
    public void PlaySFXTimes(int index, int times)
    {
        if (index >= 0 && index < sfxClips.Length)
        {
            DetermineVolume(sfxClips[index]);
            sfxClips[index].PlayTimes(times);
        }
    }

    // Play a SFX clip in a loop
    public void PlaySFXLoop(int index)
    {
        if (index >= 0 && index < sfxClips.Length)
        {
            DetermineVolume(sfxClips[index]);
            sfxClips[index].PlayLoop();
        }
    }

    // Find and return the currently playing modified clip based on the audioclip in the audio source
    public ModifiedAudioClips FindCurrentlyPlayingClip()
    {
        // Check BGM clips
        foreach (var bgm in bgmClips)
        {
            if (bgm.audioSource.isPlaying && bgm.audioSource.clip == bgm.clip)
            {
                return bgm;
            }
        }

        // Check SFX clips
        foreach (var sfx in sfxClips)
        {
            if (sfx.audioSource.isPlaying && sfx.audioSource.clip == sfx.clip)
            {
                return sfx;
            }
        }

        // Debug.Log("No clip is currently playing.");
        return null; 

    }
}