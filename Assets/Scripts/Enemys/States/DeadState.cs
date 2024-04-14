using UnityEngine;

public class DeadState : IState
{
    private readonly EnemyBrain brain; 

    public DeadState(EnemyBrain brain)
    {
        this.brain = brain;
    }

    public void OnEnter()
    {
        // brain.healthComponent.OnDeath += () => EnemyBrain.Destroy(brain.gameObject);
    }

    public void OnExit()
    {
    }

    public void Tick()
    {
        brain.healthComponent.Kill();
    }

    public Color GizmoColor()
    {
        return Color.black; // Rückgabe der Farbe Schwarz für die Anzeige im Editor
    }
}
