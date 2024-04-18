using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    #region Public Properties

    /// <summary>
    /// Direction the Projectile flies at the start. Needs to be set
    /// </summary>
    public Vector3 Direction { get; set; }

    #endregion

    #region Unity Methods

    private void Awake() {
        m_rigidbody = GetComponent<Rigidbody>();
        m_explosive = GetComponent<Explosive>();
    }

    private void Start() {
        Debug.Assert(Direction != Vector3.zero, "Direction of Projectile not set");

        // Play Audio
        if(m_shootAudioClip)
            AudioSource.PlayClipAtPoint(m_shootAudioClip, transform.position);

        // Shoot the projectile forwards
        m_rigidbody.AddForce(Direction * m_speed, ForceMode.Impulse);
    }

    private void Update() {
        // Handle Life Time
        m_maxLifeTime -= Time.deltaTime;

        if (m_maxLifeTime <= 0.0f) {
            DestroyProjectile();
        }
    }

    private void OnCollisionEnter(Collision otherCollision) {
        m_currentCollisionCount++;

        if (otherCollision.gameObject.CompareTag("Enemy")) {
            if (otherCollision.gameObject.TryGetComponent<HealthComponent>(out HealthComponent healthComponent)) {
                healthComponent.TakeDamage(m_damage);
            }
            else {
                Debug.LogError($"{otherCollision.gameObject.name} does not have a HealtComponent");
            }
        }

        if (m_currentCollisionCount >= m_maxCollisions) {
            DestroyProjectile();
        }
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// Plays <see cref="m_collisionParticles"/> and <see cref="m_collisionAudio"/>. The Projectile gets Destroyed after.
    /// If the gameObject has an Explosive Script, it will explode instead and the ExplosiveScript takes care of the Audio and Particles.
    /// </summary>
    private void DestroyProjectile() {
        if (m_explosive) {
            m_explosive.Explode();
            return;
        }

        if (m_collisionParticles) {
            m_collisionParticles.DetachPlay();
        }
        
        if(m_collisionAudio.clip)
            AudioSource.PlayClipAtPoint(m_collisionAudio.clip, transform.position);

        Destroy(gameObject);
    }

    #endregion

    #region private Member

    /// <summary>
    /// Speed of the Projectile
    /// </summary>
    [SerializeField] private float m_speed;

    /// <summary>
    /// Damage of the Projectile
    /// </summary>
    [SerializeField] private float m_damage;

    /// <summary>
    /// The time the projectile is in the scene after it gets destroyed
    /// </summary>
    [SerializeField] private float m_maxLifeTime;

    /// <summary>
    /// The amount of collisions the projectile needs to be destroyed
    /// </summary>
    [SerializeField] private int m_maxCollisions = 1;

    /// <summary>
    /// Particle System that is played when <see cref="m_currentCollisionCount"/> reaches <see cref="m_maxCollisions"/>
    /// </summary>
    [SerializeField] private ParticleSystem m_collisionParticles;

    /// <summary>
    /// Audio that is played when <see cref="m_currentCollisionCount"/> reaches <see cref="m_maxCollisions"/>
    /// </summary>
    [SerializeField] private AudioSource m_collisionAudio;

    /// <summary>
    /// Audio clip that is played at spawn of the projectile
    /// </summary>
    [SerializeField] private AudioClip m_shootAudioClip;

    /// <summary>
    /// Rigidbody of the gameObject
    /// </summary>
    private Rigidbody m_rigidbody;

    /// <summary>
    /// Explosive Script of the gameObject
    /// </summary>
    private Explosive m_explosive;

    /// <summary>
    /// Counts how many collisions occured to the projectile
    /// </summary>
    private int m_currentCollisionCount = 0;

    #endregion
}
