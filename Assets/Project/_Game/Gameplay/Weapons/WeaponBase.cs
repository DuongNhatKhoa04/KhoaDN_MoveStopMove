using MoveStopMove.Core.Combat;
using MoveStopMove.Gameplay.Projectiles;
using UnityEngine;
using UnityEngine.Pool;

namespace MoveStopMove.Gameplay.Weapons
{
    public abstract class WeaponBase : MonoBehaviour
    {
        #region -- Fields --

        [Header("Base Settings")]
        [SerializeField] protected GameObject attacker;

        [Header("Projectile")]
        [SerializeField] protected ProjectileBase projectile;

        [Header("Spawn Settings")]
        [SerializeField] protected Transform firePoint;

        protected IAttackStrategy AttackStrategy;
        protected IObjectPool<ProjectileBase> ProjectileObjectPool;

        private string m_currentWeapon;

        #endregion

        public ProjectileBase Projectile
        {
            get => projectile;
            set => projectile = value;
        }

        #region -- Methods --

        protected virtual void Awake()
        {
            m_currentWeapon = gameObject.name.Replace("(Clone)", "").Trim();

            if (attacker == null)
            {
                attacker = GameObject.FindGameObjectWithTag("Player");

                if (attacker == null)
                {
                    attacker = GameObject.FindGameObjectWithTag("Enemy");
                }
            }

            ProjectileObjectPool = new ObjectPool<ProjectileBase>
            (
                CreateProjectile,
                OnGetFromPool,
                OnReleaseToPool,
                OnDestroyPooledObject,
                true,
                20,
                100
            );
        }

        private ProjectileBase CreateProjectile()
        {
            var projectInstance = Instantiate(projectile);
            projectInstance.weaponName = m_currentWeapon;
            projectInstance.ObjectPool = ProjectileObjectPool;
            return projectInstance;
        }

        private void OnGetFromPool(ProjectileBase pooledObject)
        {
            pooledObject.gameObject.SetActive(true);
        }

        private void OnReleaseToPool(ProjectileBase pooledObject)
        {
            pooledObject.gameObject.SetActive(false);
        }

        private void OnDestroyPooledObject(ProjectileBase pooledObject)
        {
            Destroy(pooledObject.gameObject);
        }

        public abstract void Attack(Vector3 targetPosition);

        #endregion
    }
}