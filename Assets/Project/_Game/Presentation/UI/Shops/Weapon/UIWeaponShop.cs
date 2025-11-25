using System.Collections.Generic;
using MoveStopMove.Core.Stats;
using MoveStopMove.Gameplay.Items;
using MoveStopMove.Utility;
using UnityEngine;

namespace MoveStopMove.Presentation.UI.Shops.Weapon
{
    public class UIWeaponShop : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Transform itemContext;

        private List<WeaponData> m_unlockedWeapons = new();
        private List<WeaponData> m_lockedWeapons = new();

        private void Start()
        {
            m_unlockedWeapons = ItemManager.Instance.GetUnlockedWeapons();
            m_lockedWeapons = ItemManager.Instance.GetLockedWeapons();

            foreach (var item in m_unlockedWeapons)
            {
                var unlockWeapons = ObjectPoolingManager.Instance.GetObjectFromPool<UIWeaponCard>("WeaponCardPool");
                unlockWeapons.Icon = item.icon;
                unlockWeapons.WeaponName = item.name;
                unlockWeapons.WeaponMaxRange = item.maxAttackRange;
                unlockWeapons.WeaponBuff = item.rangeIncrease;
                unlockWeapons.WeaponPrice = item.price;
                unlockWeapons.WeaponSkill = item.weaponType.ToString();
            }
        }
    }
}