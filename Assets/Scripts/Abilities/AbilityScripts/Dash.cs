using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Dash")]
public class Dash : AbilityBase
{

    #region Public Methods

    /// <summary>
    /// Ability Holder will dash forward
    /// </summary>
    /// <param name="parent"><inheritdoc cref="AbilityBase.Activate"/></param>
    public override void Activate(GameObject parent)
    {
        MonoBehaviour behaviour = parent.GetComponent<MonoBehaviour>();
        behaviour.StartCoroutine(PerformDash(parent));
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// Player dashes forward for the ActiveTime duration in the direction of the legs 
    /// </summary>
    /// <param name="parent"></param>
    /// <returns></returns>
    private IEnumerator PerformDash(GameObject parent)
    {
        CharacterController characterController = parent.GetComponent<CharacterController>();
        GameObject lowerBody = GameObject.FindGameObjectWithTag("PlayerLowerBody");

        parent.GetComponent<ParticleSystemExecutor>().PlayStartParticle();

        float elapsedTime = 0.0f;
        while (elapsedTime < ActiveTime)
        {
            // Die lokale Blickrichtung des lowerBody-Objekts um -90° verringern
            Vector3 forwardDirection = lowerBody.transform.TransformDirection(Vector3.forward);
            forwardDirection = Quaternion.Euler(0, 90, 0) * forwardDirection;

            // Die Richtung des Dashes entsprechend der aktuellen lokalen Blickrichtung des lowerBody-Objekts
            Vector3 dashDirection = new Vector3(forwardDirection.x, 0f, forwardDirection.z).normalized;

            float distanceToMove = m_dashSpeed * Time.deltaTime;
            characterController.Move(dashDirection * distanceToMove);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    #endregion

    #region Private Member

    /// <summary>
    /// Speed of the dash
    /// </summary>
    [SerializeField] private float m_dashSpeed;

    #endregion
}
