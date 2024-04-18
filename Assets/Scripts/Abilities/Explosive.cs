using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosive : MonoBehaviour
{
    #region Public Methods

    /// <summary>
    /// Plays <see cref="m_explosionParticleSystem"/> and <see cref="m_explosionAudio"/>.
    /// Knockbacks and damages enemies in <see cref="m_explosionRange"/>
    /// </summary>
    public void Explode() {
        // Particle System
        if (m_explosionParticleSystem) {
            m_explosionParticleSystem.DetachPlay();
        }

        // Audio
        if (m_explosionAudio) {
            AudioSource.PlayClipAtPoint(m_explosionAudio.clip, transform.position);
        }

        Collider[] enemies = Physics.OverlapSphere(transform.position, m_explosionRange, m_layer);

        for (int i = 0; i < enemies.Length; i++)
        {
            // Damage enemies
            if (enemies[i].TryGetComponent<HealthComponent>(out HealthComponent healthComponent))
            {
                healthComponent.TakeDamage(m_explosionDamage);
            }
            else {
                Debug.LogError($"{enemies[i].name} does not have a HealthComponent");
            }

            // Knockback
            if (enemies[i].TryGetComponent<Rigidbody>(out Rigidbody rigidbody) && enemies[i].TryGetComponent<KinematicController>(out KinematicController kinematicController)) {
                kinematicController.DisableKinematic();
                rigidbody.AddExplosionForce(m_explosionForce, transform.position, m_explosionRange);
            }
            else {
                Debug.LogError($"{enemies[i].name} does not have a rigidbody or a KinematicController");
            }

        }

        Destroy(gameObject);
    }

    #endregion

    #region Unity Methods

    private void Update() {
        if (m_isProjectile) return; // Let projectile script take care

        // Timed Explosion
        if (m_explodeAfterTimer) {
            m_explosionTimer -= Time.deltaTime;

            if (m_explosionTimer <= 0.0f)
            {
                Explode();
            }
        }
    }

    private void OnCollisionEnter(Collision otherCollision) {
        // Explode on touch
        if (otherCollision.gameObject.CompareTag("Enemy") && m_explodeOnTouch) {
            Explode();
        }

        if (m_isProjectile) return; // Let projectile script take care of collision
    }

    private void OnValidate() {

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, m_explosionRange);
    }

    #endregion

    #region Private Member

    /// <summary>
    /// The force the enemies are knockbacked with
    /// </summary>
    [SerializeField] private float m_explosionForce;

    /// <summary>
    /// The damage of the explosion
    /// </summary>
    [SerializeField] private float m_explosionDamage;

    /// <summary>
    /// The Explosion Radius
    /// </summary>
    [SerializeField] private float m_explosionRange;

    /// <summary>
    /// The layer which is hit by the explosion
    /// </summary>
    [SerializeField] private LayerMask m_layer;

    /// <summary>
    /// Determines if the explosive is a projectile. The projectile takes over some explosion decisions
    /// </summary>
    [SerializeField] private bool m_isProjectile;

    /// <summary>
    /// The explosive explodes on collision with enemies. 
    /// </summary>
    [SerializeField] private bool m_explodeOnTouch;

    /// <summary>
    /// Determines if the explosive should explode after a specific time
    /// </summary>
    [SerializeField] private bool m_explodeAfterTimer;

    /// <summary>
    /// The time the explosive needs to explode
    /// </summary>
    [SerializeField] private float m_explosionTimer;
    
    /// <summary>
    /// The particly system which is played when the gameObject explodes
    /// </summary>
    [SerializeField] private ParticleSystem m_explosionParticleSystem;

    /// <summary>
    /// The Audio which is play when the gameObject explodes
    /// </summary>
    [SerializeField] private AudioSource m_explosionAudio;
    #endregion
}
