using MoveStopMove.Extensions.Strategy;
using MoveStopMove.Weapon.Projectile;
using UnityEngine;

namespace MoveStopMove.Weapon
{
    public class PiercingWeapon : WeaponBase
    {
        [Header("Spawn Settings")]
        [SerializeField] private Transform firePoint;

        private IAttackStrategy m_attackStrategy;

        protected override void Awake()
        {
            base.Awake();

            if (firePoint == null)
            {
                firePoint = transform;
            }

            if (projectileObjectPool == null)
            {
                Debug.LogWarning("[PiercingWeapon] projectileObjectPool chưa được gán trên Inspector.");
            }

            m_attackStrategy = new PiercingAttackStrategy(this);
        }

        public override void Attack(Vector3 targetPosition)
        {
            if (m_attackStrategy == null)
            {
                m_attackStrategy = new PiercingAttackStrategy(this);
            }

            m_attackStrategy.PerformAttack(targetPosition);
        }

        public ProjectileBase SpawnPiercingProjectile(Vector3 targetPosition)
        {
            if (projectileObjectPool == null)
            {
                Debug.LogWarning("[PiercingWeapon] projectileObjectPool null.");
                return null;
            }

            if (firePoint == null)
            {
                firePoint = transform;
            }

            return projectileObjectPool.Spawn(
                firePoint.position,
                firePoint.rotation,
                attacker,
                targetPosition
            );
        }
    }
}