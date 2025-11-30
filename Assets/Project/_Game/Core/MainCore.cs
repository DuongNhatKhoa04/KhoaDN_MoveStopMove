using JetBrains.Annotations;
using MoveStopMove.Core.Combat;
using MoveStopMove.Core.Movement;
using MoveStopMove.Core.Stats;
using UnityEngine;

namespace MoveStopMove.Core
{
    public class MainCore : MonoBehaviour
    {
        #region -- Fields --

        [CanBeNull] [SerializeField] private PlayerMovement movement;
        [CanBeNull] [SerializeField] private EnemyMovement enemyMovement;
        [SerializeField] private Battle battle;

        #endregion

        #region -- Properties --

        public PlayerMovement Movement => movement;
        public EnemyMovement EnemyMovement => enemyMovement;
        public Battle Battle => battle;

        #endregion
    }
}