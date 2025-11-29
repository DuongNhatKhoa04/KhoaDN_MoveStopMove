using MoveStopMove.Core.Units;
using MoveStopMove.Presentation.UI;
using MoveStopMove.Presentation.UI.Main;
using MoveStopMove.Presentation.UI.Shops.Outfit;
using MoveStopMove.Presentation.UI.Shops.Weapon;
using MoveStopMove.Utility;
using MoveStopMove.Utility.Extension;
using UnityEngine;

namespace MoveStopMove.Core
{
    public class GameManager : Singleton<GameManager>
    {
        #region -- Fields --

        [SerializeField] private Character enemyPrefab;
        [SerializeField] private UIWeaponCard weaponCardPrefab;
        [SerializeField] private UIPantCard pantCardPrefab;
        [SerializeField] private UIHairCard hairCardPrefab;
        [SerializeField] private UICustomCard customCardPrefab;

        #endregion

        #region -- Methods --

        private void Awake()
        {
            ObjectPoolingManager.Instance.CreateObjectPool(enemyPrefab, "EnemyPool");
            ObjectPoolingManager.Instance.CreateObjectPool(weaponCardPrefab, "WeaponCardPool");
            ObjectPoolingManager.Instance.CreateObjectPool(pantCardPrefab,  "PantCardPool");
            ObjectPoolingManager.Instance.CreateObjectPool(hairCardPrefab, "HairCardPool");
            ObjectPoolingManager.Instance.CreateObjectPool(customCardPrefab, "CustomCardPool");
        }

        private void Start()
        {
            UIManager.Instance.OpenUI<UIMain>();
        }

        #endregion
    }
}