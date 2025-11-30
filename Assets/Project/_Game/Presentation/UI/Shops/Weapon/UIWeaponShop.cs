using System.Collections;
using System.Collections.Generic;
using MoveStopMove.Core.Events;
using MoveStopMove.Core.SaveLoad;
using MoveStopMove.Gameplay.Camera;
using MoveStopMove.Gameplay.Items;
using MoveStopMove.Presentation.UI.Main;
using MoveStopMove.Utility;
using TMPro;
using UnityEngine;

namespace MoveStopMove.Presentation.UI.Shops.Weapon
{
    public class UIWeaponShop : UICanvas
    {
        #region -- Fields --

        [Header("References")]
        [SerializeField] private Transform itemContext;
        [SerializeField] private TextMeshProUGUI coin;

        private readonly List<UIWeaponCard> m_cards = new();
        private UIWeaponCard m_currentEquippedCard;

        #endregion

        #region -- Properties --

        public bool IsLoaded { get; private set; }

        #endregion

        #region -- Methods --

        public void OnEnable()
        {
            CameraFollower.Instance.offset = new Vector3(0f, 5f, -12f);

            if (!IsLoaded)
            {
                StartCoroutine(LoadWeapons());
            }

            coin.text = DataPersistenceManager.Instance.GameData.coins.ToString();
        }

        /// <summary>
        /// Load data from file and spawn weapon cards
        /// </summary>
        /// <returns>If loaded, set IsLoaded is true</returns>
        private IEnumerator LoadWeapons()
        {
            //Debug.Log("Loading");
            yield return new WaitUntil(() => ItemManager.Instance.IsDataLoaded);

            //
            //Debug.Log("Done");

            var unlocked = ItemManager.Instance.UnlockedWeapons;
            var locked = ItemManager.Instance.LockedWeapons;
            var equipped = ItemManager.Instance.EquippedWeapon;

            foreach (var weapon in unlocked)
            {
                var card = ObjectPoolingManager.Instance.GetObjectFromPool<UIWeaponCard>("WeaponCardPool");

                card.transform.SetParent(itemContext, false);

                bool isEquipped = (weapon == equipped);
                card.Initialize(this, weapon, false, isEquipped);

                if (isEquipped)
                    m_currentEquippedCard = card;

                m_cards.Add(card);
            }

            foreach (var weapon in locked)
            {
                var card = ObjectPoolingManager.Instance
                    .GetObjectFromPool<UIWeaponCard>("WeaponCardPool");

                card.transform.SetParent(itemContext, false);
                card.Initialize(this, weapon, isLocked: true, isEquipped: false);

                m_cards.Add(card);
            }

            IsLoaded = true;
        }

        /// <summary>
        /// For weapon card call when click buy button
        /// </summary>
        /// <param name="card">Specific weapon</param>
        public void OnClickBuyWeapon(UIWeaponCard card)
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
        public void OnClickEquipWeapon(UIWeaponCard card)
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

                EventManager.Instance.Notify(new ItemEquippedEvent(EItem.Weapon, card.WeaponData.name));

                EventManager.Instance.Notify(new NotificationPopUpEvent(EEventCode.EquipSuccess));
            }
        }

        public void OnClickTryWeapon(UIWeaponCard card)
        {
            EventManager.Instance.Notify(new ItemTryEvent(EItem.Weapon, card.WeaponData.name));
        }

        public void OnClickBackButton()
        {
            EventManager.Instance.Notify(new ItemCancelTryEvent());

            UIManager.Instance.OpenUI<UIMain>();
            UIManager.Instance.CloseUI<UIWeaponShop>();
        }

        #endregion
    }
}