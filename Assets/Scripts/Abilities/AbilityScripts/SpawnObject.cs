using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/SpawnGameObject")]
public class SpawnObject : AbilityBase
{
    #region Public Methods

    /// <summary>
    /// Spawn the m_objectToSpawn at the ability slot location
    /// </summary>
    /// <param name="parent"><inheritdoc cref="AbilityBase.Activate"/></param>
    public override void Activate(GameObject parent)
    {
        AbilityHolder abilityHolder = parent.GetComponent<AbilityHolder>();

        if (!abilityHolder.AbilityLocations[(int) AbilitySlot]) return;
        GameObject spawnedBullet = Instantiate(m_objectToSpawn, abilityHolder.AbilityLocations[(int) AbilitySlot].position, Quaternion.identity);
    }

    #endregion


    #region Private Member

    /// <summary>
    /// GameObject to spawn on ability use
    /// </summary>
    [SerializeField] private GameObject m_objectToSpawn;

    #endregion
}
