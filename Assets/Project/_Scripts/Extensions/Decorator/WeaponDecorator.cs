using MoveStopMove.Core;
using MoveStopMove.Interfaces;
using MoveStopMove.Weapon;
using MoveStopMove.Weapon.Projectile;
using UnityEngine;

namespace MoveStopMove.Extensions.Decorator
{
    public class WeaponDecorator : CharacterDecorator
    {
        private GameObject m_currentWeapon;
        public GameObject WeaponAttachment { get; set; }
        public GameObject WeaponPrefab  { get; set; }
        public ProjectileBase ProjectilePrefab { get; set; }

        public WeaponDecorator(IDecoratable inner) : base(inner)
        {
            Debug.Log("WeaponDecoration");
        }

        public override void EquipWeapon()
        {
            base.EquipWeapon();

            if (WeaponAttachment == null)
            {
                Debug.Log("WeaponAttachment is not assigned");
                return;
            }

            if (m_currentWeapon != null)
            {
                Object.Destroy(m_currentWeapon);
                m_currentWeapon = null;
            }

            if (WeaponPrefab != null && ProjectilePrefab != null)
            {
                m_currentWeapon = Object.Instantiate(WeaponPrefab, WeaponAttachment.transform);
                Core.Combat.SetWeapon(m_currentWeapon.GetComponent<WeaponBase>(), ProjectilePrefab);
            }
        }
    }
}