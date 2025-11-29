using MoveStopMove.Core.Stats;
using MoveStopMove.Utility.Extension;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MoveStopMove.Presentation.UI.Shops.Outfit
{
    public class UIPantCard : MonoBehaviour
    {
        #region -- Fields --

        [SerializeField] private Image iconPant;

        [Header("Text")]
        [SerializeField] private TextMeshProUGUI pantName;
        [SerializeField] private TextMeshProUGUI pantMovementIncrease;
        [SerializeField] private TextMeshProUGUI pantPrice;

        [Header("Buttons Root")]
        [SerializeField] private GameObject buyButton;
        [SerializeField] private GameObject tryButton;
        [SerializeField] private GameObject equipButton;
        [SerializeField] private GameObject canEquipButton;

        private UIOutfitShop m_shop;
        private PantData m_pantData;
        private bool m_isLocked;
        private bool m_isEquipped;

        #endregion

        #region -- Properties --

        public PantData PantData => m_pantData;

        #endregion

        #region -- Methods --

        /// <summary>
        /// Init data for pant card
        /// </summary>
        /// <param name="shop">Shop UI</param>
        /// <param name="data">Pant data</param>
        /// <param name="isLocked">Is pant locked?</param>
        /// <param name="isEquipped">Is pant equipped?</param>
        public void Initialize(UIOutfitShop shop, PantData data, bool isLocked, bool isEquipped)
        {
            m_shop = shop;
            m_pantData = data;
            m_isLocked = isLocked;
            m_isEquipped = isEquipped;

            SetDataToCard();
            RefreshState();
        }

        /// <summary>
        /// Setup card information
        /// </summary>
        private void SetDataToCard()
        {
            if (m_pantData == null) return;

            iconPant.sprite = m_pantData.icon;
            pantName.text = DataPersistenceHelpers.BuildStatsString("Name: ", m_pantData.name);
            pantMovementIncrease.text = DataPersistenceHelpers.BuildStatsString("Range up: ", m_pantData.movementIncrease.ToString());
            pantPrice.text = m_pantData.price.ToString();
        }

        /// <summary>
        /// Refresh state off each button for button click event
        /// </summary>
        private void RefreshState()
        {
            if (m_isLocked)
            {
                buyButton.SetActive(true);
                tryButton.SetActive(true);
                canEquipButton.SetActive(false);
                equipButton.SetActive(false);
            }
            else
            {
                buyButton.SetActive(false);
                tryButton.SetActive(false);
                canEquipButton.SetActive(!m_isEquipped);
                equipButton.SetActive(m_isEquipped);
            }
        }

        /// <summary>
        /// For locked pant
        /// </summary>
        /// <param name="value">Pant is locked or not</param>
        public void SetLocked(bool value)
        {
            m_isLocked = value;
            RefreshState();
        }

        /// <summary>
        /// For equipped pant
        /// </summary>
        /// <param name="value">Pant is equipped or not</param>
        public void SetEquipped(bool value)
        {
            m_isEquipped = value;
            RefreshState();
        }

        public void OnClickBuy()
        {
            m_shop.OnClickBuyPant(this);
        }

        public void OnClickEquip()
        {
            m_shop.OnClickEquipPant(this);
        }

        public void OnClickTry()
        {
            m_shop.OnClickTryPant(this);
        }

        #endregion
    }
}