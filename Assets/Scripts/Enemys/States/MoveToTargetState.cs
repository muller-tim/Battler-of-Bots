using UnityEngine;

public class MoveToTargetState : IState
{
    private EnemyBrain brain;
    public MoveToTargetState(EnemyBrain brain)
    {
        this.brain = brain;
    }

    public void OnEnter()
    {
        brain.navMeshAgent.enabled = true; // Aktivieren des NavMeshAgent
        brain.navMeshAgent.speed = brain.movementSpeed; // Festlegen der Geschwindigkeit des NavMeshAgent auf Laufgeschwindigkeit
    }

    public void OnExit()
    {
    }

    public void Tick()
    {
        if (brain.navMeshAgent.destination != brain.target.position) // Ist aktulle Zielposition nicht gleich der Position des Ziels?
            brain.navMeshAgent.SetDestination(brain.target.position); // Setzen des NavMeshAgent-Ziels auf die Position des aktuellen Ziels
    }

    public Color GizmoColor()
    {
        return Color.yellow; // Rückgabe der Farbe Grün für die Anzeige im Editor
    }
}
