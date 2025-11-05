using MoveStopMove.Extensions.ObjectPooling;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.Serialization;

namespace MoveStopMove.Weapon
{
    public class NormalWeapon : WeaponBase
    {
        [Header("Projectile Settings")]
        [SerializeField] private float projectileSpeed = 20f;

        private IObjectPool<ProjectileBase> m_projectilePool;
        private Transform m_firePoint;

        public void Initialize()
        {

        }

        /*// Combat sẽ gọi hàm này để inject pool vào
        public void SetPool(IObjectPool<ProjectileBase> pool)
        {
            m_projectilePool = pool;
        }

        // Dùng cho ObjectPool (Combat sẽ truyền vào)
        public ProjectileBase CreateProjectile()
        {
            var projectile = Instantiate(projectilePrefab);
            return projectile;
        }
        public void OnGetProjectile(ProjectileBase projectile)
        {
            projectile.gameObject.SetActive(true);
        }
        public void OnReleaseProjectile(ProjectileBase projectile)
        {
            projectile.gameObject.SetActive(false);
        }
        public void OnDestroyProjectile(ProjectileBase projectile)
        {
            Destroy(projectile.gameObject);
        }*/

        public override void Attack(Vector3 targetPosition)
        {
            if (projectilePooling == null)
            {
                Debug.LogWarning("NormalWeapon: projectilePrefab chưa được gán!");
                return;
            }

            Vector3 spawnPos = m_firePoint ? m_firePoint.position : transform.position;
            Vector3 dir = (targetPosition - spawnPos).normalized;

            // Lấy projectile từ pool
            ProjectileBase projectile = m_projectilePool?.Get() ?? Instantiate(projectilePooling.projectilePrefab);
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