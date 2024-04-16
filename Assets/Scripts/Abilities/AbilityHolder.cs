using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class that manages the abilities of the script-holder
/// </summary>
[RequireComponent(typeof(InputReader))]
public class AbilityHolder : MonoBehaviour {
    #region Public properties
    /// <summary>
    /// Public readonly property for the current abilities
    /// </summary>
    public AbilityBase[] Abilities => m_abilities;

    /// <summary>
    /// Public readonly property for the ability origins/locations
    /// </summary>
    public Transform[] AbilityLocations => m_abilityLocations;

    /// <summary>
    /// Public readonly property for the AimAssist location
    /// </summary>
    public Transform AimAssistLocation => m_aimAssistLocation;

    /// <summary>
    /// Current Ability cooldowns
    /// </summary>
    public float[] Cooldowns => m_cooldowns;
    #endregion

    #region Events

    /// <summary>
    /// Event when a cooldown value is updated
    /// </summary>
    public event Action<float[]> OnCooldownUpdate;

    #endregion

    #region Public Methods

    /// <summary>
    /// Replaces the current ability in the same slot as the parameter with the parameter
    /// </summary>
    /// <param name="ability"></param>
    public void UpgradeAbility(AbilityBase ability)
    {
        m_abilities[(int)ability.AbilitySlot] = ability;

        // Change Visuals Here
    }
    #endregion

    #region Unity Methods
    void Start()
    {
        m_inputReader = GetComponent<InputReader>();

        // Initialize cooldowns with 0
        for (int i = 0; i < m_cooldowns.Length; i++)
        {
            m_cooldowns[i] = 0.0f;
        }
    }

    void Update() {
        // State machine
        switch (m_currentState)
        {
            case AbilityState.Waiting:
                // Check Inputs and Cooldowns
                if (m_inputReader.StandardAttack && m_cooldowns[(int)AbilitySlotIndexName.NormalAttack] == 0.0f)
                {
                    m_activeAbility = (int)AbilitySlotIndexName.NormalAttack;
                }

                if (m_inputReader.HeavyAttack && m_cooldowns[(int)AbilitySlotIndexName.HeavyAttack] == 0.0f)
                {
                    m_activeAbility = (int)AbilitySlotIndexName.HeavyAttack;
                }

                if (m_inputReader.AOEAttack && m_cooldowns[(int)AbilitySlotIndexName.AOEAttack] == 0.0f)
                {
                    m_activeAbility = (int)AbilitySlotIndexName.AOEAttack;
                }

                if (m_inputReader.ShortDistanceAttack && m_cooldowns[(int)AbilitySlotIndexName.ShortDistanceAttack] == 0.0f)
                {
                    m_activeAbility = (int)AbilitySlotIndexName.ShortDistanceAttack;
                }

                if (m_inputReader.Dash && m_cooldowns[(int)AbilitySlotIndexName.Dash] == 0.0f)
                {
                    m_activeAbility = (int)AbilitySlotIndexName.Dash;
                }

                // Switch State if an ability was used
                if (m_activeAbility != null && m_abilities[(int)m_activeAbility])
                {
                    // Get activeTime and activate ability
                    m_activeTime = m_abilities[(int)m_activeAbility].ActiveTime;
                    m_abilities[(int)m_activeAbility].Activate(gameObject);

                    m_currentState = AbilityState.Active;

                    // Start Ability Animation here?
                }


                break;
            case AbilityState.Active:
                m_activeTime -= Time.deltaTime;

                // Switch state when current ability is done
                if (m_activeTime <= 0.0f)
                {
                    // Start ability cooldown
                    if (m_activeAbility != null)
                        m_cooldowns[(int)m_activeAbility] = m_abilities[(int)m_activeAbility].Cooldown;

                    m_activeAbility = null;
                    m_currentState = AbilityState.Waiting;
                }
                break;
            default:
                Debug.Log("AbilityHolder State machine Error");
                break;
        }

        UpdateCooldowns();
    }

    #endregion

    #region Editor Methods

    void OnValidate()
    {
        if (m_abilities.Length != 5)
        {
            Debug.LogError("Don't change the ability slot size");
            AbilityBase[] newAbilities = new AbilityBase[5];

            for (int i = 0; i < m_abilities.Length; i++)
            {
                if (i < newAbilities.Length)
                {
                    newAbilities[i] = m_abilities[i];
                }
            }

            m_abilities = newAbilities;
        }
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// Updates the Cooldowns and calls OnCooldownUpdate Event
    /// </summary>
    private void UpdateCooldowns() {
        bool updated = false;
        // Cooldowns update 
        for (int i = 0; i < m_cooldowns.Length; i++)
        {
            if (m_abilities[i] && m_cooldowns[i] != 0.0) {
                m_cooldowns[i] = Mathf.Clamp(m_cooldowns[i] - Time.deltaTime, 0.0f, m_abilities[i].Cooldown);
                updated = true;
            }
        }

        if(updated) OnCooldownUpdate?.Invoke(Cooldowns);
    }

    #endregion

    #region Private members
    /// <summary>
    /// Current abilities of the holder
    /// </summary>
    [SerializeField] private AbilityBase[] m_abilities = new AbilityBase[5];

    /// <summary>
    /// Origins of the abilities. For example the muzzle of a gun
    /// </summary>
    [SerializeField] private Transform[] m_abilityLocations = new Transform[5];

    /// <summary>
    /// Position the aim assist starts from
    /// </summary>
    [SerializeField] private Transform m_aimAssistLocation;

    /// <summary>
    /// Current cooldowns of the ability slots
    /// </summary>
    private float[] m_cooldowns = new float[5];

    /// <summary>
    /// Current Ability State -> Only one ability can be active at a time
    /// </summary>
    private AbilityState m_currentState = AbilityState.Waiting;

    /// <summary>
    /// Input Reader of the Player
    /// </summary>
    private InputReader m_inputReader;

    /// <summary>
    /// Current active time. The ability usage state is active as long as the active time is not 0.0
    /// </summary>
    private float m_activeTime = 0.0f;

    /// <summary>
    /// Index of the slot that is currently used. Null if waiting for an ability
    /// </summary>
    private int? m_activeAbility = null;

    #endregion

}

#region Enums

/// <summary>
/// Waiting defines that an ability can be used.
/// Active defines that an ability is currently in use
/// </summary>
enum AbilityState
{
    Waiting,
    Active
}

/// <summary>
/// Names of the ability slots
/// </summary>
public enum AbilitySlotIndexName
{
    NormalAttack = 0,
    HeavyAttack = 1,
    AOEAttack = 2,
    ShortDistanceAttack = 3,
    Dash = 4
}

#endregion

