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
        if (target != null)
            agent.SetDestination(target.position);
            
    }

    void OnCollisionEnter(Collision collision){
        if (collision.gameObject.CompareTag("Player")){
            // destroy the player
            Destroy(collision.gameObject);
        }
    }
}