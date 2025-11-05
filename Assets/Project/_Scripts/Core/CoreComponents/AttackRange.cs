using System;
using System.Collections.Generic;
using MoveStopMove.Extensions.Observer;
using UnityEngine;
using UnityEngine.Serialization;

namespace MoveStopMove.Core.CoreComponents
{

    public struct TargetEntry
    {
        public GameObject Target;
        public Vector3 EnterPosition;

        public TargetEntry(GameObject target, Vector3 enterPosition)
        {
            this.Target = target;
            this.EnterPosition = enterPosition;
        }
    }
    public class AttackRange : CoreComponents
    {
        [Range(16,256)]
        [SerializeField] private int segments = 64;
        [SerializeField] private float yOffset = 0.02f;

        [SerializeField] private SphereCollider sphereCol;
        [SerializeField] private LineRenderer lineRenderer;
        [SerializeField] private LayerMask targetLayer;


        private readonly Queue<TargetEntry> m_targetQueue = new();
        private readonly HashSet<GameObject> m_set = new();

        private void Awake()
        {
            lineRenderer.loop = true;
            lineRenderer.useWorldSpace = false;
            lineRenderer.positionCount = segments;

            Redraw();
        }

        public void SetRange(float r)
        {
            sphereCol.radius += Mathf.Max(0f, r);
            Redraw();
        }

        private void Redraw()
        {
            float r = sphereCol.radius;
            float step = 2f * Mathf.PI / segments;
            var pts = new Vector3[segments];
            for (int i = 0; i < segments; i++)
            {
                float a = i * step;
                pts[i] = new Vector3(Mathf.Cos(a) * r, yOffset, Mathf.Sin(a) * r);
            }
            lineRenderer.positionCount = segments;
            lineRenderer.SetPositions(pts);
        }

        private void SetVisual(bool enable)
        {
            lineRenderer.enabled = enable;
        }

        public bool IsAnyCharacterInRange(GameObject other)
        {
            if (other == null || other == gameObject) return false;

            return ((1 << other.layer) & targetLayer.value) != 0;
        }

        private void OnTriggerEnter(Collider other)
        {
            var go = other.attachedRigidbody ? other.attachedRigidbody.gameObject : other.gameObject;

            if (go == gameObject)
                return;

            if (!IsAnyCharacterInRange(go))
                return;

            if (m_set.Add(go))
            {
                var entry = new TargetEntry(go, go.transform.position);
                m_targetQueue.Enqueue(entry);
                //Debug.Log($"[ENTER] {go.name} vào vùng tấn công của {name}");
            }
        }

        private void OnTriggerExit(Collider other)
        {
            var go = other.attachedRigidbody ? other.attachedRigidbody.gameObject : other.gameObject;

            if (!IsAnyCharacterInRange(go))
                return;

            if (m_set.Remove(go))
            {
                int count = m_targetQueue.Count;
                for (int i = 0; i < count; i++)
                {
                    var enemy = m_targetQueue.Dequeue();
                    if (enemy.Target == go) continue;
                    m_targetQueue.Enqueue(enemy);
                }

                //Debug.Log($"[EXIT] {go.name} rời vùng tấn công của {name}");
            }
        }

        public TargetEntry? PeekEntry()
        {
            while (m_targetQueue.Count > 0)
            {
                var enemy = m_targetQueue.Peek();
                if (enemy.Target == null || !m_set.Contains(enemy.Target))
                {
                    m_targetQueue.Dequeue();
                    continue;
                }
                return enemy;
            }
            return null;
        }

        public TargetEntry? PopEntry()
        {
            while (m_targetQueue.Count > 0)
            {
                var e = m_targetQueue.Dequeue();
                if (e.Target == null)
                {
                    m_set.Remove(e.Target);
                    continue;
                }
                if (!m_set.Remove(e.Target))
                {
                    continue;
                }
                return e;
            }
            return null;
        }

        public static Vector3 GetTargetPosition(TargetEntry entry, bool useCurrentIfAlive = true)
        {
            if (useCurrentIfAlive && entry.Target != null)
                return entry.Target.transform.position;
            return entry.EnterPosition;
        }

        public void ClearTargets()
        {
            m_targetQueue.Clear();
            m_set.Clear();
        }

        // public int PendingTargetCount => m_targetQueue.Count;
    }
}