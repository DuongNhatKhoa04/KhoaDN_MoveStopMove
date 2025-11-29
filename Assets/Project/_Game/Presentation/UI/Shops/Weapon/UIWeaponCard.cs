using System.Text;
using MoveStopMove.Core.Stats;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace MoveStopMove.Presentation.UI.Shops.Weapon
{
    public class UIWeaponCard : MonoBehaviour
    {
        #region -- Fields --

        [SerializeField] private Image iconWeapon;

        [Header("Text")]
        [SerializeField] private TextMeshProUGUI weaponName;
        [SerializeField] private TextMeshProUGUI weaponRangeIncrease;
        [SerializeField] private TextMeshProUGUI weaponSkill;
        [SerializeField] private TextMeshProUGUI weaponMaxRange;
        [SerializeField] private TextMeshProUGUI weaponPrice;

        [Header("Buttons Root")]
        [SerializeField] private GameObject buyButton;
        [SerializeField] private GameObject tryButton;
        [SerializeField] private GameObject equipButton;
        [SerializeField] private GameObject canEquipButton;

        private UIWeaponShop m_shop;
        private WeaponData m_weaponData;
        private bool m_isLocked;
        private bool m_isEquipped;

        #endregion

        #region -- Properties --

        public WeaponData WeaponData => m_weaponData;

        #endregion

        #region -- Methods --

        /// <summary>
        /// Init data for weapon card
        /// </summary>
        /// <param name="shop">Shop UI</param>
        /// <param name="data">Weapon data</param>
        /// <param name="isLocked">Is weapon locked?</param>
        /// <param name="isEquipped">Is weapon equipped?</param>
        public void Initialize(UIWeaponShop shop, WeaponData data, bool isLocked, bool isEquipped)
        {
            m_shop = shop;
            m_weaponData = data;
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
            if (m_weaponData == null) return;

            iconWeapon.sprite = m_weaponData.icon;
            weaponName.text = BuildStatsString("Name: ", m_weaponData.name);
            weaponSkill.text = BuildStatsString("Skill: ", m_weaponData.weaponType.ToString());
            weaponRangeIncrease.text = BuildStatsString("Range up: ", m_weaponData.rangeIncrease.ToString());
            weaponMaxRange.text = BuildStatsString("Max range: ", m_weaponData.maxAttackRange.ToString());
            weaponPrice.text = m_weaponData.price.ToString();
        }

        /// <summary>
        /// Extensions for build string
        /// </summary>
        /// <param name="title">Title</param>
        /// <param name="info">Stats</param>
        /// <returns>Stats UI as string</returns>
        private string BuildStatsString(string title, string info)
        {
            var builder = new StringBuilder();
            builder.Append(title);
            builder.Append(info);
            return builder.ToString();
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
        /// For locked weapon
        /// </summary>
        /// <param name="value">Weapon is locked or not</param>
        public void SetLocked(bool value)
        {
            m_isLocked = value;
            RefreshState();
        }

        /// <summary>
        /// For equipped weapon
        /// </summary>
        /// <param name="value">Weapon is equipped or not</param>
        public void SetEquipped(bool value)
        {
            m_isEquipped = value;
            RefreshState();
        }

        public void OnClickBuy()
        {
            m_shop.OnClickBuyWeapon(this);
        }

        public void OnClickEquip()
        {
            m_shop.OnClickEquipWeapon(this);
        }

        public void OnClickTry()
        {
            m_shop.OnClickTryWeapon(this);
        }

        #endregion
    }
}
