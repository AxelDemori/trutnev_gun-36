using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace FSM.Player
{
    public class PlayerIdleState : BaseState
    {
        private readonly PlayerFSM _stateMachine;

        public PlayerIdleState(PlayerFSM stateMachine) : base(name: "Idle", stateMachine)
        {
            _stateMachine = stateMachine;
        }

        public override void OnEnter()
        {
            base.OnEnter();

            _stateMachine.InputController.actions[InputActions.LightHit.ToString()].performed += OnHit;
            _stateMachine.InputController.actions[InputActions.HeavyHit.ToString()].performed += OnHit;

            _stateMachine.AnimationStateController.SetAnimation(type: CharacterAnimation.Idle);
        }

        public override void OnExit()
        {
            _stateMachine.InputController.actions[InputActions.LightHit.ToString()].performed -= OnHit;
            _stateMachine.InputController.actions[InputActions.HeavyHit.ToString()].performed -= OnHit;
        }

        public override void UpdateLogic()
        {
            base.UpdateLogic();

            _stateMachine.MovementController.UpdateMovement();

            if (_stateMachine.MovementController.MovementInput != Vector2.zero)
            {
                _stateMachine.ChangeState(_stateMachine.PlayerMovingState);
            }
        }

        private void OnHit(InputAction.CallbackContext context)
        {
            _stateMachine.ChangeState(_stateMachine.PlayerHitState);
        }
    }
}