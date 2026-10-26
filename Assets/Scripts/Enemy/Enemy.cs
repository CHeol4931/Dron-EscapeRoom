using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour
{
    [SerializeField] private float waitTime = 3f;
    [SerializeField] private float arriveTolerance = 0.3f;
    private enum State { Idle, MovingToShot, Waiting, Returning}

    private NavMeshAgent agent;
    private Vector3 homePosition;
    private State state = State.Idle;
    private float waitUntil;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        homePosition = transform.position;
    }

    public void OnShot(Vector3 shooterPosition)
    {
        if(NavMesh.SamplePosition(shooterPosition, out NavMeshHit hit, 3f, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
            state = State.MovingToShot;
        }
        else
        {
            Debug.LogWarning($"{name} : 드론 위치 근처에 NavMesh가 없어 이동하지 못함");
        }
    }

    void Update()
    {
        switch (state)
        {
            case State.MovingToShot:
                if (HasArrived())
                {
                    state = State.Waiting; waitUntil = Time.time + waitTime;
                }
                break;
            case State.Waiting:
                {
                    if(Time.time >= waitUntil)
                    {
                        agent.SetDestination(homePosition);
                        state = State.Returning;
                    }
                }
                break;
            case State.Returning:
                if (HasArrived()) state = State.Idle;
                break;
        }
    }
    private bool HasArrived()
    {
        return !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance + arriveTolerance;
    }
}
