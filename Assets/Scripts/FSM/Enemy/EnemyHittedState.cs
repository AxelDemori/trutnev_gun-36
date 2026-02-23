using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

namespace FSM.Player
{
    public class EnemyHittedState : BaseState
    {
        private readonly EnemyFSM _stateMachine;

        public EnemyHittedState(EnemyFSM stateMachine) : base(name: "Hitted", stateMachine)
        {
            _stateMachine = stateMachine;
        }

        public override void OnEnter()
        {
            base.OnEnter();

            _stateMachine.AnimationStateController.OnAnimationFinished += CheckFinish;
            _stateMachine.AnimationStateController.SetAnimation(type: CharacterAnimation.Hitted);
        }

        public override void OnExit()
        {
            base.OnExit();

            _stateMachine.AnimationStateController.OnAnimationFinished -= CheckFinish;
        }

        private void CheckFinish(CharacterAnimation type)
        {
            if (type == CharacterAnimation.Hitted)
                _stateMachine.Die();
        }
    }
}
