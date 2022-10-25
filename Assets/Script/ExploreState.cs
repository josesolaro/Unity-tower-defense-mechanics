using System.Collections;
using UnityEngine;
using UnityEngine.AI;

class ExploreState : IState
{
    Vector3 _targetPosition;
    NavMeshAgent _naveMeshAgent;
    public ExploreState(NavMeshAgent navMeshAgent, Vector3 targetPosition)
    {
        _targetPosition = targetPosition;
        _naveMeshAgent = navMeshAgent;
    }
    public void OnEnter()
    {
        _naveMeshAgent.enabled = true;
        _naveMeshAgent.SetDestination(_targetPosition);
    }
    public IState Tick(MinionController controller)
    {
        if (controller.SpottedEnemy != null)
        {
            return new ReachState(_naveMeshAgent, controller.SpottedEnemy);
        }
        return null;
    }
    public void OnExit()
    {
        _naveMeshAgent.enabled = false;
    }
}
