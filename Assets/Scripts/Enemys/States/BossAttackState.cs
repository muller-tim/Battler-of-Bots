using UnityEngine;
public class BossAttackState : IState
{
    private float lastDamageTime; // Zeitpunkt des letzten verursachten Schadens
    private GameObject particles;

    private Boss brain;

    public BossAttackState(Boss brain)
    {
        this.brain = brain;
    }

    public void OnEnter()
    {
        brain.navMeshAgent.enabled = true; // Aktivieren des NavMeshAgent
        brain.navMeshAgent.speed = brain.movementSpeed; // Festlegen der Geschwindigkeit des NavMeshAgent auf Laufgeschwindigkeit
        particles = GameObject.FindGameObjectWithTag("BossParticles");
        particles.SetActive(false);
    }

    public void OnExit()
    {
    }

    public void Tick()
    {
        if (brain.navMeshAgent.destination != brain.target.position) // Ist aktulle Zielposition nicht gleich der Position des Ziels?
            brain.navMeshAgent.SetDestination(brain.target.position); // Setzen des NavMeshAgent-Ziels auf die Position des aktuellen Ziels
        if(brain.distanceToTarget <= brain.attackRange)
        {
            Attack();
        }
        else{
            particles.SetActive(false);
        }
    }

    public void Attack()
    {
        brain.transform.LookAt(new Vector3(brain.target.position.x, brain.transform.position.y, brain.target.position.z));
        // Überprüfe, ob genügend Zeit seit dem letzten Schaden vergangen ist
        if (Time.time - lastDamageTime >= brain.damageCooldown)
        {
            // Führe den Raycast aus
            RaycastHit hit;
            if (Physics.Raycast(brain.transform.position + new Vector3(0,3,0), brain.transform.forward, out hit, brain.attackRange, brain.hitMask))
            {
                // Wenn der Raycast das Ziel trifft und es sich auf dem richtigen Layer befindet
                // Rufe die Methode "DealDamage" auf dem getroffenen Objekt auf (falls es einen entsprechenden Schadenkomponenten hat)
                HealthComponent target = hit.collider.GetComponent<HealthComponent>();
                if (target != null)
                {
                    particles.SetActive(true);
                    target.TakeDamage(brain.damage);
                    lastDamageTime = Time.time;
                }
                
            }
        }
    }

    public Color GizmoColor()
    {
        return Color.red;
    }
}