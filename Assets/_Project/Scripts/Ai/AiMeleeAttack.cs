using UnityEngine;
using UnityEngine.AI;

public class AiMeleeAttack : MonoBehaviour
{
    
    [SerializeField] private float _attackCooldown = 2f;
    [SerializeField] private float _attackRange = 3f;
    [SerializeField] private int _attackDamage = 20;
    [SerializeField] private float _attackDuration = 1f;
    [SerializeField] private string _attackTriggerName = "KnifeAttack";

    private AiAgent _agent;
    private Animator _animator;
    private NavMeshAgent _navAgent;
    private float _attackTimer;
    private bool _isAttacking;

    private void Start()
    {
        _agent = GetComponent<AiAgent>();
        _animator = GetComponent<Animator>();
        _navAgent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (!CanAttack()) return;

        _attackTimer += Time.deltaTime;

        float distanceToTarget = _agent.targeting.TargetDistance;

        if (distanceToTarget <= _attackRange)
        {
            HandleMeleeAttack();
        }
        else
        {
            _isAttacking = false;
        }
    }

    private bool CanAttack()
    {
        return _agent != null &&
               _agent.targeting != null &&
               _agent.targeting.HasTarget &&
               _agent.targeting.Target != null;
    }

    private void HandleMeleeAttack()
    {
        if (!_isAttacking && _attackTimer >= _attackCooldown)
        {
            StartAttack();
        }

        if (_isAttacking && _attackTimer >= _attackDuration)
        {
            EndAttack();
        }
    }

    private void StartAttack()
    {
        _isAttacking = true;
        _attackTimer = 0;

        if (_navAgent != null)
        {
            _navAgent.isStopped = true;
            _navAgent.ResetPath();
        }

        if (_animator != null)
        {
            _animator.SetFloat("speed", 0);
            _animator.SetTrigger(_attackTriggerName);
        }

        Invoke(nameof(ApplyDamage), _attackDuration * 0.5f);
    }

    private void EndAttack()
    {
        _isAttacking = false;
        _attackTimer = 0;

        if (_navAgent != null)
        {
            _navAgent.isStopped = false;

            if (CanAttack())
            {
                _navAgent.SetDestination(_agent.targeting.TargetPosition);
            }
        }
    }

    private void ApplyDamage()
    {
        if (!CanAttack()) return;

        var target = _agent.targeting.Target;
        if (target == null) return;

        float distance = Vector3.Distance(transform.position, target.transform.position);
        if (distance > _attackRange) return;

        var playerHealth = target.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            Vector3 attackDirection = (target.transform.position - transform.position).normalized;
            playerHealth.TakeDamage(_attackDamage, attackDirection);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _attackRange);
    }
}