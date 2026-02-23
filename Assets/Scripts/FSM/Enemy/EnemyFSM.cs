using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

namespace FSM.Player
{
    [RequireComponent(typeof(EnemyMovementController))]
    public class EnemyFSM : StateMachine
    {
        public readonly float hitDistance = 0.9f;
        public readonly float detectionDistance = 6f;
        public readonly Vector2 hitDelay = new Vector2(x: 1, y: 2);

        [HideInInspector] public EnemyIdleState EnemyIdleState;
        [HideInInspector] public EnemyMovingState EnemyMovingState;
        [HideInInspector] public EnemyHitState EnemyHitState;
        [HideInInspector] public EnemyAttackState EnemyAttackState;
        [HideInInspector] public EnemyHittedState EnemyrHittedState;
        public EnemyMovementController MovementController { get; private set; }

        public PlayerFSM target;
        private void Awake()
        {
            MovementController = GetComponent<EnemyMovementController>();
            AnimationStateController = GetComponent<AnimationStateController>();

            EnemyIdleState = new EnemyIdleState(this);
            EnemyMovingState = new EnemyMovingState(this);
            EnemyHitState = new EnemyHitState(this);
            EnemyAttackState = new EnemyAttackState(this);
            EnemyrHittedState = new EnemyHittedState(this);
        }
        public void Die()
        {
            Destroy(GameObject);
        }

        protected override BaseState GetInitialState()
        {
            return EnemyIdleState;
        }
    }
}