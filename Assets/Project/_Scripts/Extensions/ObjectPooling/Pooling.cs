using MoveStopMove.Extensions.Helpers;
using MoveStopMove.Extensions.Singleton;
using MoveStopMove.SO;
using MoveStopMove.Weapon.Projectile;
using UnityEngine;
using UnityEngine.Pool;

namespace MoveStopMove.Extensions.ObjectPooling
{
    public class ProjectileObjectPool : MonoBehaviour
    {
        #region -- Fields --

        [Header("Config")]
        [SerializeField] private ProjectileBase projectilePrefab;
        [SerializeField] private int defaultCapacity = 10;
        [SerializeField] private int maxSize = 50;

        private IObjectPool<ProjectileBase> m_projectilePool;

        #endregion

        public ProjectileBase ProjectilePrefab { get; set; }

        #region -- Methods --

        private void Awake()
        {
            projectilePrefab = ProjectilePrefab;
            m_projectilePool = new ObjectPool<ProjectileBase>(
                CreateProjectile,
                OnGetProjectile,
                OnReleaseProjectile,
                OnDestroyProjectile,
                true,
                defaultCapacity,
                maxSize
            );
        }

        public ProjectileBase Spawn(Vector3 position, Quaternion rotation, GameObject owner, Vector3 targetPosition)
        {
            var projectile = m_projectilePool.Get();

            projectile.transform.SetPositionAndRotation(position, rotation);
            projectile.Initialize(owner, targetPosition);

            return projectile;
        }

        public void Despawn(ProjectileBase projectile)
        {
            if (projectile == null) return;
            m_projectilePool.Release(projectile);
        }

        private ProjectileBase CreateProjectile()
        {
            var projectile = Instantiate(projectilePrefab, transform);
            projectile.SetPool(m_projectilePool);
            return projectile;
        }

        private void OnGetProjectile(ProjectileBase projectile)
        {
            projectile.gameObject.SetActive(true);
        }

        private void OnReleaseProjectile(ProjectileBase projectile)
        {
            projectile.gameObject.SetActive(false);
        }

        private void OnDestroyProjectile(ProjectileBase projectile)
        {
            if (projectile != null)
            {
                Destroy(projectile.gameObject);
            }
        }

        public ProjectileBase GetProjectile(string weaponName)
        {
            return PlayerSaveLoader.GetDecoratorData<WeaponData, ProjectileBase>(
                weaponName,
                PlayerSaveLoader.SO_WEAPON_PATH,
                data => data.projectilePrefab);
        }

        public void SetProjectilePrefab(ProjectileBase prefab)
        {
            projectilePrefab = prefab;
        }

        #endregion
    }
}