using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class LandmineAbility : AbilityBase
{
    [SerializeField] private GameObject landmine;

    public override void Activate(GameObject parent)
    {
        AbilityHolder abilityHolder = parent.GetComponent<AbilityHolder>();

        if (!abilityHolder.AbilityLocations[3]) return;
        GameObject spawnedBullet = Instantiate(landmine, abilityHolder.AbilityLocations[3].position, Quaternion.identity);
    }
}
