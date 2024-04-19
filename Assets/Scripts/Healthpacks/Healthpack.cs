using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Healthpack : MonoBehaviour {

    #region Public Event

    /// <summary>
    /// Event that is fired when the healthpack is picked up
    /// </summary>
    public event Action OnPickup;

    #endregion

    #region Unity Methods

    void OnTriggerEnter(Collider otherCollision)
    {
        // Player and healtComponent check
        if (otherCollision.gameObject.CompareTag("Player") && otherCollision.gameObject.TryGetComponent<HealthComponent>(out HealthComponent healthComponent) && !m_wasPickedUp)
        {
            // Only Heal when not full life
            if (healthComponent.Health < healthComponent.MaxHealth)
            {
                healthComponent.Heal(m_healAmount);
                m_wasPickedUp = true;
                OnPickup?.Invoke();
                Destroy(gameObject);
            }
        }
    }

    #endregion



    #region Private Member

    /// <summary>
    /// The amount the player is healed by the healthpack
    /// </summary>
    [SerializeField] private float m_healAmount = 25.0f;

    /// <summary>
    /// Determines if the healthpack was already picked up
    /// </summary>
    private bool m_wasPickedUp;

    

    #endregion

}
