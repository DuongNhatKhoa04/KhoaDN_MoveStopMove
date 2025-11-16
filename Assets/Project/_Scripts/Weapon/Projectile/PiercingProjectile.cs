using System.Collections.Generic;
using MoveStopMove.Managers;
using UnityEngine;

namespace MoveStopMove.Weapon.Projectile
{
    public class PiercingProjectile : ProjectileBase
    {
        [Header("Piercing Settings")]
        [SerializeField] private int maxPierceCount = 3;

        [SerializeField] private LayerMask hittableLayers;

        private int m_currentPierceCount;
        private readonly HashSet<GameObject> m_hitTargets = new HashSet<GameObject>();

        public override void Initialize(GameObject attacker, Vector3 targetPos)
        {
            base.Initialize(attacker, targetPos);

            m_currentPierceCount = 0;
            m_hitTargets.Clear();
        }

        protected override void OnHitTarget(GameObject target)
        {
            if (target == Owner)
                return;

            if ((hittableLayers.value & (1 << target.layer)) == 0)
            {
                return;
            }

            if (!m_hitTargets.Add(target))
                return;

            Debug.Log($"[PiercingProjectile] Hit {target.name} ({m_currentPierceCount + 1}/{maxPierceCount})");
            EventManager.Notify(new HitEvent(Owner, target));

            m_currentPierceCount++;

            if (m_currentPierceCount >= maxPierceCount)
            {
                ReturnToPool();
            }
        }
    }
}