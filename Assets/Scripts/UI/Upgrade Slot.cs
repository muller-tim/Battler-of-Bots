using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UpgradeSlot : MonoBehaviour
{
    public Image icon;
    public Image nameIcon;
    public TMP_Text title;
    public TMP_Text description;
    public Image background;

    public UnityEvent UpgradeSelectedEvent;
    public UnityEvent UpgradeDeselectedEvent;

    private void Start()
    {
        background = GetComponent<Image>();
    }



}
