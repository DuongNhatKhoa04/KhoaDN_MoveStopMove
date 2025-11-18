using MoveStopMove.Core.Events;
using MoveStopMove.Core.Stats;
using MoveStopMove.Utility.Extension;
using System.Collections.Generic;
using UnityEngine;

namespace MoveStopMove.Gameplay.Projectiles
{
    public class PiercingProjectile : ProjectileBase
    {
        #region -- Fields --

        [Header("Piercing Settings")]
        [SerializeField] private int maxPierceCount = 2;

        private int m_currentPierceCount;
        private readonly HashSet<GameObject> m_hitTargets = new();

        #endregion

        #region -- Methods --

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

            var attackRangeBuff = PlayerSaveLoader.GetDecoratorData<WeaponData, float>(
                weaponName,
                PlayerSaveLoader.SO_WEAPON_PATH,
                data => data.rangeIncrease);

            EventManager.Instance.Notify(new HitTarget(Owner, attackRangeBuff, target));

            m_currentPierceCount++;

            if (m_currentPierceCount >= maxPierceCount)
            {
                ReturnToPool();
            }
        }

        #endregion
    }
}