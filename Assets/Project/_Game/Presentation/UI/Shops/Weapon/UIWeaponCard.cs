using System;
using MoveStopMove.Core.Events;
using MoveStopMove.Core.Interfaces;
using MoveStopMove.Core.SaveLoad.Data;
using MoveStopMove.Core.Stats;
using MoveStopMove.Gameplay.Shops;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MoveStopMove.Presentation.UI.Shops.Weapon
{
    public class UIWeaponCard : MonoBehaviour, IDataPersistence
    {
        [SerializeField] private Image iconWeapon;
        [Header("Text")]
        [SerializeField] private TextMeshProUGUI weaponName;
        [SerializeField] private TextMeshProUGUI weaponRangeIncrease;
        [SerializeField] private TextMeshProUGUI weaponSkill;
        [SerializeField] private TextMeshProUGUI weaponMaxRange;
        [SerializeField] private TextMeshProUGUI weaponPrice;
        [Header("Buttons")]
        [SerializeField] private GameObject buttonBuy;
        [SerializeField] private GameObject buttonEquip;
        [SerializeField] private GameObject buttonCanEquip;

        private WeaponShopPresenter m_presenter;
        private WeaponData m_weaponData;
        //private UIWeaponShop m_weaponShopManager;
        private bool m_isLocked;

        #region -- Properties --

        public Sprite Icon { get; set; }
        public string WeaponName { get; set; }
        public float WeaponRangeIncrease { get; set; }
        public string WeaponSkill { get; set; }
        public float WeaponMaxRange { get; set; }
        public int WeaponPrice { get; set; }
        public WeaponData WeaponData => m_weaponData;

        #endregion

        public void Initialize(WeaponShopPresenter presenter)
        {
            m_presenter = presenter;
        }

        public void SetDataToCard()
        {
            iconWeapon.sprite = Icon;
            weaponName.text = BuildStatsString("Name: ", WeaponName);
            weaponSkill.text = BuildStatsString("Skill: ", WeaponSkill);
            weaponRangeIncrease.text = BuildStatsString("Range up: ", WeaponRangeIncrease.ToString());
            weaponMaxRange.text = BuildStatsString("Max range: ", WeaponMaxRange.ToString());
            weaponPrice.text = WeaponPrice.ToString();
        }

        public void SetLockedWeapons(bool isLocked)
        {
            m_isLocked = isLocked;

            if (m_isLocked)
            {
                buttonBuy.SetActive(true);
                buttonCanEquip.SetActive(false);
            }
            else
            {
                buttonBuy.SetActive(false);
                buttonCanEquip.SetActive(true);
            }
        }

        private string BuildStatsString(string title, string info)
        {
            return $"{title}{info}";
        }

        public void OnClickEquipButton()
        {
            //m_weaponShopManager.EquipWeapon(this);

            buttonEquip.SetActive(true);
            buttonCanEquip.SetActive(false);
            //EventManager.Instance.Notify(new NotificationPopUpEvent(EEventCode.EquipSuccess, "Đã trang bị thành công"));
        }

        public void DeactivateEquipButton()
        {
            buttonEquip.SetActive(false);
            buttonCanEquip.SetActive(true);
        }

        public void LoadData(GameData data)
        {
            throw new NotImplementedException();
        }

        public void SaveData(GameData data)
        {
            throw new NotImplementedException();
        }
    }
}