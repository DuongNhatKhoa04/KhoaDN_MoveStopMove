using MoveStopMove.Core.Units;
using MoveStopMove.Utility.Extension;
using UnityEngine;
using UnityEngine.Pool;

namespace MoveStopMove.Utility
{
    public class ObjectPoolingManager : Singleton<ObjectPoolingManager>
    {
        #region -- Fields --

        [SerializeField] private Character enemy;

        private IObjectPool<Character> m_objectPool;

        #endregion

        #region -- Methods --

        private void Awake()
        {
            base.Awake();

            m_objectPool = new ObjectPool<Character>
            (
                CreateEnemy,
                OnGetromPool,
                OnReleaseToPool,
                OnDestroyPooledObject,
                true,
                20,
                100
            );
        }

        private Character CreateEnemy()
        {
            if (enemy == null)
            {
                Debug.LogError("Enemy prefab chưa được gán trong ObjectPoolingManager!");
                return null;
            }
            var enemyInstance = Instantiate(enemy);
            enemyInstance.ObjectPool = m_objectPool;
            return enemyInstance;
        }

        private void OnGetromPool(Character pooledObject)
        {
            pooledObject.gameObject.SetActive(true);
            //pooledObject.Initialize();
        }

        private void OnReleaseToPool(Character pooledObject)
        {
            pooledObject.gameObject.SetActive(false);
        }

        private void OnDestroyPooledObject(Character pooledObject)
        {
            Destroy(pooledObject.gameObject);
        }

        public Character GetEnemy()
        {
            return m_objectPool.Get();
        }

        public Character SpawnEnemy(Vector3 position)
        {
            var enemyInstance = m_objectPool.Get();

            enemyInstance.transform.position = position;

            enemyInstance.Initialize();

            return enemyInstance;
        }

        #endregion
    }
}