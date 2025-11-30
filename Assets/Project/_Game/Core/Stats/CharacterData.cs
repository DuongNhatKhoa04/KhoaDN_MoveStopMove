using UnityEngine;

namespace MoveStopMove.Core.Stats
{
    [CreateAssetMenu(fileName = "CharacterData", menuName = "MoveStopMove/CharacterData")]
    public class CharacterData : ScriptableObject
    {
        public float speed = 5;
        public float acceleration = 60;

        public float attackRangeRadius = 4;
    }
}