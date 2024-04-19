using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class KinematicController : MonoBehaviour {

    #region Public Methods

    /// <summary>
    /// Enables Kinematic on the rigidbody of the gameObject
    /// </summary>
    public void EnableKinematic()
    {
        m_rigidbody.isKinematic = true;
    }

    /// <summary>
    /// Disables Kinematic on the rigidbody of the gameObject and start the timers <see cref="m_minTimer"/> and <see cref="m_maxTimer"/>
    /// </summary>
    public void DisableKinematic()
    {
        m_minTimer = m_minDisabledTime;
        m_maxTimer = m_maxDisabledTime;
        m_rigidbody.isKinematic = false;
    }

    #endregion

    #region Unity Methods

    void Start()
    {
        m_rigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (m_autoEnable && m_minTimer <= 0.0f && !m_rigidbody.isKinematic && m_rigidbody.velocity.magnitude < m_velocityThreshold)
        {
            EnableKinematic();
        }

        if (m_autoEnable && m_maxTimer <= 0.0f && !m_rigidbody.isKinematic)
        {
            EnableKinematic();
        }

        if (m_minTimer > 0.0f)
        {
            m_minTimer -= Time.deltaTime;
        }

        if (m_maxTimer > 0.0f)
        {
            m_maxTimer -= Time.deltaTime;
        }
    }

    #endregion

    #region Private Member

    /// <summary>
    /// Determines if kinematic should automatically enable when the velocity reaches <see cref="m_velocityThreshold"/> and  <see cref="m_minTimer"/> reaches 0.
    /// Or when <see cref="m_maxTimer"/> reaches 0
    /// </summary>
    [SerializeField] private bool m_autoEnable = true;

    /// <summary>
    /// The threshold the velocity of the rigidbody needs to reach to re-enable kinematic
    /// </summary>
    [SerializeField] private float m_velocityThreshold = 1.0f;

    /// <summary>
    /// The minimum time until kinematic can be re-enabled
    /// </summary>
    [SerializeField] private float m_minDisabledTime = 0.5f;

    /// <summary>
    /// The maximum time kinematic can be disabled;
    /// </summary>
    [SerializeField] private float m_maxDisabledTime = 2.0f;

    /// <summary>
    /// The timer for <see cref="m_minDisabledTime"/>
    /// </summary>
    private float m_minTimer;

    /// <summary>
    /// The timer for <see cref="m_maxDisabledTime"/>
    /// </summary>
    private float m_maxTimer;

    /// <summary>
    /// The rigidbody of the gameObject
    /// </summary>
    private Rigidbody m_rigidbody;

    #endregion
}
