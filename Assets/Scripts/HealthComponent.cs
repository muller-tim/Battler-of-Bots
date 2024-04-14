using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthComponent : MonoBehaviour
{
    [SerializeField, Range(0, 10000)] private float m_MaxHealth = 100.0f;
    public float m_currentHealth;
    public bool m_isDead = false;
    private bool m_isInvincible = false;

    public event Action OnDeath;
    public event Action OnHealthChange;

    public float MaxHealth => m_MaxHealth;
    public AudioClip damageSound;
    public AudioClip deathSound;
    
    public float Health {
        get => m_currentHealth;
        private set {
            m_currentHealth = Mathf.Clamp(value, 0, m_MaxHealth);
            OnHealthChange?.Invoke();
        }
    }

    public bool Invincible {
        get => m_isInvincible;
        set => m_isInvincible = value;
    }

    void Awake() {
        m_currentHealth = m_MaxHealth;
    }

    [ContextMenu("Kill")]
    public void Kill() {
        TakeDamage(m_MaxHealth);
    }

    [ContextMenu("Take Damage")]
    private void TakeSomeDamage()
    {
        TakeDamage(20);
        Debug.LogError(Health);
    }

    public void TakeDamage(float damage) {
        Debug.Log("Enemy got damaged");
        if (!m_isInvincible) {
            if(Health - damage > 0 && damageSound != null)
                AudioSource.PlayClipAtPoint(damageSound, transform.position);
            Health -= damage;
        }

        if (Health <= 0 && !m_isDead) {
            if(deathSound != null)
                AudioSource.PlayClipAtPoint(deathSound, transform.position);
            m_isDead = true;
            OnDeath?.Invoke();
        }
    }

    public void Heal(float amount) {
        if (!m_isDead) {
            Health += amount;
        }
    }

    public bool IsAlive() {
        return !m_isDead;
    }
}
