using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SoundClip
{
    Hurt,
    Place,
    Portal,
    GetItem,
    BloodyPunch,
    Bullet14,
    Bullet21,
    Metal17,
    MonsterBite
}
public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    [SerializeField] private AudioSource musicSource, effectSource;
    [SerializeField] private AudioClip[] clips = new AudioClip[3];

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(this);
            return;
        }
    }

    public void PlaySound(AudioClip clip)
    {
        effectSource.PlayOneShot(clip);
    }
    public void PlaySound(SoundClip s)
    {
        PlaySound(clips[(int)s]);
    }
    public void ChangeEffectVolume(float value){
        effectSource.GetComponent<AudioSource>().volume=value;
    }
    public void ChangeMusicVolume(float value){
        musicSource.GetComponent<AudioSource>().volume=value;
    }
}
