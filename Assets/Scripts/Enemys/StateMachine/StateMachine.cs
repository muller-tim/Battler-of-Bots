using System;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine
{
    private IState currentState; // Aktueller Zustand

    // Übergänge zwischen Zuständen
    private Dictionary<Type, List<Transition>> transitions = new Dictionary<Type, List<Transition>>();
    private List<Transition> currentTransitions = new List<Transition>();
    private List<Transition> anyTransitions = new List<Transition>();
    private static List<Transition> emptyTransitions = new List<Transition>(0);

    private class Transition
    {
        public Func<bool> Condition { get; }  // Bedingung für den Übergang
        public IState To { get; } // Zielzustand des Übergangs

        public Transition(IState to, Func<bool> condition)
        {
            To = to;
            Condition = condition;
        }
    }

    public void Tick() //Methode wird jeden Frame aufgerufen, um den aktuellen Zustand zu aktualisieren
    {
        var transition = GetTransition(); // Erhalten des nächsten Übergangs
        if (transition != null)
            SetState(transition.To); // Wechseln zum Zielzustand des Übergangs

        currentState?.Tick(); // Aufrufen der Tick-Methode des aktuellen Zustands
    }

    public void SetState(IState state) // Methode zum Setzen des aktuellen Zustands
    {
        if (state == currentState)
            return;

        currentState?.OnExit(); // Ausführen von Exit-Logik des aktuellen Zustands
        currentState = state; // Aktualisieren des aktuellen Zustands

        transitions.TryGetValue(currentState.GetType(), out currentTransitions); // Abrufen der Übergänge für den aktuellen Zustand
        if (currentTransitions == null)
            currentTransitions = emptyTransitions;

        currentState.OnEnter(); // Ausführen von Enter-Logik des neuen Zustands
    }

    public void AddTransition(IState from, IState to, Func<bool> predicate)
    {
        // Hinzufügen eines Übergangs von einem Zustand zum anderen mit einer Bedingung
        if (transitions.TryGetValue(from.GetType(), out var transitionsList) == false)
        {
            transitionsList = new List<Transition>();
            transitions[from.GetType()] = transitionsList;
        }

        transitionsList.Add(new Transition(to, predicate));
    }

    public void AddAnyTransition(IState state, Func<bool> predicate)
    {
        // Hinzufügen eines Übergangs von einem beliebigen Zustand zu einem bestimmten Zustand mit einer Bedingung
        anyTransitions.Add(new Transition(state, predicate));
    }

    public Color GetGizmoColor()  // Methode zum Abrufen der Farbe für die Anzeige im Editor für Debugging-Zwecke
    {
        if (currentState != null)
        {
            return currentState.GizmoColor();
        }
        return Color.grey; // Grau, wenn kein Zustand vorhanden ist
    }

    private Transition GetTransition() // Methode zum Erhalten des nächsten Übergangs
    {
        foreach (var transition in anyTransitions) 
            if (transition.Condition()) // Überprüfen, ob die Bedingung für den Übergang erfüllt ist
                return transition;

        foreach (var transition in currentTransitions)
            if (transition.Condition()) // Überprüfen, ob die Bedingung für den Übergang erfüllt ist
                return transition;

        return null;  // Rückgabe von null, wenn kein Übergang gefunden wird
    }
}
