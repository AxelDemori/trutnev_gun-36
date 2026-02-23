using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

namespace FSM.Player
{
    public class EnemyMovingState : BaseState
    {
        private readonly EnemyFSM _stateMachine;
        public EnemyMovingState(EnemyFSM stateMachine) : base(name: "Moving", stateMachine)
        {
            _stateMachine = stateMachine;
        }
        public override void OnEnter()
        {
            base.OnEnter();
            _stateMachine.AnimationStateController.SetAnimation(type: CharacterAnimation.Walk);
        }
        public override void UpdateLogic()
        {
            base.UpdateLogic();

            _stateMachine.MovementController.UpdateMovement(_stateMachine.target.transform.position);

            if (Vector3.Distance(a: _stateMachine.transform.position, b: _stateMachine.target.transform.position) <= _stateMachine.hitDistance)
                _stateMachine.ChangeState(_stateMachine.EnemyAttackState);
        }
        private void OnHit(InputAction.CallbackContext context)
        {
            //_stateMachine.ChangeState(_stateMachine.PlayerHitState);
        }
    }
}

