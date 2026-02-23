using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

namespace FSM.Player
{
    public class EnemyIdleState : BaseState
    {
        private readonly EnemyFSM _stateMachine;
        public EnemyIdleState(EnemyFSM stateMachine) : base(name: "Idle", stateMachine)
        {
            _stateMachine = stateMachine;
        }
        public override void OnEnter()
        {
            base.OnEnter();

            _stateMachine.AnimationStateController.SetAnimation(type: CharacterAnimation.Idle);
        }
        public override void UpdateLogic()
        {
            base.UpdateLogic();
            TryDetectPlayer();
        }
        private void TryDetectPlayer()
        {
            foreach (var player in PlayersHolder.Instance.players)
            {
                if (Vector3.Distance(a: player.transform.position, b: _stateMachine.transform.position) <= _stateMachine.detectionDistance)
                {
                    _stateMachine.target = player;
                    _stateMachine.ChangeState(_stateMachine.EnemyMovingState);
                    break;
                }
            }
        }
    }
}