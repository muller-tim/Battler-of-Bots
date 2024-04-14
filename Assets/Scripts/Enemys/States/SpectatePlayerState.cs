
using System.Collections;
using UnityEngine;

public class SpectatePlayerState : IState
{
    private BigEnemyBrain brain;
    private float timer;
    public SpectatePlayerState(BigEnemyBrain brain)
    {
        this.brain = brain;
    }

    public void OnEnter()
    {
        brain.laserpointer.enabled = true;
        brain.navMeshAgent.enabled = true;
        brain.navMeshAgent.speed = brain.aimMovementSpeed;
        timer = brain.timeBetweenBeam;
    }

    public void OnExit()
    {
        brain.laserpointer.enabled = false;
    } 

    public void Tick()
    {
        if(timer > 0f){
            timer -= Time.deltaTime;
        }
        else{
            brain.Attack = true;
        }
        LaserPointer();
        if (brain.navMeshAgent.destination != brain.target.position) // Ist aktulle Zielposition nicht gleich der Position des Ziels?
            brain.navMeshAgent.SetDestination(brain.target.position); // Setzen des NavMeshAgent-Ziels auf die Position des aktuellen Ziels
    }

    private void LaserPointer(){
        brain.transform.LookAt(new Vector3(brain.target.position.x, brain.transform.position.y, brain.target.position.z));
        Ray ray  = new Ray(brain.muzzlePoint.position, brain.muzzlePoint.forward);
        bool cast = Physics.Raycast(ray, out RaycastHit hit, brain.beamMaxLength, brain.hitMaskBeam);
        Vector3 hitPosition = cast ? hit.point : brain.muzzlePoint.position + brain.muzzlePoint.forward * brain.beamMaxLength;
        brain.laserpointer.SetPosition(0, brain.muzzlePoint.position);
        brain.laserpointer.SetPosition(1, hitPosition);
    }

    public Color GizmoColor()
    {
        return Color.red;
    }
}