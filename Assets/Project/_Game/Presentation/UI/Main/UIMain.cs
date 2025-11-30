using MoveStopMove.Core;
using MoveStopMove.Gameplay.Camera;
using MoveStopMove.Presentation.UI.Setting;
using MoveStopMove.Presentation.UI.Shops.Outfit;
using MoveStopMove.Presentation.UI.Shops.Weapon;
using MoveStopMove.Utility.Audio;
using UnityEngine;

namespace MoveStopMove.Presentation.UI.Main
{
    public class UIMain : UICanvas
    {
        #region -- Methods --

        public void OnEnable()
        {
            CameraFollower.Instance.offset = new Vector3(0f, 5f, -6f);
        }

        public void OnClickWeapon()
        {
            SoundManager.Instance.PlaySFX(ESfxType.ButtonClick);
            UIManager.Instance.OpenUI<UIWeaponShop>();
            UIManager.Instance.CloseUI<UIMain>();
        }

        public void OnClickOutfit()
        {
            SoundManager.Instance.PlaySFX(ESfxType.ButtonClick);
            UIManager.Instance.OpenUI<UIOutfitShop>();
            UIManager.Instance.CloseUI<UIMain>();
        }

        public void OnClickPlay()
        {
            SoundManager.Instance.PlaySFX(ESfxType.ButtonClick);
            UIManager.Instance.OpenUI<UIController>();
            UIManager.Instance.CloseUI<UIMain>();
            GameManager.Instance.SpawnEnemy();
        }

        public void OnClickSettings()
        {
            SoundManager.Instance.PlaySFX(ESfxType.ButtonClick);
            UIManager.Instance.OpenUI<UISettingMain>();
            UIManager.Instance.CloseUI<UIMain>();
        }

        #endregion
    }
}