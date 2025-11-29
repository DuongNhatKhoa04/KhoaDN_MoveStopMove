using System.Collections;
using System.Collections.Generic;
using MoveStopMove.Core.Events;
using MoveStopMove.Core.SaveLoad;
using MoveStopMove.Gameplay.Camera;
using MoveStopMove.Gameplay.Items;
using MoveStopMove.Presentation.UI.Main;
using MoveStopMove.Presentation.UI.Shops.Weapon;
using MoveStopMove.Utility;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MoveStopMove.Presentation.UI.Shops.Outfit
{
    public enum EOutfitCategory
    {
        Pant,
        Hair,
        Custom
    }

    public class UIOutfitShop : UICanvas
    {
        #region -- Fields --

        [Header("References")]
        [SerializeField] private Transform itemContext;
        [SerializeField] private TextMeshProUGUI coin;
        [SerializeField] private Button pantSelector;
        [SerializeField] private Button hairSelector;
        [SerializeField] private Button customSelector;

        [Header("Color Settings")]
        [SerializeField] private Color normalTabColor = new(0.2f, 0.7f, 0.6f);
        [SerializeField] private Color selectedTabColor = new(0f, 0.4f, 0.3f);


        private readonly List<UICustomCard> m_customCards = new();
        private readonly List<UIHairCard> m_hairCards = new();
        private readonly List<UIPantCard> m_pantCards = new();

        private UICustomCard m_currentEquippedCustomCard;
        private UIHairCard m_currentEquippedHairCard;
        private UIPantCard m_currentEquippedPantCard;

        #endregion

        #region -- Properties --

        public bool IsLoaded { get; private set; }

        #endregion

        #region -- Methods --

        public void OnEnable()
        {
            CameraFollower.Instance.offset = new Vector3(0f, 5f, -12f);

            if (!IsLoaded)
                StartCoroutine(LoadOutfits());
            else
                ShowCategory(EOutfitCategory.Pant);

            coin.text = DataPersistenceManager.Instance.GameData.coins.ToString();
        }

        /// <summary>
        /// Load data from file and spawn item cards
        /// </summary>
        /// <returns>If loaded, set IsLoaded is true</returns>
        private IEnumerator LoadOutfits()
        {
            yield return new WaitUntil(() => ItemManager.Instance.IsDataLoaded);

            //PANT
            var unlockedPants = ItemManager.Instance.UnlockedPants;
            var lockedPants   = ItemManager.Instance.LockedPants;
            var equippedPant  = ItemManager.Instance.EquippedPant;

            foreach (var pant in unlockedPants)
            {
                var card = ObjectPoolingManager.Instance.GetObjectFromPool<UIPantCard>("PantCardPool");
                card.transform.SetParent(itemContext, false);

                bool isEquipped = (pant == equippedPant);
                card.Initialize(this, pant, false, isEquipped);

                if (isEquipped)
                    m_currentEquippedPantCard = card;

                m_pantCards.Add(card);
            }

            foreach (var pant in lockedPants)
            {
                var card = ObjectPoolingManager.Instance.GetObjectFromPool<UIPantCard>("PantCardPool");
                card.transform.SetParent(itemContext, false);
                card.Initialize(this, pant, isLocked: true, isEquipped: false);

                m_pantCards.Add(card);
            }

            //HAIR
            var unlockedHairs = ItemManager.Instance.UnlockedHairs;
            var lockedHairs   = ItemManager.Instance.LockedHairs;
            var equippedHair  = ItemManager.Instance.EquippedHair;

            foreach (var hair in unlockedHairs)
            {
                var card = ObjectPoolingManager.Instance.GetObjectFromPool<UIHairCard>("HairCardPool");
                card.transform.SetParent(itemContext, false);

                bool isEquipped = (hair == equippedHair);
                card.Initialize(this, hair, false, isEquipped);

                if (isEquipped)
                    m_currentEquippedHairCard = card;

                m_hairCards.Add(card);
            }

            foreach (var hair in lockedHairs)
            {
                var card = ObjectPoolingManager.Instance.GetObjectFromPool<UIHairCard>("HairCardPool");
                card.transform.SetParent(itemContext, false);
                card.Initialize(this, hair, isLocked: true, isEquipped: false);

                m_hairCards.Add(card);
            }

            //CUSTOM
            var unlockedCustoms = ItemManager.Instance.UnlockedCustoms;
            var lockedCustoms   = ItemManager.Instance.LockedCustoms;
            var equippedCustom  = ItemManager.Instance.EquippedCustom;

            foreach (var custom in unlockedCustoms)
            {
                var card = ObjectPoolingManager.Instance.GetObjectFromPool<UICustomCard>("CustomCardPool");
                card.transform.SetParent(itemContext, false);

                bool isEquipped = (custom == equippedCustom);
                card.Initialize(this, custom, false, isEquipped);

                if (isEquipped)
                    m_currentEquippedCustomCard = card;

                m_customCards.Add(card);
            }

            foreach (var custom in lockedCustoms)
            {
                var card = ObjectPoolingManager.Instance.GetObjectFromPool<UICustomCard>("CustomCardPool");
                card.transform.SetParent(itemContext, false);
                card.Initialize(this, custom, isLocked: true, isEquipped: false);

                m_customCards.Add(card);
            }

            IsLoaded = true;
            ShowCategory(EOutfitCategory.Pant);
        }

        /// <summary>
        /// For custom card call when click buy button
        /// </summary>
        /// <param name="card">Specific custom</param>
        public void OnClickBuyCustom(UICustomCard card)
        {
            var custom = card.CustomData;
            if (custom == null) return;

            bool success = ItemManager.Instance.TryBuyCustom(custom);

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
        }

        /// <summary>
        /// For custom card call when click equip button
        /// </summary>
        /// <param name="card">Specific custom</param>
        public void OnClickEquipCustom(UICustomCard card)
        {
            var custom = card.CustomData;
            if (custom == null) return;

            bool success = ItemManager.Instance.TryEquipCustom(custom);

            if (success)
            {
                if (m_currentEquippedCustomCard != null)
                    m_currentEquippedCustomCard.SetEquipped(false);

                m_currentEquippedCustomCard = card;
                card.SetEquipped(true);

                DataPersistenceManager.Instance.SaveGame();
                Debug.Log(card.CustomData.name);
                EventManager.Instance.Notify(new ItemEquippedEvent(EItem.Custom, card.CustomData.name));

                EventManager.Instance.Notify(
                    new NotificationPopUpEvent(EEventCode.EquipSuccess)
                );
            }
        }

        /// <summary>
        /// For hair card call when click buy button
        /// </summary>
        /// <param name="card">Specific hair</param>
        public void OnClickBuyHair(UIHairCard card)
        {
            var hair = card.HairData;
            if (hair == null) return;

            bool success = ItemManager.Instance.TryBuyHair(hair);

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
        }

        /// <summary>
        /// For hair card call when click equip button
        /// </summary>
        /// <param name="card">Specific hair</param>
        public void OnClickEquipHair(UIHairCard card)
        {
            var hair = card.HairData;
            if (hair == null) return;

            bool success = ItemManager.Instance.TryEquipHair(hair);

            if (success)
            {
                if (m_currentEquippedHairCard != null)
                    m_currentEquippedHairCard.SetEquipped(false);

                m_currentEquippedHairCard = card;
                card.SetEquipped(true);

                DataPersistenceManager.Instance.SaveGame();
                Debug.Log(card.HairData.name);
                EventManager.Instance.Notify(new ItemEquippedEvent(EItem.Hair, card.HairData.name));

                EventManager.Instance.Notify(
                    new NotificationPopUpEvent(EEventCode.EquipSuccess)
                );
            }
        }

        /// <summary>
        /// For pant card call when click buy button
        /// </summary>
        /// <param name="card">Specific pant</param>
        public void OnClickBuyPant(UIPantCard card)
        {
            var pant = card.PantData;
            if (pant == null) return;

            bool success = ItemManager.Instance.TryBuyPant(pant);

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
        }

        /// <summary>
        /// For pant card call when click equip button
        /// </summary>
        /// <param name="card">Specific pant</param>
        public void OnClickEquipPant(UIPantCard card)
        {
            var pant = card.PantData;
            if (pant == null) return;

            bool success = ItemManager.Instance.TryEquipPant(pant);

            if (success)
            {
                if (m_currentEquippedPantCard != null)
                    m_currentEquippedPantCard.SetEquipped(false);

                m_currentEquippedPantCard = card;
                card.SetEquipped(true);

                DataPersistenceManager.Instance.SaveGame();

                EventManager.Instance.Notify(new ItemEquippedEvent(EItem.Pant, card.PantData.name));

                EventManager.Instance.Notify(
                    new NotificationPopUpEvent(EEventCode.EquipSuccess)
                );
            }
        }

        public void OnClickTabPant()
        {
            ShowCategory(EOutfitCategory.Pant);
        }

        public void OnClickTabHair()
        {
            ShowCategory(EOutfitCategory.Hair);
        }

        public void OnClickTabCustom()
        {
            ShowCategory(EOutfitCategory.Custom);
        }

        public void OnClickBackButton()
        {
            EventManager.Instance.Notify(new ItemCancelTryEvent());

            UIManager.Instance.OpenUI<UIMain>();
            UIManager.Instance.CloseUI<UIOutfitShop>();
        }

        public void OnClickTryPant(UIPantCard card)
        {
            EventManager.Instance.Notify(new ItemTryEvent(EItem.Pant, card.PantData.name));
        }

        public void OnClickTryHair(UIHairCard card)
        {
            EventManager.Instance.Notify(new ItemTryEvent(EItem.Hair, card.HairData.name));
        }

        public void OnClickTryCustom(UICustomCard card)
        {
            EventManager.Instance.Notify(new ItemTryEvent(EItem.Custom, card.CustomData.name));
        }

        /// <summary>
        /// Show item card for each selector
        /// </summary>
        /// <param name="category"></param>
        private void ShowCategory(EOutfitCategory category)
        {
            foreach (var card in m_pantCards)
                card.gameObject.SetActive(category == EOutfitCategory.Pant);

            foreach (var card in m_hairCards)
                card.gameObject.SetActive(category == EOutfitCategory.Hair);

            foreach (var card in m_customCards)
                card.gameObject.SetActive(category == EOutfitCategory.Custom);

            UpdateTabVisual(category);
        }

        private void UpdateTabVisual(EOutfitCategory selected)
        {
            SetTabButton(pantSelector, selected == EOutfitCategory.Pant);
            SetTabButton(hairSelector, selected == EOutfitCategory.Hair);
            SetTabButton(customSelector, selected == EOutfitCategory.Custom);
        }

        private void SetTabButton(Button btn, bool isSelected)
        {
            var img = btn.image;
            if (img == null) return;

            img.color = isSelected ? selectedTabColor : normalTabColor;
        }

        #endregion
    }
}