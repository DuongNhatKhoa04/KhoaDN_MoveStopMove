using System;
using MoveStopMove.Extensions.Helpers;
using MoveStopMove.SO;
using UnityEngine;

namespace MoveStopMove.Core
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

        private void Update()
        {
            context.direction = ControlProvider.Instance.CheckDirection();
            context.isMoving = ControlProvider.Instance.IsMoving(context.direction);

            Machine.Tick(Time.deltaTime);
            var path = StatePath(Machine.Root.Leaf());
            if (path != LastPath)
            {
                Debug.Log(path.ToString());
                LastPath = path;
            }
        }

        private void FixedUpdate()
        {
            m_isGrounded = base.IsGrounded();

            if (!m_isMoving || !m_isGrounded)
            {
                base.Stop();
                base.ChangeAnimation(EAnim.Attack);
                return;
            }

            base.Movement(m_direction, context.speed);
            base.ChangeAnimation(EAnim.Run);
        }

        #endregion
    }
}