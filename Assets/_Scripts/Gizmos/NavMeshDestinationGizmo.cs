using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class NavMeshDestinationGizmo : MonoBehaviour
{
    [SerializeField] private float circleRadius;

    private NavMeshAgent agent;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(agent.destination, circleRadius);
    }
}