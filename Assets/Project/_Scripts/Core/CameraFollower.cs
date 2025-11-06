using UnityEngine;

namespace MoveStopMove.Core
{
    public class CameraFollower : MonoBehaviour
    {
        [Header("Target")]
        public Transform target;

        [Header("Offset")]
        public Vector3 offset = new Vector3(0f, 15f, -15f);

        [Header("Rotation (tilt)")]
        [Range(-89f, 89f)]
        public float pitch = 30f;

        public bool followYaw = true;
        public float yawOffset = 0f;

        [Header("Smoothing")]
        [Tooltip("Position smoothing time (seconds). Small value -> snappier.")]
        public float positionSmoothTime = 0.2f;

        [Tooltip("Rotation smoothing speed (bigger -> faster).")]
        public float rotationSmoothSpeed = 5f;

        private Vector3 m_velocity = Vector3.zero;

        private void LateUpdate()
        {
            if (target == null) return;

            Vector3 desiredPosition = ComputeDesiredPosition();
            Vector3 newPos = Vector3.SmoothDamp(transform.position, desiredPosition, ref m_velocity, Mathf.Max(0.0001f, positionSmoothTime));
            transform.position = newPos;

            float desiredYaw = followYaw ? (target.eulerAngles.y + yawOffset) : yawOffset;
            Quaternion desiredRot = Quaternion.Euler(pitch, desiredYaw, 0f);
            transform.rotation = Quaternion.Slerp(transform.rotation, desiredRot, Mathf.Clamp01(Time.deltaTime * rotationSmoothSpeed));
        }

        private Vector3 ComputeDesiredPosition()
        {
            if (followYaw && target != null)
            {
                Quaternion yRot = Quaternion.Euler(0f, target.eulerAngles.y + yawOffset, 0f);
                return target.position + (yRot * offset);
            }

            return target.position + offset;
        }
    }
}