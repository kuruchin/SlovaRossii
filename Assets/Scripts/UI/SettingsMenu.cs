using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SettingsMenu : MonoBehaviour
{
    private bool menuShow;
    [SerializeField]
    private GameObject menu;

    public AudioMixer AudioMixer;
    private bool isBGMMuted;

    public void Start()
    {
        menuShow = false;
        isBGMMuted = false;
    }

    public void Update()
    {
        menu.SetActive(menuShow);
    }

    public void OnClick_SettingsMenuShow()
    {
        menuShow = !menuShow;
    }

    public void OnClick_BGMMute()
    {
        isBGMMuted = !isBGMMuted;

        if(isBGMMuted)
            AudioMixer.SetFloat("BGMVolume", -80f);
        else
            AudioMixer.SetFloat("BGMVolume", 0f);
    }
}
