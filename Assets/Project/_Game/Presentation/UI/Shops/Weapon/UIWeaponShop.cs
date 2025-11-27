using System.Collections;
using System.Collections.Generic;
using MoveStopMove.Core.Events;
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
        private UIWeaponCard m_currentEquippedCard = null;
        private int m_currentCoins;

        public bool IsLoaded { get; private set; }

        private void Start()
        {
            StartCoroutine(LoadWeapons());
        }

        private IEnumerator LoadWeapons()
        {
            yield return new WaitUntil(() => ItemManager.Instance.IsDataLoaded);

            m_unlockedWeapons = ItemManager.Instance.GetUnlockedWeapons();
            m_lockedWeapons = ItemManager.Instance.GetLockedWeapons();

            foreach (var item in m_unlockedWeapons)
            {
                if (m_unlockedWeapons.Contains(item))
                {

                }
                var unlockWeapons = ObjectPoolingManager.Instance.GetObjectFromPool<UIWeaponCard>("WeaponCardPool");
                UpdateWeaponCard(unlockWeapons, item);
                unlockWeapons.SetLockedWeapons(false);
                unlockWeapons.SetDataToCard();
                unlockWeapons.transform.SetParent(itemContext);
                //unlockWeapons.Initialize(this);
                //Debug.Log(unlockWeapons.Icon.name + unlockWeapons.WeaponName + unlockWeapons.WeaponMaxRange + unlockWeapons.WeaponBuff + unlockWeapons.WeaponPrice + unlockWeapons.WeaponSkill);
            }

            foreach (var item in m_lockedWeapons)
            {
                var lockWeapons = ObjectPoolingManager.Instance.GetObjectFromPool<UIWeaponCard>("WeaponCardPool");
                UpdateWeaponCard(lockWeapons, item);
                lockWeapons.SetLockedWeapons(true);
                lockWeapons.SetDataToCard();
                lockWeapons.transform.SetParent(itemContext);
                //lockWeapons.Initialize(this);
                //Debug.Log(lockWeapons.Icon.name + lockWeapons.WeaponName + lockWeapons.WeaponMaxRange + lockWeapons.WeaponBuff + lockWeapons.WeaponPrice + lockWeapons.WeaponSkill);
            }

            IsLoaded = true;
        }

        private void UpdateWeaponCard(UIWeaponCard card, WeaponData item)
        {
            card.Icon = item.icon;
            card.WeaponName = item.name;
            card.WeaponMaxRange = item.maxAttackRange;
            card.WeaponRangeIncrease = item.rangeIncrease;
            card.WeaponPrice = item.price;
            card.WeaponSkill = item.weaponType.ToString();
        }

        public void EquipWeapon(UIWeaponCard weaponCard)
        {
            if (m_currentEquippedCard != null)
            {
                m_currentEquippedCard.DeactivateEquipButton();
            }

            m_currentEquippedCard = weaponCard;

            EventManager.Instance.Notify(new NotificationPopUpEvent(EEventCode.EquipSuccess));
        }

        public void BuyWeapon(string weaponName, int price)
        {
            if (m_currentCoins < price)
            {
                EventManager.Instance.Notify(new NotificationPopUpEvent(EEventCode.NotEnoughCoins));
            }
            else
            {

            }
        }
    }
}