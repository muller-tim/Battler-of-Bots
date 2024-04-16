using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDMenu : Menu
{
    [Range(0, 1)]
    public float healthFill = 1.0f;

    [Range(0, 1)]
    public float dashFill = 1.0f;

    [Range(0, 1)]
    public float cheerFill = 1.0f;

    public Slider healthBar;
    public Slider cheerBar;
    public Slider dashBar;
    public TMP_Text dashCounter;


    [Header("Skills")]

    public Slider fillIndicatorNormal;
    public Slider fillIndicatorHeavy;
    public Slider fillIndicatorShortDistance;
    public Slider fillIndicatorAOE;

    public Image skillIconNormal;
    public Image skillIconHeavy;
    public Image skillIconShortDistance;
    public Image skillIconAOE;

    [Range(0, 1)]
    public float cooldownNormal;
    [Range(0, 1)]
    public float cooldownHeavy;
    [Range(0, 1)]
    public float cooldownShortDistance;
    [Range(0, 1)] 
    public float cooldownAOE;

    public TMP_Text timer;
    public TMP_Text round;


    public void Awake()
    {
        base.Awake();


    }


    public void Start()
    {
        base.Start();
    }
    public void Update()
    {
        healthBar.value = healthFill;
        dashBar.value = dashFill;
        cheerBar.value = cheerFill;

        fillIndicatorShortDistance.value = cooldownShortDistance;
        fillIndicatorHeavy.value = cooldownHeavy;
        fillIndicatorNormal.value = cooldownNormal;
        fillIndicatorAOE.value = cooldownAOE;

    }
}
