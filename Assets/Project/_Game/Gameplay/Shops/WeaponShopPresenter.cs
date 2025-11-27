using MoveStopMove.Core.Shops;
using MoveStopMove.Presentation.UI.Shops.Weapon;
using UnityEngine;

namespace MoveStopMove.Gameplay.Shops
{
    public class WeaponShopPresenter : MonoBehaviour
    {
        [SerializeField] private UIWeaponShop view;

        private ShopModel m_model;

        private void Start()
        {
            m_model = new ShopModel();
        }
    }
}