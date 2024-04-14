using UnityEngine;

public class IdleState : IState
{
    EnemyBrain brain;
    public IdleState(EnemyBrain brain)
    {
        this.brain = brain;
    }

    public void OnEnter()
    {
        brain.navMeshAgent.enabled = false;
    }

    public void OnExit()
    {
    }

    public void Tick()
    {
        brain.target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public Color GizmoColor()
    {
        return Color.white; // Rückgabe der Farbe Weiß für die Anzeige im Editor
    }
}
