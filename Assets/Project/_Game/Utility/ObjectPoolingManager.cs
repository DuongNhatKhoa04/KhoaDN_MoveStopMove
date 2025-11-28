using System.Collections.Generic;
using MoveStopMove.Core.Units;
using MoveStopMove.Utility.Extension;
using UnityEngine;
using UnityEngine.Pool;

namespace MoveStopMove.Utility
{
    public class ObjectPoolingManager : Singleton<ObjectPoolingManager>
    {
        #region -- Fields --

        private Dictionary<string, object> m_objectPools = new();

        #endregion

        #region -- Methods --

        public void CreateObjectPool<T>(T prefab, string poolKey, int initialSize = 20, int maxSize = 100)
            where T : MonoBehaviour
        {
            var pool = new ObjectPool<T>(
                () => Instantiate(prefab),
                entity => entity.gameObject.SetActive(true),
                entity => entity.gameObject.SetActive(false),
                entity => Destroy(entity.gameObject),
                true,
                initialSize,
                maxSize
            );

            m_objectPools[poolKey] = pool;
        }

        public T GetObjectFromPool<T>(string poolKey) where T : MonoBehaviour
        {
            if (m_objectPools.ContainsKey(poolKey))
            {
                var pool = (ObjectPool<T>)m_objectPools[poolKey];
                return pool.Get();
            }
            else
            {
                Debug.LogError($"Pool with key {poolKey} not found.");
                return null;
            }
        }

        public void ReleaseObjectToPool<T>(T obj, string poolKey) where T : MonoBehaviour
        {
            if (m_objectPools.ContainsKey(poolKey))
            {
                var pool = (ObjectPool<T>)m_objectPools[poolKey];
                pool.Release(obj);
            }
            else
            {
                Debug.LogError($"Pool with key {poolKey} not found.");
            }
        }

        #endregion
    }
}