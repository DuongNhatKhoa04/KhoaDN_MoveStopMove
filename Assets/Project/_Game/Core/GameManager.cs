using MoveStopMove.Utility;
using MoveStopMove.Utility.Extension;
using MoveStopMove.Core.Units;
using MoveStopMove.Presentation.UI.Shops.Weapon;
using UnityEngine;

namespace MoveStopMove.Core
{
    public class GameManager : Singleton<GameManager>
    {
        #region -- Fields --

        [SerializeField] private Character enemyPrefab;
        [SerializeField] private UIWeaponCard weaponCardPrefab;

        #endregion

        #region -- Methods --

        private void Start()
        {
            ObjectPoolingManager.Instance.CreateObjectPool(enemyPrefab, "EnemyPool");
            ObjectPoolingManager.Instance.CreateObjectPool(weaponCardPrefab, "WeaponCardPool");
        }

        /*private void SpawnEnemy(Vector3 position)
        {
            var enemy = ObjectPoolingManager.Instance.GetObjectFromPool<Character>("EnemyPool");
            enemy.transform.position = position;
            enemy.gameObject.SetActive(true);
        }

        private void SpawnProjectile(Vector3 position, Vector3 direction)
        {
            var projectile = ObjectPoolingManager.Instance.GetObjectFromPool<Projectile>("ProjectilePool");
            projectile.transform.position = position;
            projectile.SetDirection(direction);
            projectile.gameObject.SetActive(true);
        }*/

        #endregion
    }
}