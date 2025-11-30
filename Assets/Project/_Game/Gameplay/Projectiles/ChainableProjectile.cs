using System.Collections.Generic;
using MoveStopMove.Core.Events;
using UnityEngine;

namespace MoveStopMove.Gameplay.Projectiles
{
    public class ChainableProjectile :  ProjectileBase
    {
        #region -- Fields --

        [Header("Chain Settings")]
        [SerializeField] private float chainRadius = 5f;
        [SerializeField] private int maxChains = 1;

        private int m_chainCount;
        private readonly HashSet<GameObject> m_hitTargets = new();

        #endregion

        #region -- Methods --

        public override void Initialize(GameObject attacker, Vector3 targetPos)
        {
            base.Initialize(attacker, targetPos);

            m_chainCount = 0;
            m_hitTargets.Clear();
        }

        protected override void OnHitTarget(GameObject target)
        {
            if (target == Owner) return;

            if ((hittableLayers.value & (1 << target.layer)) == 0) return;

            if (!m_hitTargets.Add(target)) return;

            EventManager.Instance.Notify(new HitTarget(Owner, target));

            if (m_chainCount >= maxChains)
            {
                ReturnToPool();
                return;
            }

            GameObject nextTarget = FindNextTarget(target.transform.position);
            if (nextTarget == null)
            {
                ReturnToPool();
                return;
            }

            m_chainCount++;

            Vector3 dir = nextTarget.transform.position - transform.position;
            if (dir.sqrMagnitude < 1e-4f)
            {
                dir = transform.forward;
            }
            Direction = dir.normalized;
        }

        private GameObject FindNextTarget(Vector3 fromPos)
        {
            Collider[] hits = Physics.OverlapSphere(fromPos, chainRadius, hittableLayers);

            GameObject best = null;
            float bestSqrDist = float.MaxValue;

            foreach (var hit in hits)
            {
                var go = hit.attachedRigidbody ? hit.attachedRigidbody.gameObject : hit.gameObject;

                if (go == null) continue;
                if (go == Owner) continue;
                if (m_hitTargets.Contains(go)) continue;

                float sqr = (go.transform.position - fromPos).sqrMagnitude;
                if (sqr < bestSqrDist)
                {
                    bestSqrDist = sqr;
                    best = go;
                }
            }

            return best;
        }

        #endregion
    }
}