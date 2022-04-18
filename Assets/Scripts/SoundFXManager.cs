using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundFXManager : MonoBehaviour
{
    [SerializeField] GameObject musicManager = null;
    [SerializeField] GameObject audioSourcePrefab = null;
    [SerializeField] int amountToCreate = 6;

    List<AudioSource> audioSources = new List<AudioSource>();

    ProjectileSFX projectileSFX = null;

    bool canPlayAudio = false;

    private void Awake()
    {
        projectileSFX = GetComponent<ProjectileSFX>();
    }

    private void Start()
    {
        CreateAudioSources();
    }

    private void CreateAudioSources()
    {
        for (int i = 0; i < amountToCreate; i++)
        {
            GameObject asInstance = Instantiate(audioSourcePrefab, transform);
            AudioSource audioSource = asInstance.GetComponent<AudioSource>();

            audioSources.Add(audioSource);
        }
    }

    public void PlayAudioClip(Sound sound)
    {
        if (!canPlayAudio) return;
        SoundFX soundFX = projectileSFX.GetSoundFX(sound);
        AudioSource activeAudioSource = GetActiveAudioSource();

        activeAudioSource.volume = soundFX.GetVolume();
        activeAudioSource.clip = soundFX.GetAudioClip();
        activeAudioSource.Play();
        StartCoroutine(BeginReset(activeAudioSource));
    }

    private IEnumerator BeginReset(AudioSource audioSource)
    {
        float clipLength = audioSource.clip.length;

        yield return new WaitForSeconds(clipLength);

        audioSource.clip = null;
    }

    private AudioSource GetActiveAudioSource()
    {
        AudioSource activeAudioSource = null;

        foreach(AudioSource audioSource in audioSources)
        {
            if (audioSource.clip == null)
            {
                activeAudioSource = audioSource;
            }
        }

        return activeAudioSource;
    }

    public void ActivateSounds(bool shouldActivate)
    {
        musicManager.SetActive(shouldActivate);
        canPlayAudio = shouldActivate;
    }
}
