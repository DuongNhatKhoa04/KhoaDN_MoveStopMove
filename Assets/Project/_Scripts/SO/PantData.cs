using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoveStopMove
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
