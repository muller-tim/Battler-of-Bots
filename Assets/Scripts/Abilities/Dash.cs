using System.Collections;
using UnityEngine;

[CreateAssetMenu]
public class Dash : AbilityBase
{    
    public float dashSpeed;
    
    public override void Activate(GameObject parent) {
        MonoBehaviour behaviour = parent.GetComponent<MonoBehaviour>();
        behaviour.StartCoroutine(PerformDash(parent));
    }

   private IEnumerator PerformDash(GameObject parent)
    {
        CharacterController characterController = parent.GetComponent<CharacterController>();
        GameObject lowerBody = GameObject.FindGameObjectWithTag("PlayerLowerBody");

        parent.GetComponent<ParticleSystemExecutor>().PlayStartParticle();
        float elapsedTime = 0f;
        while (elapsedTime < activeTime)
        {
            // Die lokale Blickrichtung des upperBody-Objekts um -90° verringern
            Vector3 forwardDirection = lowerBody.transform.TransformDirection(Vector3.forward);
            forwardDirection = Quaternion.Euler(0, 90, 0) * forwardDirection;

            // Die Richtung des Dashes entsprechend der aktuellen lokalen Blickrichtung des upperBody-Objekts
            Vector3 dashDirection = new Vector3(forwardDirection.x, 0f, forwardDirection.z).normalized;

            float distanceToMove = dashSpeed * Time.deltaTime;
            characterController.Move(dashDirection * distanceToMove);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
}
