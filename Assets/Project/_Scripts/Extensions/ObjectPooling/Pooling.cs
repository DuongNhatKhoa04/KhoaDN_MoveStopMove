using System.Collections.Generic;
using MoveStopMove.Weapon;
using UnityEngine;
using UnityEngine.Pool;

namespace MoveStopMove.Extensions.ObjectPooling
{
    public class ProjectileObjectPool : MonoBehaviour
    {
        public ProjectileBase projectilePrefab;
        private IObjectPool<ProjectileBase> m_projectilePool;
        private Transform m_firePoint;

        // Combat sẽ gọi hàm này để inject pool vào
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
        }
    }
}