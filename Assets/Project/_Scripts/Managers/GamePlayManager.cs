using System.Collections.Generic;
using MoveStopMove.Extensions.Singleton;
using MoveStopMove.Project._Scripts.Managers;
using UnityEngine;

namespace MoveStopMove.Managers
{
    public class GamePlayManager : Singleton<GamePlayManager>
    {
        #region -- Fields --

        [Header("Enemy Spawn Settings")]
        [SerializeField] private int enemyCount = 20;
        [SerializeField] private float spawnRadius = 100f;
        [SerializeField] private float minDistanceBetweenEnemies = 2f;
        [SerializeField] private Transform spawnCenter;

        private readonly List<Vector3> m_spawnedPositions = new();

        #endregion

        #region -- Methods --

        private void Start()
        {
            if (spawnCenter == null)
                spawnCenter = transform;

            SpawnEnemies(enemyCount);
        }

        private void SpawnEnemies(int count)
        {
            for (int i = 0; i < count; i++)
            {
                if (!TryGetValidSpawnPosition(out Vector3 pos))
                {
                    Debug.LogWarning("Không tìm được vị trí spawn hợp lệ, dừng sớm.");
                    break;
                }

                Debug.Log($"Spawn enemy {i} tại {pos}");
                ObjectPoolingManager.Instance.SpawnEnemy(pos);

                // lưu lại để lần sau còn check khoảng cách
                m_spawnedPositions.Add(pos);
            }
        }

        private bool TryGetValidSpawnPosition(out Vector3 result)
        {
            const int maxAttempts = 50;
            int attempts = 0;

            while (attempts < maxAttempts)
            {
                attempts++;

                Vector2 randomCircle = Random.insideUnitCircle * spawnRadius;
                Vector3 candidate = new Vector3(
                    spawnCenter.position.x + randomCircle.x,
                    spawnCenter.position.y,
                    spawnCenter.position.z + randomCircle.y
                );

                bool tooClose = false;
                foreach (var pos in m_spawnedPositions)
                {
                    if ((pos - candidate).sqrMagnitude < minDistanceBetweenEnemies * minDistanceBetweenEnemies)
                    {
                        tooClose = true;
                        break;
                    }
                }

                if (!tooClose)
                {
                    result = candidate;
                    return true;
                }
            }

            result = spawnCenter.position;
            return false;
        }

        #endregion
    }
}