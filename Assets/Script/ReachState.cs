using UnityEngine;
using UnityEngine.AI;

class ReachState : IState
{
    NavMeshAgent _agent;
    GameObject _target;
    public ReachState(NavMeshAgent agent, GameObject target)
    {
        _agent = agent;
        _target = target;
    }
    public void OnEnter()
    {
        _agent.enabled = true;
    }

    public void OnExit()
    {
        _agent.enabled = false;
    }

    public IState Tick(MinionController controller)
    {
        _agent.SetDestination(_target.transform.position);
        if (controller.EnemyOnRange)
        {
            return new AttackState(controller.GetComponent<IAttack>(), controller.SpottedEnemy.GetComponent<IHurt>());
        }
        return null;
    }
}
