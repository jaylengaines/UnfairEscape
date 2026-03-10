using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
public class ChaseTarget : MonoBehaviour
{
    public Transform target;
    NavMeshAgent agent;
    EnemyStun enemyStun;
    [SerializeField] float navMeshSnapDistance = 3f;
    bool warnedNoNavMesh;

    void Awake(){
        agent = GetComponent<NavMeshAgent>();
        enemyStun = GetComponent<EnemyStun>();
    } 
    void Update()
    {
        if (agent == null || !agent.isActiveAndEnabled){
            return;
        }

        if (!agent.isOnNavMesh){
            if (TrySnapToNavMesh()){
                warnedNoNavMesh = false;
            }
            else if (!warnedNoNavMesh){
                Debug.LogWarning($"{name} cannot find nearby NavMesh. Increase navMeshSnapDistance or move spawn point.");
                warnedNoNavMesh = true;
            }
            return;
        }

        if (enemyStun != null && enemyStun.isStunned){ // if the target is stunned
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

    bool TrySnapToNavMesh()
    {
        NavMeshHit hit;
        if (NavMesh.SamplePosition(transform.position, out hit, navMeshSnapDistance, NavMesh.AllAreas)){
            return agent.Warp(hit.position);
        }
        return false;
    }

    void OnCollisionEnter(Collision collision){
        if (collision.gameObject.CompareTag("Player")){
            // destroy the player
            SceneManager.LoadScene("GameOverScene");
        }
    }
}