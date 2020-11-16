using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.EventSystems;

public class AudioController : MonoBehaviour
{

    public GameObject mAudioManagerUI;
    public AudioMixer audioMixer;  // 进行控制的Mixer变量
    [Range(0.001f, 1f)]
    public float mainVolumeWhenMenuOpen = 0.5f;
    [Range(0.001f, 1f)]
    public float musicVolumeWhenMenuOpen = 0.5f;
    [Range(0.001f, 1f)]
    public float effectVolumeWhenMenuOpen = 0.5f;
    public Slider mainVolumeSlider;
    public Slider musicVolumeSlider;
    public Slider effectVolumeSlider;
    // Start is called before the first frame update
    void Start()
    {
        mAudioManagerUI.SetActive(false);
        mainVolumeSlider.onValueChanged.AddListener(SetMasterVolume);
        musicVolumeSlider.onValueChanged.AddListener(SetMusicVolume);
        effectVolumeSlider.onValueChanged.AddListener(SetSoundEffectVolume);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            SetPauseMenuActivation(!mAudioManagerUI.activeSelf);
        }
    }
    private void SetPauseMenuActivation(bool active)
    {
        mAudioManagerUI.SetActive(active);
        if (mAudioManagerUI.activeSelf)
        {
            Time.timeScale = 0f;
            EventSystem.current.SetSelectedGameObject(null);
        }
        else
        {
            Time.timeScale = 1f;
        }
    }
    public void SetMasterVolume(float volume)    // 控制主音量的函数
    {
        audioMixer.SetFloat("MasterVolume", SwitchVolume2Stander(volume));
        // MasterVolume为我们暴露出来的Master的参数
    }

    public void SetMusicVolume(float volume)    // 控制背景音乐音量的函数
    {
        audioMixer.SetFloat("MusicVolume", SwitchVolume2Stander(volume));
        // MusicVolume为我们暴露出来的Music的参数
    }

    public void SetSoundEffectVolume(float volume)    // 控制音效音量的函数
    {
        audioMixer.SetFloat("SoundEffectVolume", SwitchVolume2Stander(volume));
        // SoundEffectVolume为我们暴露出来的SoundEffect的参数

    }

    private float SwitchVolume2Stander(float oringeVolume)
    {
        float elevation = 0;
        elevation = Mathf.Clamp(elevation + (1 - oringeVolume) * 100 - 80, -80, 20);
        return elevation;
    }
}