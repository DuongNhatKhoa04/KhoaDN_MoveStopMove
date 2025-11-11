using MoveStopMove.Extensions.ObjectPooling;
using MoveStopMove.Managers;
using MoveStopMove.SO;
using UnityEngine;

namespace MoveStopMove.Weapon
{
    public abstract class WeaponBase : MonoBehaviour
    {
        [Header("Base Settings")]
        [SerializeField] protected GameObject attacker;

        [Header("Pierce Settings")]
        [SerializeField] protected int maxPierce = 1;

        //[SerializeField] protected WeaponMode weaponMode;

        [SerializeField] protected ProjectileObjectPool projectileObjectPool;

        //[SerializeField] protected WeaponData weaponSO;
        //protected PlayerSaveData playerSaveData;
        protected int PierceCount;
        protected bool Returning;
        protected bool Chaining;

        public ProjectileObjectPool ProjectilePooling => projectileObjectPool;

        protected virtual void Awake()
        {
            PierceCount = 0;
            Returning = false;
            Chaining = false;
            /*playerSaveData = new PlayerSaveData();
            playerSaveData = PlayerSaveLoader.LoadFromResourcesText("userGameplayData");
            Debug.Log($"{playerSaveData.equippedWeapon}");
            weaponSO = WeaponBinder.GetWeaponDataById(playerSaveData.equippedWeapon);*/
            attacker = GameObject.FindGameObjectWithTag("Player");
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
    }
}