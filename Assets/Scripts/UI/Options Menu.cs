using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OptionsMenu : Menu
{

    public Button backToMainMenuButton;
    public Button continueButton;
    public TMP_Text endTimer;
    

    public void Awake()
    {
        base.Awake();

        backToMainMenuButton.onClick.AddListener(BackToMainMenuButtonClicked);
        continueButton.onClick.AddListener(OnContinueButtonClicked);

    }

    public void Start()
    {
        base.Start();
    }

    public override void OnMenuDidAppear()
    {
        base.OnMenuDidAppear();
        Time.timeScale = 0.0f;      
    }
    public override void OnMenuWillDisappear()
    {
        base.OnMenuWillDisappear();
        Time.timeScale = 1.0f;
    }
    private void OnContinueButtonClicked()
    {
        MenuManager.ShowMenu("Game HUD");
    }
    private void BackToMainMenuButtonClicked()
    {
        SceneManager.LoadScene("Menu Scene");
    }
}
