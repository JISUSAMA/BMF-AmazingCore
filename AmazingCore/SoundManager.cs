using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public AudioMixer audioMixer;
    public AudioSource BGM;
    public AudioSource SFX;

    public float BGM_volume;
    public float SFX_volume;

    public AudioClip[] Bgm_clip;
    public AudioClip[] Sfx_clip;

    public Dictionary<string, AudioClip> bgm_lookupTable = new Dictionary<string, AudioClip>();
    public Dictionary<string, AudioClip> sfx_lookupTable = new Dictionary<string, AudioClip>();

    public static SoundManager instance { get; private set; }
    private void Awake()
    {
        if (instance != null) Destroy(this);
        else instance = this;

        DontDestroyOnLoad(this);
    }
    private void Start()
    {
        //설정된 볼륨의 크기 , 설정창에서 음량 조절 가능
        if (PlayerPrefs.HasKey("BGM_Volum"))
        {
            BGM_volume = PlayerPrefs.GetFloat("BGM_Volum");
            SFX_volume = PlayerPrefs.GetFloat("SFX_Volum");

            //audioMixer.BGM.volume = BGM_volume;
            //audioMixer.volume = SFX_volume;
        }
        else
        {
            PlayerPrefs.SetFloat("BGM_Volum", 1);
            PlayerPrefs.SetFloat("SFX_Volum", 1);
        }
        //Sf_clip 안에있는 AudioClip를 sfx_lookupTable 안에 넣어줌
        foreach (AudioClip clip in Sfx_clip)
        {
            sfx_lookupTable.Add(clip.name, clip);
        }
        foreach (AudioClip clip in Bgm_clip)
        {
            bgm_lookupTable.Add(clip.name, clip);
        }
    }
    public AudioClip Sfx_GetClipByName(string soundName)
    {
        return sfx_lookupTable[soundName];
    }
    public void PlaySFX_OneShot(string soundName, float volume = 1f)
    {
        SFX.PlayOneShot(Sfx_GetClipByName(soundName), volume);
    }
    public AudioClip Bgm_GetClipByName(string soundName)
    {
        return bgm_lookupTable[soundName];
    }
    public void PlayBGM(string clipName, float volume = 1f)
    {
        if (BGM.isPlaying && BGM.clip.name.Equals(clipName))
            return;
        else
        {
            //Debug.LogError("게임 종료후 다시 !/???");
            BGM.clip = Bgm_GetClipByName(clipName);
            BGM.Play();
        }
    }
}