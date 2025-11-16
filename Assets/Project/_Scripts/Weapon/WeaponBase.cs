using MoveStopMove.Extensions.ObjectPooling;
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

        [Header("Pierce Settings")]
        [SerializeField] protected int maxPierce = 1;

        [Header("Projectile Pool")]
        [SerializeField] protected ProjectileObjectPool projectileObjectPool;

        protected int PierceCount;
        protected bool Returning;
        protected bool Chaining;

        #endregion

        #region -- Properties --

        public ProjectileObjectPool ProjectilePooling => projectileObjectPool;
        public int MaxPierce => maxPierce;

        #endregion

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

        public virtual void OnProjectilePoolFound(ProjectileObjectPool foundProjectileObjectPool)
        {
            projectileObjectPool = foundProjectileObjectPool;
        }

        #endregion
    }
}