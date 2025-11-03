using System;
using System.Collections.Generic;
using System.Linq;
using MoveStopMove.Extensions.Helpers;
using MoveStopMove.Extensions.HSM;
using MoveStopMove.Interfaces;
using MoveStopMove.SO;
using UnityEngine;
using UnityEngine.Serialization;

namespace MoveStopMove.Core
{
    [Serializable]
    public class CharacterContext
    {
        public Rigidbody rb;
        public CapsuleCollider col;

        public Animator animator;
        public EAnim currentAnimation;

        public float speed;
        public float acceleration;

        public float attackSpeed;
        public float attackRangeRadius;

        public Vector3 direction;
        public bool isMoving;
        public bool isGrounded;
        public bool danceButtonPressed;
        public bool ultimateButtonPressed;
    }

    public abstract class Character : MonoBehaviour, IInitializable, IMoveable
    {
        #region -- Fields --
        [Header("Character Movement")]
        [SerializeField] protected Rigidbody rb;
        [SerializeField] protected CapsuleCollider col;
        [SerializeField] protected LayerMask groundMask;
        [SerializeField] protected float castDistance;
        [SerializeField] protected float startOffset;
        private static readonly RaycastHit[] s_hits = new RaycastHit[1];
        private RaycastHit m_groundHit;

        [Header("Character Animation")]
        [SerializeField] protected Animator animator;
        [SerializeField] protected EAnim currentAnimation;

        [Header("Character Data")]
        [SerializeField] protected CharacterContext context;

        protected string LastPath;
        protected StateMachine Machine;
        protected State Root;

        #endregion

        #region -- Methods --

        protected static string StatePath(State state)
        {
            return string.Join(" > ", state.PathToRoot().AsEnumerable().Reverse().Select(name => name.GetType().Name));
        }

        public virtual void Initialize()
        {
            animator.SetTrigger(AnimHashes.Map[currentAnimation]);
            Root = new PlayerRoot(null, context);
            var builder = new StateMachineBuilder(Root);
            Machine = builder.Build();
        }

        public virtual void Movement(Vector3 direction, float speed)
        {
            Vector3 inputDir = Vector3.ClampMagnitude(direction, 1f);

            Vector3 velocity = rb.velocity;
            Vector3 horizontalVelocity = new Vector3(velocity.x, 0f, velocity.z);

            Vector3 desiredVelocity = inputDir * speed;

            Vector3 deltaV = desiredVelocity - horizontalVelocity;

            float maxAccelPerFrame = context.acceleration * Time.fixedDeltaTime;
            if (deltaV.magnitude > maxAccelPerFrame)
                deltaV = deltaV.normalized * maxAccelPerFrame;

            rb.AddForce(deltaV, ForceMode.VelocityChange);

            if (!(direction.sqrMagnitude > 1e-4f)) return;

            Quaternion target = Quaternion.LookRotation(direction);
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, target, speed * Time.fixedDeltaTime));
        }

        public virtual void Stop()
        {
            Vector3 v = rb.velocity;
            Vector3 horiz = new Vector3(v.x, 0f, v.z);
            if (horiz.sqrMagnitude > 1e-4f)
                rb.AddForce(-horiz, ForceMode.VelocityChange);

            rb.angularVelocity = Vector3.zero;
        }

        public virtual bool IsGrounded()
        {
            if (!col) return false;

            Vector3 center = transform.TransformPoint(col.center);
            float radius = col.radius;
            float half = Mathf.Max(0, col.height * 0.5f - radius);

            Vector3 p1 = center + Vector3.up * (half + startOffset);
            Vector3 p2 = center - Vector3.up * (half - startOffset);

            int count = Physics.CapsuleCastNonAlloc(
                p1,
                p2,
                radius,
                Vector3.down,
                s_hits,
                castDistance,
                groundMask,
                QueryTriggerInteraction.Ignore
            );

            if (count <= 0) return false;

            m_groundHit = s_hits[0];

            return true;

        }

        protected void ChangeAnimation(EAnim animationName, float speed = 1)
        {
            animator.speed = Mathf.Max(0f, speed);

            if (currentAnimation == animationName) return;

            animator.ResetTrigger(AnimHashes.Map[currentAnimation]);
            currentAnimation = animationName;
            animator.SetTrigger(AnimHashes.Map[currentAnimation]);
        }

        #endregion
    }
}