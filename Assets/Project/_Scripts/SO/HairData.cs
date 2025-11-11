using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoveStopMove
{
    [CreateAssetMenu(fileName = "HairData", menuName = "MoveStopMove/HairData")]
    public class HairData : ScriptableObject
    {
        [Header("Prefab")]
        public GameObject prefab;

        [Header("Buff")]
        public float rangeIncrease = 0.2f;

        [Header("Shopping")]
        public int price;
    }
}
