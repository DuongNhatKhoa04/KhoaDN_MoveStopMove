using MoveStopMove.Gameplay.Camera;
using MoveStopMove.Presentation.UI.Shops.Weapon;
using UnityEngine;

namespace MoveStopMove.Presentation.UI.Main
{
    public class UIMain : UICanvas
    {
        public void OnClickWeapon()
        {
            UIManager.Instance.OpenUI<UIWeaponShop>();
            UIManager.Instance.CloseUI<UIMain>();
        }

        public void OnClickOutfit()
        {

        }
    }
}