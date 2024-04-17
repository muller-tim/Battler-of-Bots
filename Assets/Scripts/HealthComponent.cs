using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthComponent : MonoBehaviour
{
    #region Public Properties

    /// <summary>
    /// Defines the maximum health of the owner
    /// </summary>
    public float MaxHealth => m_maxHealth;

    /// <summary>
    /// Returns if the owner is alive
    /// </summary>
    public bool IsAlive => !m_isDead;

    /// <summary>
    /// Current Health of the owner
    /// </summary>
    public float Health
    {
        get => m_currentHealth;
        private set
        {
            m_currentHealth = Mathf.Clamp(value, 0, m_maxHealth);
            OnHealthChange?.Invoke();
        }
    }

    /// <summary>
    /// Defines if the owner is currently invincible
    /// </summary>
    public bool Invincible
    {
        get => m_isInvincible;
        set => m_isInvincible = value;
    }

    #endregion

    #region Public Events

    /// <summary>
    /// Event is called when <see cref="Health"/> of the owner is 0
    /// </summary>
    public event Action OnDeath;

    /// <summary>
    /// Event is called when <see cref="Health"/> changes in any way 
    /// </summary>
    public event Action OnHealthChange;

    #endregion

    #region Public Methods

    /// <summary>
    /// <see cref="Health"/> is reduced by <paramref name="damage"/> if owner is not invincible
    /// </summary>
    /// <param name="damage"></param>
    public void TakeDamage(float damage)
    {
        if (!m_isInvincible)
        {
            // Damage Sound
            if (Health - damage > 0 && m_damageSound != null)
                AudioSource.PlayClipAtPoint(m_damageSound, transform.position);

            Health -= damage;
        }

        if (Health <= 0 && !m_isDead)
        {
            // Deathsound
            if (m_deathSound != null)
                AudioSource.PlayClipAtPoint(m_deathSound, transform.position);

            m_isDead = true;
            OnDeath?.Invoke();
        }
    }

    /// <summary>
    /// Increases <see cref="Health"/> by <paramref name="amount"/> if the owner is not dead
    /// </summary>
    /// <param name="amount"></param>
    public void Heal(float amount)
    {
        if (!m_isDead)
        {
            Health += amount;
        }
    }

    /// <summary>
    /// Owner takes <see cref="m_maxHealth"/> damage and dies
    /// </summary>
    [ContextMenu("Kill")]
    public void Kill()
    {
        TakeDamage(m_maxHealth);
    }

    #endregion

    #region Unity Methods

    void Awake()
    {
        m_currentHealth = m_maxHealth;
    }

    #endregion

    #region Private Methods

    [ContextMenu("Take Damage"), Tooltip("Deal 20 Damage")]
    private void TakeSomeDamage()
    {
        TakeDamage(20);
        Debug.LogError(Health);
    }

    #endregion

    #region Private Member

    /// <summary>
    /// Defines the maximum health
    /// </summary>
    [SerializeField, Range(0, 10000)] private float m_maxHealth = 100.0f;

    /// <summary>
    /// Sound is played when taking damage
    /// </summary>
    [SerializeField] private AudioClip m_damageSound;

    /// <summary>
    /// Sound is played when the owner dies
    /// </summary>
    [SerializeField] private AudioClip m_deathSound;

    /// <summary>
    /// Current health of the owner
    /// </summary>
    private float m_currentHealth;

    /// <summary>
    /// Current state if the owner is dead or alive
    /// </summary>
    private bool m_isDead = false;

    /// <summary>
    /// Defines if the owner can take damage
    /// </summary>
    private bool m_isInvincible = false;

    #endregion
}
