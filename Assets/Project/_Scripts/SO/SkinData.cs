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

        [Header("Skin Texture")]
        [CanBeNull] public Texture2D skin;

        [Header("Skin Material")]
        [CanBeNull] public Material material;

        [Header("Skin Features")]
        public bool hasHair;
        public bool hasPant;
        public bool hasTail;
        public bool hasWing;
        public bool hasSkin;

        [Header("Buff")]
        public float rangeIncrease = 0.2f;
        public float attackSpeedIncrease = 0f;
        public float movementIncrease = 1f;

        [Header("Shopping")]
        public int price;
    }
}
