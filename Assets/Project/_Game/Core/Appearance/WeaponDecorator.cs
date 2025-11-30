using MoveStopMove.Core.Interfaces;
using MoveStopMove.Core.Units;
using MoveStopMove.Gameplay.Projectiles;
using MoveStopMove.Gameplay.Weapons;
using UnityEngine;

namespace MoveStopMove.Core.Appearance
{
    public class WeaponDecorator : CharacterDecorator
    {
        #region -- Fields --

        private GameObject m_currentWeapon;

        #endregion

        #region -- Properties --

        public GameObject WeaponAttachment { get; set; }
        public GameObject WeaponPrefab  { get; set; }
        public ProjectileBase ProjectilePrefab { get; set; }

        #endregion

        #region -- Methods --

        public WeaponDecorator(IDecoratable inner) : base(inner) { }

        /// <summary>
        /// Equip weapon to weapon attachment and setup weapon, projectile
        /// </summary>
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
                Core.Battle.SetWeapon(m_currentWeapon.GetComponent<WeaponBase>(), ProjectilePrefab);
            }
        }

        #endregion
    }
}