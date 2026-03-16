public class AiMeleeAttackState : AiState
{
    public AiStateId GetId() => AiStateId.MeleeAttack;

    public void Enter(AiAgent agent)
    {
        agent.navMeshAgent.stoppingDistance = 2.0f;
        agent.navMeshAgent.speed = agent.config.attackSpeed;
    }

    public void Update(AiAgent agent)
    {
        if (!agent.targeting.HasTarget)
        {
            agent.stateMachine.ChangeState(AiStateId.FindTarget);
            return;
        }

        float distance = agent.targeting.TargetDistance;
        if (distance > 5.0f)
        {
            agent.stateMachine.ChangeState(AiStateId.ChasePlayer);
            return;
        }

        agent.navMeshAgent.destination = agent.targeting.TargetPosition;

    }

    public void Exit(AiAgent agent)
    {
        agent.navMeshAgent.stoppingDistance = 0;
    }
}