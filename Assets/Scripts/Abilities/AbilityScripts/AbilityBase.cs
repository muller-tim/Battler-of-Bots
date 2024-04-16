using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Base ScriptableObject Class for Abilities.
/// Contains the information of an ability an the method to use the ability
/// </summary>
public class AbilityBase : ScriptableObject
{
    #region Public Member

    /// <summary>
    /// Icon for the name of the ability
    /// </summary>
    public Sprite NameSprite;

    /// <summary>
    /// Text Description of the ability
    /// </summary>
    [TextArea] public string Description;

    /// <summary>
    /// Sprite for the description of the ability
    /// </summary>
    public Sprite DescriptionSprite;

    /// <summary>
    /// Cooldown of the ability. The cooldown starts at the End of the ActiveTime
    /// </summary>
    public float Cooldown;

    /// <summary>
    /// The time the ability is activated or the time the ability needs to be finished
    /// </summary>
    public float ActiveTime;

    /// <summary>
    /// Contains the available upgrades for the current ability
    /// </summary>
    public AbilityBase[] Upgrades;

    /// <summary>
    /// Animation that will be played when the ability is used
    /// </summary>
    public Animation Animation;

    /// <summary>
    /// Icon of the Ability that will be shown to the Player
    /// </summary>
    public Sprite AbilityIcon;

    /// <summary>
    /// The ability slot the ability is used in. The slot defines which buttons are used to use the ability 
    /// </summary>
    public AbilitySlotIndexName AbilitySlot;

    #endregion

    #region Public Methods

    /// <summary>
    /// Method that activates the ability and starts the ActiveTime.
    /// </summary>
    /// <param name="parent">Parent should be the GameObject that calls the function. For example the Player</param>
    public virtual void Activate(GameObject parent) { }

    #endregion

}
