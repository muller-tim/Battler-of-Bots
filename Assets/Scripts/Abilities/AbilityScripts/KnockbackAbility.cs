using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

[CreateAssetMenu(menuName = "Abilities/Knockback")]
public class KnockbackAbility : AbilityBase {
    #region Public Methods

    /// <summary>
    /// Knockbacks the enemies in a cone area in front of the ability holder
    /// </summary>
    /// <param name="parent"><inheritdoc cref="AbilityBase.Activate"/></param>
    public override void Activate(GameObject parent)
    {
        if (m_particleSystem)
        {
            m_particleSystem.Play();
        }

        parent.GetComponent<ParticleSystemExecutor>().StartPushBackParticleSystem();


        AbilityHolder abilityHolder = parent.GetComponent<AbilityHolder>();

        Debug.Assert(abilityHolder.AimAssistLocation != null, "AimAssistLocation on Player not set");

        m_aimLocation = abilityHolder.AimAssistLocation;

        enemiesHit.Clear();
        Vector3 forward = m_aimLocation.forward;

        for (float currentAngle = 0.0f; currentAngle < m_maxAngleDegrees; currentAngle++)
        {
            Vector3 positiveRotatedVector = Quaternion.Euler(0.0f, currentAngle, 0.0f) * forward;
            Vector3 negativeRotatedVector = Quaternion.Euler(0.0f, -currentAngle, 0.0f) * forward;

            RaycastHit hit;
            if (Physics.Raycast(m_aimLocation.position, positiveRotatedVector, out hit, m_range, m_layer))
            {
                KnockbackHit(hit, positiveRotatedVector);
            }

            if (Physics.Raycast(m_aimLocation.position, negativeRotatedVector, out hit, m_range, m_layer))
            {
                KnockbackHit(hit, negativeRotatedVector);
            }


            if (m_showDebugLines)
            {
                Debug.DrawRay(parent.transform.position, positiveRotatedVector * m_range, Color.blue, 2.0f);
                Debug.DrawRay(parent.transform.position, negativeRotatedVector * m_range, Color.blue, 2.0f);
            }
        }
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// Knockbacks the hittet objects if they are enemies. Hittet enemies are added to enemiesHit list
    /// </summary>
    /// <param name="hit">Hit of a raycast</param>
    /// <param name="direction">Direction of the knockback</param>
    private void KnockbackHit(RaycastHit hit, Vector3 direction)
    {

        if (!enemiesHit.Contains(hit.transform.gameObject) && hit.transform.gameObject.CompareTag("Enemy"))
        {
            Rigidbody enemyRB = hit.transform.GetComponent<Rigidbody>();
            KinematicController kinematicController = hit.transform.GetComponent<KinematicController>();

            if (kinematicController)
            {
                kinematicController.DisableKinematic();
                enemyRB.AddForce(m_force * direction + Vector3.up * m_upwardsForce, ForceMode.Impulse);
            }

            enemiesHit.Add(hit.transform.gameObject);

        }
    }

    #endregion

    #region Private Member

    /// <summary>
    /// Angle of the knockback area in degrees
    /// </summary>
    [SerializeField] private float m_maxAngleDegrees = 45.0f;

    /// <summary>
    /// Range of the knockback area
    /// </summary>
    [SerializeField] private float m_range = 20.0f;

    /// <summary>
    /// Force of the knockback
    /// </summary>
    [SerializeField] private float m_force = 50.0f;

    /// <summary>
    /// Upwards force of the knockback
    /// </summary>
    [SerializeField] private float m_upwardsForce = 0.0f;

    /// <summary>
    /// Layer to be hit by the knockback
    /// </summary>
    [SerializeField] private LayerMask m_layer;

    /// <summary>
    /// Particle System to play at start of the ability
    /// </summary>
    [SerializeField] private ParticleSystem m_particleSystem;

    /// <summary>
    /// Determines if the Debug Lines are drawn
    /// </summary>
    [SerializeField] private bool m_showDebugLines = false;

    /// <summary>
    /// Location/Origin of the ability
    /// </summary>
    private Transform m_aimLocation;

    /// <summary>
    /// List with all enemies that are hit with the knockback
    /// </summary>
    private List<GameObject> enemiesHit = new List<GameObject>();

    #endregion

}
