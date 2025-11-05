using System;
using MoveStopMove.Core.CoreComponents;
using MoveStopMove.Extensions.ObjectPooling;
using MoveStopMove.Managers;
using UnityEngine;

namespace MoveStopMove.Weapon
{
    [System.Serializable]
    public class WeaponMode
    {
        public bool normal = true;
        public bool canPierce;
        public bool chainable;
        public bool returnable;

        public void ValidateMode()
        {
            int trueCount = 0;

            if (normal) trueCount++;
            if (canPierce) trueCount++;
            if (chainable) trueCount++;
            if (returnable) trueCount++;

            if (trueCount > 1)
            {
                if (canPierce)
                {
                    chainable = false;
                    returnable = false;
                    normal = false;
                }
                else if (chainable)
                {
                    canPierce = false;
                    returnable = false;
                    normal = false;
                }
                else if (returnable)
                {
                    canPierce = false;
                    chainable = false;
                    normal = false;
                }
                else if (normal)
                {
                    canPierce = false;
                    chainable = false;
                    returnable = false;
                }
            }
        }

        public string GetActiveMode()
        {
            if (canPierce) return "Piercing";
            if (chainable) return "Chainable";
            if (returnable) return "Returnable";
            return "Normal";
        }
    }

    public abstract class WeaponBase : MonoBehaviour
    {
        [Header("Base Settings")]
        [SerializeField] protected GameObject attacker;

        [Header("Pierce Settings")]
        [SerializeField] protected int maxPierce = 1;

        [SerializeField] protected WeaponMode weaponMode;

        [SerializeField] protected ProjectileObjectPool projectilePooling;

        protected int PierceCount;
        protected bool Returning;
        protected bool Chaining;

        public ProjectileObjectPool ProjectilePooling => projectilePooling;

        protected virtual void OnValidate()
        {
            if (weaponMode != null)
                weaponMode.ValidateMode();
        }

        protected virtual void Awake()
        {
            PierceCount = 0;
            Returning = false;
            Chaining = false;

            weaponMode?.ValidateMode();
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

        public string GetActiveWeaponMode()
        {
            return weaponMode?.GetActiveMode() ?? "Normal";
        }
    }
}