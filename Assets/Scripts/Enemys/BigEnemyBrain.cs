using System;
using UnityEngine;
public class BigEnemyBrain : EnemyBrain
{
    public float aimMovementSpeed;
    public LayerMask hitMaskBeam;
    public LineRenderer laserpointer;
    public LineRenderer smallBeam;
    public LineRenderer bigBeam;
    public float smallBeamDamage = 1f;
    public float bigBeamDamage = 3f;
    public float timeBetweenBeam = 1f;
    public float smallBeamDuration = 1.5f;
    public float bigBeamDuration = 3f;
    public float beamDelay = 0.5f;
    public Transform muzzlePoint;
    public float beamMaxLength = 50f;
    public bool DoneAttack1 { get; set; } = false;
    public bool DoneAttack2 { get; set; } = false;
    public bool Attack { get; set; } = false;
    public GameObject healthpack;
    void Start() {
        healthComponent.OnDeath += () => Instantiate(healthpack, transform.position, Quaternion.identity);
        healthComponent.OnDeath += () => Destroy(gameObject);
        stateMachine = new StateMachine();

        //STATES
        var idleState = new IdleState(this);
        var moveToTargetState = new MoveToTargetState(this);
        var spectatePlayerState = new SpectatePlayerState(this);
        var attackState = new BigAttackState(this);
        var deadState = new DeadState(this);

        //TRANSITIONS
        Any(deadState, Dead());
        At(idleState, moveToTargetState, HasTarget());
        At(moveToTargetState, spectatePlayerState, InAttackRange());
        At(spectatePlayerState, moveToTargetState, NotInAttackRange());
        At(spectatePlayerState, attackState, DoAttack());
        At(attackState, moveToTargetState, NotInAttackRange());
        At(attackState, spectatePlayerState, DontAttack());
        
        //START STATE
        stateMachine.SetState(idleState);

        //CONDITIONS & FUNCTIONS
        Func<bool> HasTarget() => () => target != null;
        Func<bool> Dead() => () => healthComponent.Health <= 0f;
        Func<bool> InAttackRange() => () => distanceToTarget <= attackRange;
        Func<bool> NotInAttackRange() => () => distanceToTarget > attackRange;
        Func<bool> DoAttack() => () =>  Attack == true && distanceToTarget <= attackRange;
        Func<bool> DontAttack() => () =>  Attack == false && distanceToTarget <= attackRange;

        void At(IState from, IState to, Func<bool> condition) => stateMachine.AddTransition(from, to, condition);
        void Any(IState to, Func<bool> condition) => stateMachine.AddAnyTransition(to, condition);
    }
}
