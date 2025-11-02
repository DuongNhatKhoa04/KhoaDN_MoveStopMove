/*using System;
using System.Linq;
using MoveStopMove.Extensions.Helpers;
using MoveStopMove.Extensions.HSM;
using MoveStopMove.Interfaces;
using MoveStopMove.SO;
using UnityEngine;
using UnityEngine.Serialization;

namespace MoveStopMove.Core
{
    public class PlayerStateDriver : MonoBehaviour, IMoveable
    {
        #region -- Fields --

        /*[SerializeField] private PlayerContext context;#1#

        private LayerMask m_groundMask;
        private float m_castDistance;
        private float m_startOffset;
        private static readonly RaycastHit[] s_hits = new RaycastHit[1];
        private RaycastHit m_groundHit;

        private string m_lastPath;
        private Rigidbody m_rigidBody;
        private StateMachine machine;
        private State root;

        #endregion

        #region -- Methods --

        private void Update()
        {
            context.direction = ControlProvider.Instance.CheckDirection();
            context.isMoving = ControlProvider.Instance.IsMoving(context.direction);
        }

        private static string StatePath(State state)
        {
            return string.Join(" > ", state.PathToRoot().AsEnumerable().Reverse().Select(name => name.GetType().Name));
        }

        #endregion

        public bool IsGrounded()
        {
            throw new NotImplementedException();
        }

        public void Movement(Vector3 direction, float speed)
        {
            throw new NotImplementedException();
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }
    }

    /*[Serializable]
    public struct PlayerContext
    {
        public Rigidbody rb;
        public CapsuleCollider col;

        public Animator animator;
        public EAnim currentAnimation;

        public PlayerData playerData;

        public Vector3 direction;
        public bool isMoving;
        public bool isGrounded;
        public bool danceButtonPressed;
        public bool ultimateButtonPressed;
    }#1#
}*/