using System.Collections;

class AttackState : IState
{
    IHurt _target;
    IAttack _attacker;
    public AttackState(IAttack attacker, IHurt target)
    {
        _target = target;
        _attacker = attacker;
    }

    public void OnEnter()
    {
    }

    public void OnExit()
    {
    }

    public IState Tick(MinionController controller)
    {
        _attacker.Attack(_target);
        if (controller.SpottedEnemy == null)
        {
            return new ExploreState(controller.NavMeshAgent, controller.EnemyBase.transform.position);
        }
        return null;
    }
}
