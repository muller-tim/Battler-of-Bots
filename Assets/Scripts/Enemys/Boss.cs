using System;
using UnityEngine;

public class Boss : EnemyBrain
{
    public LayerMask hitMask;
    public float rollSpeed;
    public float damage = 10f;
    public float damageCooldown = 1f; // Abklingzeit zwischen den Schaden
    void Start()
    {
        healthComponent.OnDeath += () => Destroy(gameObject);
        stateMachine = new StateMachine();

        //STATES
        var idleState = new IdleState(this);
        var attackState = new BossAttackState(this);
        var deadState = new DeadState(this);

        //TRANSITIONS
        Any(deadState, Dead());
        At(idleState, attackState, HasTarget());
        
        //START STATE
        stateMachine.SetState(idleState);

        //CONDITIONS & FUNCTIONS
        Func<bool> HasTarget() => () => target != null;
        Func<bool> Dead() => () => healthComponent.Health <= 0f;

        void At(IState from, IState to, Func<bool> condition) => stateMachine.AddTransition(from, to, condition);
        void Any(IState to, Func<bool> condition) => stateMachine.AddAnyTransition(to, condition);
    }
}
