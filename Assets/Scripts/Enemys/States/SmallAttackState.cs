
using UnityEngine;

public class SmallAttackState : IState
{
    private SmallEnemyBrain brain;

    public SmallAttackState(SmallEnemyBrain brain)
    {
        this.brain = brain;
    }

    public void OnEnter()
    {
    }

    public void OnExit()
    {
    }

    public void Tick()
    {
        Explode();
    }

    public void Explode()
    {
        Debug.Log("Jetzt Explosion!");
        Object.Instantiate(brain.explosionsEffekt,brain.transform.position, brain.transform.rotation);
        Collider[] colliders = Physics.OverlapSphere(brain.transform.position, brain.explosionsRadius);
        foreach(Collider col in colliders){
            if(col.tag == "Player"){
                col.gameObject.GetComponent<HealthComponent>().TakeDamage(brain.explosionsDamage);
                break;
            }
        }
        brain.healthComponent.Kill();
    }

    public Color GizmoColor()
    {
        return Color.red;
    }
}