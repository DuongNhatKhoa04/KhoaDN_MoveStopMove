using System.Collections;
using System.Collections.Generic;
using MoveStopMove.Core.Events;
using MoveStopMove.Core.SaveLoad;
using MoveStopMove.Gameplay.Items;
using MoveStopMove.Presentation.UI.Shops.Weapon;
using MoveStopMove.Utility;
using UnityEngine;

namespace MoveStopMove.Presentation.UI.Shops.Outfit
{
    public class UIOutfitShop : MonoBehaviour
    {
        #region -- Fields --

        [Header("References")]
        [SerializeField] private Transform itemContext;

        private readonly List<UIWeaponCard> m_cards = new();
        private UIWeaponCard m_currentEquippedCard;

        #endregion

        #region -- Properties --

        public bool IsLoaded { get; private set; }

        #endregion

        #region -- Methods --

        private void Start()
        {
            StartCoroutine(LoadWeapons());
        }

        /// <summary>
        /// Load data from file and spawn weapon cards
        /// </summary>
        /// <returns>If loaded, set IsLoaded is true</returns>
        private IEnumerator LoadWeapons()
        {
            yield return new WaitUntil(() => ItemManager.Instance.IsDataLoaded);

            var unlocked = ItemManager.Instance.UnlockedWeapons;
            var locked = ItemManager.Instance.LockedWeapons;
            var equipped = ItemManager.Instance.EquippedWeapon;

            foreach (var weapon in unlocked)
            {
                var card = ObjectPoolingManager.Instance.GetObjectFromPool<UIWeaponCard>("WeaponCardPool");

                card.transform.SetParent(itemContext, false);

                bool isEquipped = (weapon == equipped);
                //card.Initialize(this, weapon, false, isEquipped);

                if (isEquipped)
                    m_currentEquippedCard = card;

                m_cards.Add(card);
            }

            foreach (var weapon in locked)
            {
                var card = ObjectPoolingManager.Instance
                    .GetObjectFromPool<UIWeaponCard>("WeaponCardPool");

                card.transform.SetParent(itemContext, false);
                //card.Initialize(this, weapon, isLocked: true, isEquipped: false);

                m_cards.Add(card);
            }

            IsLoaded = true;
        }

        /// <summary>
        /// For weapon card call when click buy button
        /// </summary>
        /// <param name="card">Specific weapon</param>
        public void OnClickBuy(UIWeaponCard card)
        {
            var weapon = card.WeaponData;
            if (weapon == null) return;

            bool success = ItemManager.Instance.TryBuyWeapon(weapon);

            if (success)
            {
                card.SetLocked(false);

                DataPersistenceManager.Instance.SaveGame();

                EventManager.Instance.Notify(
                    new NotificationPopUpEvent(EEventCode.BuySuccess)
                );
            }
            else
            {
                EventManager.Instance.Notify(
                    new NotificationPopUpEvent(EEventCode.NotEnoughCoins)
                );
            }
            // Nếu fail thì ItemManager đã bắn event NotEnoughCoins,
            // UINotification sẽ popup.
        }

        /// <summary>
        /// For weapon card call when click equip button
        /// </summary>
        /// <param name="card">Specific weapon</param>
        public void OnClickEquip(UIWeaponCard card)
        {
            var weapon = card.WeaponData;
            if (weapon == null) return;

            bool success = ItemManager.Instance.TryEquipWeapon(weapon);

            if (success)
            {
                if (m_currentEquippedCard != null)
                    m_currentEquippedCard.SetEquipped(false);

                m_currentEquippedCard = card;
                card.SetEquipped(true);

                DataPersistenceManager.Instance.SaveGame();
                Debug.Log(card.WeaponData.name);
                EventManager.Instance.Notify(new ItemEquippedEvent(EItem.Weapon, card.WeaponData.name));

                EventManager.Instance.Notify(
                    new NotificationPopUpEvent(EEventCode.EquipSuccess)
                );
            }
        }

        #endregion
    }
}