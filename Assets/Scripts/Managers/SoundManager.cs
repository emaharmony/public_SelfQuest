using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager INSTANCE;

    [SerializeField] AudioSource sfx, bgm;
    [SerializeField] AudioClip questComplete, music;
    [SerializeField] TMPro.TMP_Text text;

    private void Awake()
    {
        INSTANCE = this;    
    }

    private void Start()
    {
        sfx.loop = false;
        sfx.playOnAwake = false;
        sfx.volume = 0.2f;
        bgm.clip = music;
        bgm.volume = 0.05f;
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
}
