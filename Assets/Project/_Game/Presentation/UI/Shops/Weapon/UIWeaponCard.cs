using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MoveStopMove.Presentation.UI.Shops.Weapon
{
    public class UIWeaponCard : MonoBehaviour
    {
        [SerializeField] private Image iconWeapon;
        [SerializeField] private TextMeshProUGUI weaponName;
        [SerializeField] private TextMeshProUGUI weaponBuff;
        [SerializeField] private TextMeshProUGUI weaponSkill;
        [SerializeField] private TextMeshProUGUI weaponMaxRange;
        [SerializeField] private TextMeshProUGUI weaponPrice;

        public Sprite Icon { get; set; }
        public string WeaponName { get; set; }
        public float WeaponBuff { get; set; }
        public string WeaponSkill { get; set; }
        public float WeaponMaxRange { get; set; }
        public float WeaponPrice { get; set; }

        private void Start()
        {
            
        }
    }
}