using UnityEngine;

namespace MoveStopMove.Core.Stats
{
    [CreateAssetMenu(fileName = "PantData", menuName = "MoveStopMove/PantData")]
    public class PantData : ScriptableObject
    {
        [Header("Texture")]
        public Texture2D texture;

        [Header("Buff")]
        public float movementIncrease = 1f;

        [Header("Shopping")]
        public int price;
    }
}