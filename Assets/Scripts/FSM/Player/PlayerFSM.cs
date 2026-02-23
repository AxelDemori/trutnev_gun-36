using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace FSM.Player
{
    [RequireComponent(typeof(PlayerMovementController))]
    public class PlayerFSM : StateMachine
    {
        public readonly float hitDistance = 0.9f;

        [HideInInspector] public PlayerIdleState PlayerIdleState;
        [HideInInspector] public PlayerMovingState PlayerMovingState;
        [HideInInspector] public PlayerHitState PlayerHitState;
        [HideInInspector] public PlayerStunnedState PlayerStunnedState;
        public PlayerMovementController MovementController { get; private set; }
        public PlayerInput InputController { get; private set; }
        private void Awake()
        {
            MovementController = GetComponent<PlayerMovementController>();
            InputController = GetComponent<PlayerInput>();
            AnimationStateController = GetComponent<AnimationStateController>();

            PlayerIdleState = new PlayerIdleState(this);
            PlayerMovingState = new PlayerMovingState(this);
            PlayerHitState = new PlayerHitState(this);
            PlayerStunnedState = new PlayerStunnedState(this);
        }
        protected override BaseState GetInitialState()
        {
            return PlayerIdleState;

        }
    }
}