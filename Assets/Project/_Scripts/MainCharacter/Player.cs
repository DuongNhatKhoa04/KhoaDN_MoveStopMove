using MoveStopMove.Core;
using MoveStopMove.Extensions.FSM;
using MoveStopMove.Extensions.FSM.States;
using MoveStopMove.Extensions.Helpers;
using MoveStopMove.MainCharacter.Data;
using UnityEngine;

namespace MoveStopMove.MainCharacter
{
    public class Player : Character
    {
        #region -- Fields --

        private Vector3 m_direction;
        private bool m_isMoving;
        private bool m_isGrounded;

        #endregion

        #region -- Methods --

        private void Awake()
        {
            base.Initialize();
        }

        private void Start()
        {
            StateMachine.Initialize(CharacterIdleState);
        }

        private void Update()
        {
            StateMachine.CurrentState.LogicUpdate();
        }

        private void FixedUpdate()
        {
            StateMachine.CurrentState.PhysicsUpdate();
        }

        #endregion
    }
}