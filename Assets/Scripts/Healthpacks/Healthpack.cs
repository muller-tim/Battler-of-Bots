using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Healthpack : MonoBehaviour {
    [SerializeField] private float m_healAmount = 25.0f;
    private bool wasPickedUp;

    private event Action OnPickup;


    void OnTriggerEnter(Collider triggerCollider) {
        if (triggerCollider.gameObject.CompareTag("Player") && triggerCollider.gameObject.TryGetComponent<HealthComponent>(out HealthComponent healthComponent) && !wasPickedUp) {
            if (healthComponent.Health < healthComponent.MaxHealth) {
                healthComponent.Heal(m_healAmount);
                wasPickedUp = true;
                OnPickup?.Invoke();
                Destroy(gameObject);
            }
            
        }
    }

}
