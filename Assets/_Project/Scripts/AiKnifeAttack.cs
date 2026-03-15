using UnityEngine;

public class AiMeleeAttack : MonoBehaviour
{
    [Header("Attack Settings")]
    [SerializeField] private float _attackCooldown = 2f;
    [SerializeField] private float _attackRange = 3f;
    [SerializeField] private int _attackDamage = 20;
    [SerializeField] private float _attackDuration = 1f;

    [Header("Animation")]
    [SerializeField] private string _attackTriggerName = "KnifeAttack";

    private AiAgent _agent;
    private Animator _animator;
    private float _attackTimer;
    private bool _isAttacking;

    private void Start()
    {
        _agent = GetComponent<AiAgent>();
        _animator = GetComponent<Animator>();

        if (_animator == null)
            _animator = gameObject.AddComponent<Animator>();
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
    }

    private bool CanAttack()
    {
        return _agent != null &&
               _agent.targeting != null &&
               _agent.targeting.HasTarget;
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

        // Останавливаем движение
        if (_agent.navMeshAgent != null)
            _agent.navMeshAgent.isStopped = true;

        // Запускаем анимацию
        if (_animator != null)
            _animator.SetTrigger(_attackTriggerName);

        // Урон в середине анимации
        Invoke(nameof(ApplyDamage), _attackDuration * 0.5f);

        Debug.Log($"{gameObject.name} performs knife attack!");
    }

    private void EndAttack()
    {
        _isAttacking = false;
        _attackTimer = 0;

        // Возобновляем движение
        if (_agent.navMeshAgent != null)
            _agent.navMeshAgent.isStopped = false;
    }

    private void ApplyDamage()
    {
        if (!CanAttack()) return;

        var target = _agent.targeting.Target;
        if (target == null) return;

        // Проверяем дистанцию
        float distance = Vector3.Distance(transform.position, target.transform.position);
        if (distance > _attackRange) return;

        // Получаем компонент здоровья игрока
        var playerHealth = target.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            // Направление удара (от AI к игроку)
            Vector3 attackDirection = (target.transform.position - transform.position).normalized;

            // ИСПОЛЬЗУЕМ TakeDamage из базового класса Health
            playerHealth.TakeDamage(_attackDamage, attackDirection);

            Debug.Log($"AI damaged player for {_attackDamage}! Player health: {playerHealth.currentHealth}");
        }
    }

    // Для отладки радиуса атаки
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _attackRange);
    }
}