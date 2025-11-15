using UnityEngine;
using UnityEngine.Pool;

namespace MoveStopMove.Weapon
{
    public class Weapon : WeaponBase
    {
        [Header("Projectile Settings")]
        [SerializeField] private float projectileSpeed = 20f;

        private IObjectPool<ProjectileBase> m_projectilePool;
        private Transform m_firePoint;

        public void Initialize()
        {

        }

        public override void Attack(Vector3 targetPosition)
        {
            if (projectileObjectPool == null)
            {
                /*Debug.LogWarning("Weapon: projectilePrefab chưa được gán!");*/
                return;
            }

            Vector3 spawnPos = m_firePoint ? m_firePoint.position : transform.position;
            Vector3 dir = (targetPosition - spawnPos).normalized;

            // Lấy projectile từ pool
            ProjectileBase projectile = m_projectilePool?.Get() ?? Instantiate(projectileObjectPool.projectilePrefab);
            projectile.transform.SetPositionAndRotation(spawnPos, Quaternion.LookRotation(dir));
            projectile.Initialize(attacker, targetPosition);

            // Nếu projectile có rigidbody thì set vận tốc
            var rb = projectile.GetComponent<Rigidbody>();
            if (rb != null)
                rb.velocity = dir * projectileSpeed;

            Debug.Log($"NormalWeapon: Bắn từ {spawnPos} tới {targetPosition}");
        }
    }
}