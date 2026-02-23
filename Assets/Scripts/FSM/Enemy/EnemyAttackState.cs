using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

namespace FSM.Player
{
    public class EnemyAttackState : BaseState
    {
        private readonly EnemyFSM _stateMachine;

        private CancellationTokenSource cancellationToken;
        public EnemyAttackState(EnemyFSM stateMachine) : base(name: "Attack", stateMachine)
        {
            _stateMachine = stateMachine;
        }
        public override async void OnEnter()
        {
            base.OnEnter();

            _stateMachine.AnimationStateController.SetAnimation(type: CharacterAnimation.Idle);

            cancellationToken = new CancellationTokenSource();
            await Attack();
        }
        public override void OnExit()
        {
            base.OnExit();
            cancellationToken.Cancel();
            cancellationToken.Dispose();
        }
        public override void UpdateLogic()
        {
            base.UpdateLogic();

            //AttackTimer();
        }
        private async Task Attack()
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                CheckTargetGone();

                var newDelay :float = UnityEngine.Random.Range(_stateMachine.hitDelay.x, _stateMachine.hitDelay.y) * 1000; //Convert to milliseconds                                                                                                 
                await Task.Delay((int)newDelay);

                Hit();
            }
        }
        private void Hit()
        {
            _stateMachine.ChangeState(_stateMachine.EnemyHitState);
        }
        private void CheckTargetGone()
        {
            var distanceToTarget :float = Vector2.Distance(a: _stateMachine.transform.position, b: _stateMachine.target.transform.position);
            if (distanceToTarget >= _stateMachine.hitDistance)
                _stateMachine.ChangeState(_stateMachine.EnemyMovingState);
        }
    }
}