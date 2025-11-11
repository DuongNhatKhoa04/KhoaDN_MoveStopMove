using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace MoveStopMove
{
    [CreateAssetMenu(fileName = "SkinData", menuName = "MoveStopMove/SkinData")]
    public class SkinData : ScriptableObject
    {
        [Header("Prefab")]
        public GameObject prefab;

        [Header("Icon")]
        [CanBeNull] public Texture2D icon;

        [Header("Pant")]
        [CanBeNull] public Texture2D pant;

        [Header("Skin")]
        [CanBeNull] public Texture2D skin;

        [Header("Buff")]
        public float rangeIncrease = 0.2f;
        public float attackSpeedIncrease = 0f;
        public float movementIncrease = 1f;

        [Header("Shopping")]
        public int price;
    }
}
