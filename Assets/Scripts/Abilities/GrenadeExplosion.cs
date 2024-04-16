using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeExplosion : MonoBehaviour {
    [SerializeField] private ParticleSystem m_explosion;
    [SerializeField] private AudioSource m_explosionAudioSource;
    [SerializeField] private AudioClip m_shotAudioClip;

    [SerializeField] private float m_damage;
    [SerializeField] private float m_explosionForce;
    [SerializeField] private float m_explosionRange;
    [SerializeField] private LayerMask m_layer;

    private bool exploded;

    public void Explode()
    {
        if (exploded) return;
        exploded = true;

        if (m_explosion != null)
        {
            //Instantiate(m_explosion, transform.position, Quaternion.identity);
            m_explosion.transform.parent = null;
            m_explosion.Play();
            AudioSource.PlayClipAtPoint(m_explosionAudioSource.clip, transform.position);

        }

        Collider[] enemies = Physics.OverlapSphere(transform.position, m_explosionRange, m_layer);

        for (int i = 0; i < enemies.Length; i++)
        {
            if (enemies[i].GetComponent<HealthComponent>())
            {
                enemies[i].GetComponent<HealthComponent>().TakeDamage(m_damage);
                // Debug.LogError($"Damage {i}: {m_damage}, Health: {enemies[i].GetComponent<HealthComponent>().Health}");
            }


            Rigidbody enemyRB = enemies[i].GetComponent<Rigidbody>();
            KinematicController kc = enemies[i].GetComponent<KinematicController>();

            if (enemyRB)
            {
                kc.DisableKinematic();
                enemyRB.AddExplosionForce(m_explosionForce, transform.position, m_explosionRange);
            }

        }

        Destroy(gameObject.transform.parent.gameObject);
    }

    void Start() {
        AudioSource.PlayClipAtPoint(m_shotAudioClip, transform.position);
    }
}
