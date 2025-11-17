using MoveStopMove.Weapon;
using MoveStopMove.Weapon.Projectile;
using UnityEngine;

namespace MoveStopMove.Core.CoreComponents
{
    public class Combat : CoreComponents
    {
        #region -- Fields --

        [SerializeField] private AttackRange attackRange;
        [SerializeField] private WeaponBase weapon;

        [SerializeField] private Transform ownerRoot;
        [SerializeField] private float rotateSpeed = 20f;

        #endregion

        #region -- Properties --

        public AttackRange GetAttackRange => attackRange;

        #endregion

        #region -- Methods --

        public void SetWeapon(WeaponBase newWeapon, ProjectileBase weaponProjectile)
        {
            weapon = newWeapon;
            weapon.Projectile = weaponProjectile;
        }

        public void Attack()
        {
            var targetEntry = attackRange.PeekEntry();
            if (targetEntry == null)
            {
                // Debug.Log("Không có kẻ địch trong vùng tấn công!");
                return;
            }

            var targetPos = AttackRange.GetTargetPosition(targetEntry.Value);
            RotateTowards(targetPos);
            weapon.Attack(targetPos);
        }

        public void RotateTowards(Vector3 targetPos)
        {
            if (ownerRoot == null)
                ownerRoot = transform;

            Vector3 dir = targetPos - ownerRoot.position;

            dir.y = 0f;

            if (dir.sqrMagnitude < 0.0001f)
                return;

            Quaternion targetRot = Quaternion.LookRotation(dir.normalized);

            // ownerRoot.rotation = targetRot;

            ownerRoot.rotation = Quaternion.Slerp(
                ownerRoot.rotation,
                targetRot,
                rotateSpeed * Time.deltaTime
            );
        }

        #endregion
    }
}