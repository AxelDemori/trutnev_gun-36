using UnityEngine;
using UnityEngine.InputSystem;

namespace FSM.Player
{
    public class PlayerHitState : BaseState
    {
        private readonly PlayerFSM _stateMachine;
        private float _timer;
        private readonly float _hitDuration = 0.5f;

        public PlayerHitState(PlayerFSM stateMachine) : base(name: "Hit", stateMachine)
        {
            _stateMachine = stateMachine;
        }

        public override void OnEnter()
        {
            base.OnEnter();

            _timer = 0f;

            var isHeavyHit = _stateMachine.InputController.actions[InputActions.HeavyHit.ToString()].IsPressed();

            if (isHeavyHit)
                _stateMachine.AnimationStateController.SetAnimation(type: CharacterAnimation.HeavyHit);
            else
                _stateMachine.AnimationStateController.SetAnimation(type: CharacterAnimation.LightHit);

            _stateMachine.MovementController.StopMoving();
        }

        public override void OnExit()
        {
        }

        public override void UpdateLogic()
        {
            base.UpdateLogic();

            _timer += Time.deltaTime;

            if (_timer >= _hitDuration)
            {
                if (_stateMachine.MovementController.MovementInput != Vector2.zero)
                    _stateMachine.ChangeState(_stateMachine.PlayerMovingState);
                else
                    _stateMachine.ChangeState(_stateMachine.PlayerIdleState);
            }
        }
    }
}