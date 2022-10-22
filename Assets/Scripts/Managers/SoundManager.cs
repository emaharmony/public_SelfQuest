using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager INSTANCE;

    [SerializeField] AudioSource sfx, bgm;
    [SerializeField] AudioClip questComplete, music;
    [SerializeField] TMPro.TMP_Text text;
    [SerializeField] AudioClip morn, evening, night, lateNight;

    public bool musicON = false;

    private void Awake()
    {
        INSTANCE = this;    
    }

    private void Start()
    {
        sfx.loop = false;
        sfx.playOnAwake = false;
        sfx.volume = 0.2f;
        bgm.volume = 0.5f;
        bgm.loop = true;
        bgm.playOnAwake = true;
    }
    public void FinishQuest()
    { 
        sfx.clip = questComplete;
        sfx.Play();
    }

    public void VolumeAdjust(float f)
    {
        sfx.volume = bgm.volume = f/10;
        text.text = f + "";
    }
    public void Morning()
    {
        musicON = true;
        bgm.clip = morn;
        bgm.Play();
    }

    public void Evening()
    {
        musicON = true;

        bgm.clip = evening;
        bgm.Play();
    }

    public void Night()
    {
        musicON = true;
        bgm.clip = night;
        bgm.Play();
    }

    public void LateNight()
    {
        musicON = true;
        bgm.clip = lateNight;
        bgm.Play();
    }
}
