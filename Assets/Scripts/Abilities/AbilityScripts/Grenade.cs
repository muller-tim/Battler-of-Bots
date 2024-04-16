using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Grenade : AbilityBase
{
    [SerializeField] private GameObject bullet;

    public override void Activate(GameObject parent)
    {
        AbilityHolder abilityHolder = parent.GetComponent<AbilityHolder>();

        if (!abilityHolder.AbilityLocations[2]) return;
        GameObject spawnedBullet = Instantiate(bullet, abilityHolder.AbilityLocations[2].position, Quaternion.LookRotation(abilityHolder.AbilityLocations[2].forward) * bullet.transform.localRotation);

        GrenadeProjectile projectileComponent = spawnedBullet.GetComponent<GrenadeProjectile>();
        projectileComponent.Direction = abilityHolder.AbilityLocations[2].forward;
    }
}
