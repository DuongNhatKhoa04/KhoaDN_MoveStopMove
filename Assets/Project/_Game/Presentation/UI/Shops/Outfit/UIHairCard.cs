using System.Text;
using MoveStopMove.Core.Stats;
using MoveStopMove.Utility.Extension;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MoveStopMove.Presentation.UI.Shops.Outfit
{
    public class UIHairCard : MonoBehaviour
    {
        #region -- Fields --

        [SerializeField] private Image iconHair;

        [Header("Text")]
        [SerializeField] private TextMeshProUGUI hairName;
        [SerializeField] private TextMeshProUGUI hairRangeIncrease;
        [SerializeField] private TextMeshProUGUI hairPrice;

        [Header("Buttons Root")]
        [SerializeField] private GameObject buyButton;
        [SerializeField] private GameObject tryButton;
        [SerializeField] private GameObject equipButton;
        [SerializeField] private GameObject canEquipButton;

        private UIOutfitShop m_shop;
        private HairData m_hairData;
        private bool m_isLocked;
        private bool m_isEquipped;

        #endregion

        #region -- Properties --

        public HairData HairData => m_hairData;

        #endregion

        #region -- Methods --

        /// <summary>
        /// Init data for hair card
        /// </summary>
        /// <param name="shop">Shop UI</param>
        /// <param name="data">Hair data</param>
        /// <param name="isLocked">Is hair locked?</param>
        /// <param name="isEquipped">Is hair equipped?</param>
        public void Initialize(UIOutfitShop shop, HairData data, bool isLocked, bool isEquipped)
        {
            m_shop = shop;
            m_hairData = data;
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
            if (m_hairData == null) return;

            iconHair.sprite = m_hairData.icon;
            hairName.text = DataPersistenceHelpers.BuildStatsString("Name: ", m_hairData.name);
            hairRangeIncrease.text = DataPersistenceHelpers.BuildStatsString("Range up: ", m_hairData.rangeIncrease.ToString());
            hairPrice.text = m_hairData.price.ToString();
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
        /// For locked hair
        /// </summary>
        /// <param name="value">Hair is locked or not</param>
        public void SetLocked(bool value)
        {
            m_isLocked = value;
            RefreshState();
        }

        /// <summary>
        /// For equipped hair
        /// </summary>
        /// <param name="value">Hair is equipped or not</param>
        public void SetEquipped(bool value)
        {
            m_isEquipped = value;
            RefreshState();
        }

        public void OnClickBuy()
        {
            m_shop.OnClickBuyHair(this);
        }

        public void OnClickEquip()
        {
            m_shop.OnClickEquipHair(this);
        }

        public void OnClickTry()
        {
            m_shop.OnClickTryHair(this);
        }

        #endregion
    }
}