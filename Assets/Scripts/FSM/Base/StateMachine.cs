using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FSM
{
    public class StateMachine : MonoBehaviour
    {
        public AnimationStateController AnimationStateController { get; protected set; }
        public BaseState CurrentState { get; private set; }
        void Start()
        {
            CurrentState = GetInitialState();

            if (CurrentState != null)
                CurrentState.OnEnter();
        }
        void Update()
        {
            if (CurrentState != null)
                CurrentState.UpdateLogic();
        }
        public void ChangeState(BaseState newState)
        {
            CurrentState.OnExit();

            CurrentState = newState;
            CurrentState.OnEnter();
        }
        protected virtual BaseState GetInitialState()
        {
            return null;
        }
    }
}
