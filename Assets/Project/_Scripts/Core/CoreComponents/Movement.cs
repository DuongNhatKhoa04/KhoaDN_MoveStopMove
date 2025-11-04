using MoveStopMove.Interfaces;
using UnityEngine;
using UnityEngine.Serialization;

namespace MoveStopMove.Core.CoreComponents
{
    public class Movement : CoreComponents, IMoveable
    {
        [SerializeField] private Rigidbody rb;
        [SerializeField] private CapsuleCollider col;
        [SerializeField] private LayerMask layerMask;
        [SerializeField] private float castDistance = 0.5f;
        [SerializeField] private float startOffset = 0.1f;
        private static readonly RaycastHit[] s_hits = new RaycastHit[1];
        private RaycastHit m_groundHit;

        public void Moving(Vector3 direction, float speed, float acceleration)
        {
            Vector3 inputDir = Vector3.ClampMagnitude(direction, 1f);

            Vector3 velocity = rb.velocity;
            Vector3 horizontalVelocity = new Vector3(velocity.x, 0f, velocity.z);

            Vector3 desiredVelocity = inputDir * speed;

            Vector3 deltaV = desiredVelocity - horizontalVelocity;

            float maxAccelPerFrame = acceleration * Time.fixedDeltaTime;
            if (deltaV.magnitude > maxAccelPerFrame)
                deltaV = deltaV.normalized * maxAccelPerFrame;

            rb.AddForce(deltaV, ForceMode.VelocityChange);

            if (!(direction.sqrMagnitude > 1e-4f)) return;

            Quaternion target = Quaternion.LookRotation(direction);
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, target, speed * Time.fixedDeltaTime));
        }

        public void Stop()
        {
            Vector3 v = rb.velocity;
            Vector3 horiz = new Vector3(v.x, 0f, v.z);
            if (horiz.sqrMagnitude > 1e-4f)
                rb.AddForce(-horiz, ForceMode.VelocityChange);

            rb.angularVelocity = Vector3.zero;
        }

        public bool IsGrounded()
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
                layerMask,
                QueryTriggerInteraction.Ignore
            );

            if (count <= 0) return false;

            m_groundHit = s_hits[0];

            return true;
        }
    }
}