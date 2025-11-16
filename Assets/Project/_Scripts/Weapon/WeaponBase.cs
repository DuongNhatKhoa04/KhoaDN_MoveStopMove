using MoveStopMove.Managers;
using MoveStopMove.SO;
using MoveStopMove.Weapon.Projectile;
using UnityEngine;
using UnityEngine.Pool;

namespace MoveStopMove.Weapon
{
    public abstract class WeaponBase : MonoBehaviour
    {
        #region -- Fields --

        [Header("Base Settings")]
        [SerializeField] protected GameObject attacker;

        [Header("Projectile")]
        [SerializeField] protected ProjectileBase projectile;

        protected IObjectPool<ProjectileBase> ProjectileObjectPool;

        protected int PierceCount;
        protected bool Returning;
        protected bool Chaining;

        #endregion

        public ProjectileBase Projectile
        {
            get => projectile;
            set => projectile = value;
        }

        #region -- Methods --

        protected virtual void Awake()
        {
            PierceCount = 0;
            Returning   = false;
            Chaining    = false;

            if (attacker == null)
            {
                attacker = GameObject.FindGameObjectWithTag("Player");
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

        protected virtual void OnTriggerEnter(Collider other)
        {
            if (other.attachedRigidbody == null) return;

            var target = other.attachedRigidbody.gameObject;
            if (target == attacker) return;

            OnHitTarget(target);
        }

        protected virtual void OnHitTarget(GameObject target)
        {
            EventManager.Notify(new HitEvent(attacker, target));
        }

        public virtual void SetWeaponScriptableObject(WeaponData newWeaponScriptableObject)
        {
            //weaponSO = newWeaponScriptableObject;
        }

        public virtual void OnFirePointFound(Transform firePointTransform)
        {
            // Lớp con override nếu cần
        }

        #endregion
    }
}