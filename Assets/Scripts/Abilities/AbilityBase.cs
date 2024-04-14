using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityBase : ScriptableObject
{
    public Sprite nameIcon;
    [TextArea] public string description;
    public float cooldown;
    public float activeTime;
    public AbilityBase[] Upgrades;
    public Animation animation;
    public Sprite abilityIcon;
    public Sprite descriptionIcon;
    public AbilitySpecifier AbilitySlot;
    public virtual void Activate(GameObject parent) {}
}
