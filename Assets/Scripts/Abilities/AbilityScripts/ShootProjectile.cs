using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// ScriptableObject for shooting a projectile ability
/// </summary>
[CreateAssetMenu(menuName = "Abilities/ShootProjectile")]
public class ShootProjectile : AbilityBase {
    #region Public Methods

    /// <summary>
    /// Spawns and shoots a projectile
    /// </summary>
    /// <param name="parent"><inheritdoc cref="AbilityBase.Activate"/></param>
    public override void Activate(GameObject parent)
    {
        AbilityHolder abilityHolder = parent.GetComponent<AbilityHolder>();

        Debug.Assert(abilityHolder.AbilityLocations[(int)AbilitySlot] != null, $"Ability Location {(int)AbilitySlot} on Player is not set");
        Debug.Assert(abilityHolder.AimAssistLocation != null, "AimAssistLocation on Player is not set");

        m_aimAssistLocation = abilityHolder.AimAssistLocation;

        var shootDirection = aimAssistActivated ? GetShootDirection(abilityHolder.AbilityLocations[(int)AbilitySlot]) : abilityHolder.AbilityLocations[(int)AbilitySlot].forward;
        
        Projectile spawnedBullet = Instantiate(bullet, abilityHolder.AbilityLocations[(int)AbilitySlot].position, Quaternion.LookRotation(shootDirection) * bullet.transform.localRotation);
        spawnedBullet.Direction = shootDirection;
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// Calculates shooting direction for aim assist
    /// </summary>
    /// <param name="shootLocation"></param>
    /// <returns></returns>
    private Vector3 GetShootDirection(Transform shootLocation)
    {
        //Debug.DrawRay(m_aimAssistLocation.position, m_aimAssistLocation.forward, Color.blue, 5.0f);

        Vector3 target;
        Ray ray = new Ray(m_aimAssistLocation.transform.position, m_aimAssistLocation.forward);

        if (Physics.SphereCast(m_aimAssistLocation.transform.position, 2.0f, m_aimAssistLocation.forward, out RaycastHit hit, Mathf.Infinity, m_layer)) {
            target = hit.point;
        } else {
            target = ray.GetPoint(1000.0f);
        }

        Vector3 lookAtDirection = target - shootLocation.position;
        // Debug.DrawRay(shootLocation.position, lookAtDirection, Color.red, 5.0f);

        return lookAtDirection.normalized;
    }

    #endregion


    #region Private members

    /// <summary>
    /// Projectile to spawn and shoot
    /// </summary>
    [SerializeField, Tooltip("Projectile to spawn and shoot")] private Projectile bullet;


    /// <summary>
    /// Determines if aim assist is active
    /// </summary>
    [SerializeField, Tooltip("Aim Assist")] private bool aimAssistActivated = true;

    /// <summary>
    /// Layer for the aim assist
    /// </summary>
    [SerializeField, Tooltip("Layer for the aim assist")] private LayerMask m_layer;

    /// <summary>
    /// Location where the aim assist starts tracing
    /// </summary>
    private Transform m_aimAssistLocation;

    #endregion
}
