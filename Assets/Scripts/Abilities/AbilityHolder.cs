using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InputReader))]
public class AbilityHolder : MonoBehaviour {
    [SerializeField] private AbilityBase[] m_abilities = new AbilityBase[5];
    [SerializeField] private Transform[] m_abilityLocations = new Transform[5];
    [SerializeField] private Transform m_aimLocation;
    private float[] m_cooldowns = new float[5];
    private AbilityState m_currentState = AbilityState.Waiting;
    private InputReader m_inputReader;
    private float m_activeTime = 0.0f;
    private int? m_activeAbility = null;

    public AbilityBase[] Abilities => m_abilities;
    public Transform[] AbilityLocations => m_abilityLocations;
    public Transform AimLocation => m_aimLocation;
    public float[] Cooldowns => m_cooldowns;
    public event Action<float[]> OnCooldownUpdate;

    public void UpgradeAbility(AbilityBase ability) {
        m_abilities[(int)ability.AbilitySlot] = ability;
        // Change Visuals Here
    }

    void Start() {
        m_inputReader = GetComponent<InputReader>();

        for (int i = 0; i < m_cooldowns.Length; i++) {
            m_cooldowns[i] = 0.0f;
        }
    }

    void Update() {

        switch (m_currentState) {
            case AbilityState.Waiting:
                // Check Inputs and Cooldowns
                if (m_inputReader.StandardAttack && m_cooldowns[(int) AbilitySpecifier.NormalAttack] == 0.0f)
                {
                    m_activeAbility = (int)AbilitySpecifier.NormalAttack;
                }

                if (m_inputReader.HeavyAttack && m_cooldowns[(int)AbilitySpecifier.HeavyAttack] == 0.0f) 
                {
                    m_activeAbility = (int)AbilitySpecifier.HeavyAttack;
                }

                if (m_inputReader.AOEAttack && m_cooldowns[(int)AbilitySpecifier.AOEAttack] == 0.0f)
                {
                    m_activeAbility = (int)AbilitySpecifier.AOEAttack;
                }

                if (m_inputReader.ShortDistanceAttack && m_cooldowns[(int)AbilitySpecifier.ShortDistanceAttack] == 0.0f)
                {
                    m_activeAbility = (int)AbilitySpecifier.ShortDistanceAttack;
                }

                if (m_inputReader.Dash && m_cooldowns[(int)AbilitySpecifier.Dash] == 0.0f)
                {
                    m_activeAbility = (int)AbilitySpecifier.Dash;
                }

                // Switch State if ability was used
                if (m_activeAbility != null && m_abilities[(int)m_activeAbility]) {
                    m_activeTime = m_abilities[(int)m_activeAbility].activeTime;
                    m_currentState = AbilityState.Active;
                    m_abilities[(int)m_activeAbility].Activate(gameObject);
                    // Debug.LogError(m_abilities[(int)m_activeAbility].abilityName);
                    
                    // Start Animation here?
                }
                

                break;
            case AbilityState.Active:
                m_activeTime -= Time.deltaTime;

                // Switch state when current ability is done
                if (m_activeTime <= 0.0f) {
                    if (m_activeAbility != null)
                        m_cooldowns[(int)m_activeAbility] = m_abilities[(int)m_activeAbility].cooldown;
                    m_activeAbility = null;
                    m_currentState = AbilityState.Waiting;
                }
                break;
            default:
                Debug.Log("AbilityHolder State machine Error");
                break;
        }

        for (int i = 0; i < m_cooldowns.Length; i++) {
            if(m_abilities[i])
                m_cooldowns[i] = Mathf.Clamp(m_cooldowns[i] - Time.deltaTime, 0, m_abilities[i].cooldown);
        }

        OnCooldownUpdate?.Invoke(Cooldowns);
    }

    void OnValidate() {
        if (m_abilities.Length != 5) {
            Debug.LogError("Don't change the ability slot size");
            AbilityBase[] newAbilities = new AbilityBase[5];

            for (int i = 0; i < m_abilities.Length; i++) {
                if (i < newAbilities.Length) {
                    newAbilities[i] = m_abilities[i];
                }
            }

            m_abilities = newAbilities;
        }
    }
}

enum AbilityState {
    Waiting,
    Active
}

public enum AbilitySpecifier {
    NormalAttack = 0,
    HeavyAttack = 1,
    AOEAttack = 2,
    ShortDistanceAttack = 3,
    Dash = 4
}
