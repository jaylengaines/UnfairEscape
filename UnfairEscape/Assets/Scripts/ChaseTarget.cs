using UnityEngine;
using UnityEngine.AI;

public class ChaseTarget : MonoBehaviour
{
    public Transform target;
    NavMeshAgent agent;

    void Awake() => agent = GetComponent<NavMeshAgent>();

    void Update()
    {
        if (target != null)
            agent.SetDestination(target.position);
    }
}