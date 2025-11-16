using MoveStopMove.Weapon;
using MoveStopMove.Weapon.Projectile;
using UnityEngine;

namespace MoveStopMove.Core.CoreComponents
{
    public class Combat : CoreComponents
    {
        [SerializeField] private AttackRange attackRange;
        [SerializeField] private WeaponBase weapon;

        public AttackRange GetAttackRange => attackRange;

        private new void Awake()
        {

        }

        public void SetWeapon(WeaponBase newWeapon, ProjectileBase weaponProjectile)
        {
            weapon = newWeapon;
            weapon.Projectile = weaponProjectile;
        }

        public void Attack()
        {
            var targetEntry = attackRange.PeekEntry();
            var targetPos= AttackRange.GetTargetPosition(targetEntry.Value);

            weapon.Attack(targetPos);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log("Attack");
                var targetEntry = attackRange.PeekEntry();
                if (targetEntry == null)
                {
                    Debug.Log("Không có kẻ địch trong vùng tấn công!");
                    return;
                }

                var target = targetEntry.Value.Target;
                Vector3 targetPosition = AttackRange.GetTargetPosition(targetEntry.Value);

                //Debug.Log($"Tấn công {target.name} bằng {weapon.GetActiveWeaponMode()}");

                weapon.Attack(targetPosition);
            }
        }
    }
}