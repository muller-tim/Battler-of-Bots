using UnityEngine;

public interface IState
{
    void Tick();//Wird jeden Frame aufgerufen, um die Logik des Zustands auszuführen.
    void OnEnter(); // Wird aufgerufen, wenn der Zustand betreten wird.
    void OnExit(); // Wird aufgerufen, wenn der Zustand verlassen wird.
    Color GizmoColor(); // Farbe für die Anzeige im Editor zurückgeben für Debugging-Zwecke.
}