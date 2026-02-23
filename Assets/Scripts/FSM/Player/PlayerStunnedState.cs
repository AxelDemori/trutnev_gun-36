using UnityEngine;
using UnityEngine.InputSystem;

namespace FSM.Player
{
    public class PlayerStunnedState : BaseState
    {
        private readonly PlayerFSM _stateMachine;
        private float _timer;
        private readonly float _stunDuration = 2f;

        public PlayerStunnedState(PlayerFSM stateMachine) : base(name: "Stunned", stateMachine)
        {
            _stateMachine = stateMachine;
        }

        public override void OnEnter()
        {
            base.OnEnter();

            _timer = 0f;

            _stateMachine.AnimationStateController.SetAnimation(type: CharacterAnimation.Stunned);
            _stateMachine.MovementController.StopMoving();

            _stateMachine.InputController.actions[InputActions.LightHit.ToString()].Disable();
            _stateMachine.InputController.actions[InputActions.HeavyHit.ToString()].Disable();
        }

        public override void OnExit()
        {
            _stateMachine.InputController.actions[InputActions.LightHit.ToString()].Enable();
            _stateMachine.InputController.actions[InputActions.HeavyHit.ToString()].Enable();
        }

        public override void UpdateLogic()
        {
            base.UpdateLogic();

            _timer += Time.deltaTime;

            if (_timer >= _stunDuration)
            {
                if (_stateMachine.MovementController.MovementInput != Vector2.zero)
                    _stateMachine.ChangeState(_stateMachine.PlayerMovingState);
                else
                    _stateMachine.ChangeState(_stateMachine.PlayerIdleState);
            }
        }
    }
}