using System.Text;
using MoveStopMove.Core.Stats;
using MoveStopMove.Utility.Extension;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MoveStopMove.Presentation.UI.Shops.Outfit
{
    public class UICustomCard : MonoBehaviour
    {
        #region -- Fields --

        [SerializeField] private Image iconCustom;

        [Header("Text")]
        [SerializeField] private TextMeshProUGUI customName;
        [SerializeField] private TextMeshProUGUI customRangeIncrease;
        [SerializeField] private TextMeshProUGUI customMovementIncrease;
        [SerializeField] private TextMeshProUGUI customPrice;

        [Header("Buttons Root")]
        [SerializeField] private GameObject buyButton;
        [SerializeField] private GameObject tryButton;
        [SerializeField] private GameObject equipButton;
        [SerializeField] private GameObject canEquipButton;

        private UIOutfitShop m_shop;
        private CustomData m_customData;
        private bool m_isLocked;
        private bool m_isEquipped;

        #endregion

        #region -- Properties --

        public CustomData CustomData => m_customData;

        #endregion

        #region -- Methods --

        /// <summary>
        /// Init data for custom card
        /// </summary>
        /// <param name="shop">Shop UI</param>
        /// <param name="data">Custom data</param>
        /// <param name="isLocked">Is custom locked?</param>
        /// <param name="isEquipped">Is custom equipped?</param>
        public void Initialize(UIOutfitShop shop, CustomData data, bool isLocked, bool isEquipped)
        {
            m_shop = shop;
            m_customData = data;
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
            if (m_customData == null) return;

            iconCustom.sprite = m_customData.icon;
            customName.text = DataPersistenceHelpers.BuildStatsString("Name: ", m_customData.name);
            customRangeIncrease.text = DataPersistenceHelpers.BuildStatsString("Range up: ", m_customData.rangeIncrease.ToString());
            customMovementIncrease.text = DataPersistenceHelpers.BuildStatsString("Movement up: ", m_customData.movementIncrease.ToString());
            customPrice.text = m_customData.price.ToString();
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
        /// For locked custom
        /// </summary>
        /// <param name="value">Custom is locked or not</param>
        public void SetLocked(bool value)
        {
            m_isLocked = value;
            RefreshState();
        }

        /// <summary>
        /// For equipped custom
        /// </summary>
        /// <param name="value">Custom is equipped or not</param>
        public void SetEquipped(bool value)
        {
            m_isEquipped = value;
            RefreshState();
        }

        public void OnClickBuy()
        {
            m_shop.OnClickBuyCustom(this);
        }

        public void OnClickEquip()
        {
            m_shop.OnClickEquipCustom(this);
        }

        public void OnClickTry()
        {
            m_shop.OnClickTryCustom(this);
        }

        #endregion
    }
}