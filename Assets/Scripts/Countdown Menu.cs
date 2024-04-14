using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CountdownMenu : Menu {
    public Sprite countdown1;
    public Sprite countdown2;
    public Sprite countdown3;
    public Sprite countdownGo;
    private Image timerImage;


    public void SetCountdownImage(int second) {
        switch (second) {
            case 0:
                timerImage.sprite = countdownGo;
                break;
            case 1:
                timerImage.sprite = countdown1;
                break;
            case 2:
                timerImage.sprite = countdown2;
                break;
            case 3:
                timerImage.sprite = countdown3;
                break;
            default:
                timerImage.sprite = null;
                break;
        }
    }
    public void Awake()
    {
        base.Awake();
        timerImage = GetComponent<Image>();
    }



    public void Start()
    {
        base.Start();
    }

}
