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
        [SerializeField] private FixedJoystick fixedJoystick;

        public int EnemyCount { get; set; } = 100;

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

        /// <summary>
        /// Check direction of joystick input
        /// </summary>
        /// <returns>Vector3</returns>
        public Vector3 CheckDirection()
        {
            if (fixedJoystick == null)
            {
                return Vector3.zero;
            }

            return new Vector3(fixedJoystick.Horizontal, 0, fixedJoystick.Vertical);
        }

        /// <summary>
        /// Check joystick input is moving or not
        /// </summary>
        /// <param name="direction">Direction input</param>
        /// <returns>Bool</returns>
        public bool IsMoving(Vector3 direction)
        {
            return direction.sqrMagnitude > 1e-4f;
        }

        public void FindController()
        {
            fixedJoystick = GameObject.Find("FixedJoystick").GetComponent<FixedJoystick>();
        }

        #endregion
    }
}