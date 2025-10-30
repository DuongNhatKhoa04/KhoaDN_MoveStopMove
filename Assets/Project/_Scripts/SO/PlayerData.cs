using UnityEngine;

namespace MoveStopMove.SO
{
    [CreateAssetMenu(fileName = "PlayerData", menuName = "MoveStopMove/PlayerData")]
    public class PlayerData : ScriptableObject
    {
        [Header("Movement")]
        public float speed;
        public float acceleration;

        [Header("Attack References")]
        public float attackSpeed;
        public float attackRangeRadius;
    }
}