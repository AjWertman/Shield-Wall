using UnityEngine;

public enum Sound { Woosh, TakePotion, ShieldBlock, GlassBreak, HoWPickup, RockHit, LevelUp}

[System.Serializable]
public class SoundFX
{
    [SerializeField] Sound sound = Sound.Woosh;
    [SerializeField] AudioClip audioClip = null;
    [Range(0,1)][SerializeField] float volume = 1;

    public Sound GetSound()
    {
        return sound;
    }

    public AudioClip GetAudioClip()
    {
        return audioClip;
    }

    public float GetVolume()
    {
        return volume;
    }
}

public class ProjectileSFX : MonoBehaviour
{
    [SerializeField] SoundFX[] soundFXes = null;

    public SoundFX GetSoundFX(Sound sound)
    {
        SoundFX soundFX = null;
        foreach(SoundFX _soundFX in soundFXes)
        {
            if(_soundFX.GetSound() == sound)
            {
                soundFX = _soundFX;
            }
        }

        return soundFX;
    }
}
