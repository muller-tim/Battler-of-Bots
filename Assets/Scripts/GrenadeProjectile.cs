using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeProjectile : MonoBehaviour
{
    [SerializeField] private float m_speed;
    [SerializeField] private float m_maxLifeTime;
    [SerializeField] private float m_maxCollisions;



    private Rigidbody m_rb;
    private SphereCollider m_collider;
    private GrenadeExplosion m_grenade;
    private int m_collisions;

    public Vector3 Direction { get; set; } = Vector3.forward;


    void OnCollisionEnter(Collision otherCollision) {
        m_collisions++;
    }

    void Start()
    {
        //AudioSource.PlayClipAtPoint(m_shotAudioClip, transform.position);
        m_rb = GetComponent<Rigidbody>();
        m_collider = GetComponent<SphereCollider>();
        m_grenade = GetComponentInChildren<GrenadeExplosion>();

        m_rb.AddForce(Direction * m_speed + Vector3.up * 5.0f, ForceMode.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        if (m_collisions >= m_maxCollisions) {
            m_grenade.Explode();
        }
    }
}
