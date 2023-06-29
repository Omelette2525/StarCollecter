using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SoundSliderController : MonoBehaviour
{
    //複雑なコード。詳しくはhttps://yumeoimushi.hatenablog.com/entry/2021/02/09/234812を参照
    public AudioMixer AudioMixer;
    public Slider masterSlider;
    public Slider bgmSlider;
    public Slider seSlider;

    void Start()
    {
        float volume;
        if (AudioMixer.GetFloat("MASTER", out volume))
        {
            masterSlider.value = ConvertDbToVolume(volume);
        }
        //Debug.Log(volume);
        if (AudioMixer.GetFloat("BGM", out volume))
        {
            bgmSlider.value = ConvertDbToVolume(volume);
        }
        if (AudioMixer.GetFloat("SE", out volume))
        {
            seSlider.value = ConvertDbToVolume(volume);
        }
    }

    public float ConvertVolumeToDb(float volume)
    {
        return Mathf.Clamp(Mathf.Log10(Mathf.Clamp(volume, 0f, 1f)) * 20f, -80f, 0f);
    }
    public float ConvertDbToVolume(float db)
    {
        return Mathf.Pow(10f, db / 20f);
    }
    public void SetMasterVolume(float volume)
    {
        AudioMixer.SetFloat("MASTER", ConvertVolumeToDb(masterSlider.value));
    }
    public void SetBGMVolume(float volume)
    {
        AudioMixer.SetFloat("BGM", ConvertVolumeToDb(bgmSlider.value));
    }
    public void SetSEVolume(float volume)
    {
        AudioMixer.SetFloat("SE", ConvertVolumeToDb(seSlider.value));
    }
}
