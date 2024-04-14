using System;
using UnityEngine;

public class SmallEnemyBrain : EnemyBrain
{
    public GameObject explosionsEffekt;
    public float explosionsRadius = 5f;
    public float explosionsDamage = 50f;
    void Start()
    {
        healthComponent.OnDeath += () => Destroy(gameObject);
        stateMachine = new StateMachine();

        //STATES
        var idleState = new IdleState(this);
        var moveToTargetState = new MoveToTargetState(this);
        var attackState = new SmallAttackState(this);
        var deadState = new DeadState(this);

        //TRANSITIONS
        Any(deadState, Dead());
        At(idleState, moveToTargetState, HasTarget());
        At(moveToTargetState, attackState, InAttackRange());
        
        //START STATE
        stateMachine.SetState(idleState);

        //CONDITIONS & FUNCTIONS
        Func<bool> HasTarget() => () => target != null;
        Func<bool> Dead() => () => healthComponent.Health <= 0f;
        Func<bool> InAttackRange() => () => distanceToTarget <= attackRange;

        void At(IState from, IState to, Func<bool> condition) => stateMachine.AddTransition(from, to, condition);
        void Any(IState to, Func<bool> condition) => stateMachine.AddAnyTransition(to, condition);
    }
}
