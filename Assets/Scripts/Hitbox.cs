using UnityEngine;

public class Hitbox : MonoBehaviour
{
    public float damage = 20;
    public LayerMask hitMask;


    protected virtual void OnTriggerEnter(Collider other)
    {
        if (hitMask == (hitMask | (1 << other.transform.gameObject.layer)))
        {
            Debug.Log("Hit!");
            HealthComponent healthComponent = other.transform.root.GetComponent<HealthComponent>();

            if (healthComponent != null)
            {
                OnHit(healthComponent);
            }
        }
    }

    protected virtual void OnHit(HealthComponent healthComponent)
    {
        healthComponent.TakeDamage(damage);
    }
}
