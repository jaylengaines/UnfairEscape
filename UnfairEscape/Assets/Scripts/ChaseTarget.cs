using UnityEngine;
using UnityEngine.AI;

public class ChaseTarget : MonoBehaviour
{
    public Transform target;
    NavMeshAgent agent;

    void Awake(){
        agent = GetComponent<NavMeshAgent>();
    } 
    void Update()
    {
        if (GetComponent<EnemyStun>().isStunned){ // if the target is stunned
           agent.isStopped = true; // stop the agent
           return;
        }
        else { 
            agent.isStopped = false; // start the agent
        }
        if (target != null){
            agent.SetDestination(target.position); // set the destination to the target's position
        }
    }

    void OnCollisionEnter(Collision collision){
        if (collision.gameObject.CompareTag("Player")){
            // destroy the player
            Destroy(collision.gameObject);
        }
    }
}