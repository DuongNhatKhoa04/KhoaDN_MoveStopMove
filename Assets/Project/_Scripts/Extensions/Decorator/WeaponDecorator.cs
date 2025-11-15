using MoveStopMove.Core;
using MoveStopMove.DataPersistence;
using MoveStopMove.Interfaces;
using UnityEngine;

namespace MoveStopMove.Extensions.Decorator
{
    public class WeaponDecorator : CharacterDecorator
    {
        private GameObject m_currentWeapon;

        public GameObject WeaponAttachment { get; set; }
        public GameObject WeaponPrefab  { get; set; }

        public WeaponDecorator(IDecoratable inner) : base(inner)
        {
            Debug.Log("WeaponDecoration");
        }

        public override void EquipWeapon()
        {
            base.EquipWeapon();
            //Debug.Log(WeaponPrefab.name);

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

            if (WeaponPrefab != null)
            {
                m_currentWeapon = Object.Instantiate(WeaponPrefab, WeaponAttachment.transform);
            }
        }
    }
}