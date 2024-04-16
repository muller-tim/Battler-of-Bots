using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ControlsSplash : Menu
{
    public Button continueButton;
    public RoundSystem roundSystem;


    public void Awake()
    {
        base.Awake();
        continueButton.onClick.AddListener(OnContinueButtonClicked);

    }

    public void Start()
    {
        base.Start();
        roundSystem = FindObjectOfType<RoundSystem>();
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
        // roundSystem.StartNextRound();
        roundSystem.StartCoroutine(roundSystem.StartCountdown());
    }
    private void OnContinueButtonClicked()
    {
        MenuManager.ShowMenu("Game HUD");
    }
}
