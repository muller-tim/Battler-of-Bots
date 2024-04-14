using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ControlsMenu : Menu
{

    public Button backToMainMenuButton;

    public Slider volumeSlider;


    public void Awake()
    {
        base.Awake();


        backToMainMenuButton.onClick.AddListener(BackToMainMenuButtonClicked);
        volumeSlider?.onValueChanged.AddListener(OnVolumeSliderValueChanged);
    }

    public void changeVolume(float newVolume)
    {
        PlayerPrefs.SetFloat("volume", newVolume);
        AudioListener.volume = PlayerPrefs.GetFloat("volume");
    }


    private void OnVolumeSliderValueChanged(float arg0)
    {
        changeVolume(arg0);
    }

    public void Start()
    {
        base.Start();
    }

    private void BackToMainMenuButtonClicked()
    {
        MenuManager.ShowMenu("Main Menu");
    }
}
