using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SoundManager
{
    public enum Sound
    {
        BtnClick,
        BtnHover,
    }
    
    public static void PlaySound(Sound sound)
    {
        GameObject soundGameObject = new("Sound");
        AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();
        audioSource.PlayOneShot(GetAudioClip(sound));
    }

    private static AudioClip GetAudioClip(Sound sound)
    {
        foreach (MainMenu.SoundAudioClip soundAudioClip in MainMenu.i.soundAudioClipArray)
        {
            if(soundAudioClip.sound == sound)
            {
                return soundAudioClip.audioClip;
            }
        }
        Debug.Log("Erro! Som '" + sound + "' não encontrado.");
        return null;
    }
}
