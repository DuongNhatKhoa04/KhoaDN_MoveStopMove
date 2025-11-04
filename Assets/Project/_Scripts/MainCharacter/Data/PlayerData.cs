using UnityEngine;

namespace MoveStopMove.MainCharacter.Data
{
    [CreateAssetMenu(fileName = "PlayerData", menuName = "MoveStopMove/PlayerData")]
    public class PlayerData : ScriptableObject
    {
        public float speed;
        public float acceleration;

        public float attackSpeed;
        public float attackRangeRadius;
    }
}