using UnityEngine;

namespace MoveStopMove.SO
{
    [CreateAssetMenu(fileName = "CharacterData", menuName = "MoveStopMove/CharacterData")]
    public class CharacterData : ScriptableObject
    {
        public float speed;
        public float acceleration;

        public float attackSpeed;
        public float attackRangeRadius;
    }
}