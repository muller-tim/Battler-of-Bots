using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu]
public class Bazooka : AbilityBase
{
    [SerializeField] private GameObject bullet;
    [SerializeField] private LayerMask m_layer;

    private Transform m_aimLocation;
    public override void Activate(GameObject parent) {
        AbilityHolder abilityHolder = parent.GetComponent<AbilityHolder>();

        if (!abilityHolder.AbilityLocations[1]) return;
        if (!abilityHolder.AimLocation) return;
        m_aimLocation = abilityHolder.AimLocation;

        Vector3 shootDirection = GetShootDirection(parent, abilityHolder.AbilityLocations[1]);
        GameObject spawnedBullet = Instantiate(bullet, abilityHolder.AbilityLocations[1].position, Quaternion.LookRotation(shootDirection) * bullet.transform.localRotation);

        Projectile projectileComponent = spawnedBullet.GetComponent<Projectile>();
        projectileComponent.Direction = shootDirection;
    }

    private Vector3 GetShootDirection(GameObject parent, Transform shootLocation)
    {

        //Debug.DrawRay(m_aimLocation.position, m_aimLocation.forward, Color.blue, 5.0f);

        RaycastHit hit;
        Vector3 target;
        Ray ray = new Ray(m_aimLocation.transform.position, m_aimLocation.forward);
        if (Physics.SphereCast(m_aimLocation.transform.position, 2.0f, m_aimLocation.forward, out hit, Mathf.Infinity, m_layer)) {
            target = hit.point;
        }
        else
        {
            target = ray.GetPoint(1000.0f);
        }

        Vector3 lookAtDirection = target - shootLocation.position;
        // Debug.DrawRay(shootLocation.position, lookAtDirection, Color.red, 5.0f);

        return lookAtDirection.normalized;
    }
}
