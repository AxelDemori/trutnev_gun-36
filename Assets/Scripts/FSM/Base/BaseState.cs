using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FSM
{
    public class BaseState
    {
        public string name;
        protected StateMachine stateMachine;

        public BaseState(string name, StateMachine stateMashine)
        {
            this.name = name;
            this.stateMachine = stateMashine;
        }

        public virtual void OnEnter() { }
        public virtual void UpdateLogic() { }
        public virtual void OnExit() { }
    }
}
