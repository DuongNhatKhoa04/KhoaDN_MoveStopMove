using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Serialization;

namespace MoveStopMove
{
    [CreateAssetMenu(fileName = "CustomData", menuName = "MoveStopMove/CustomData")]
    public class CustomData : ScriptableObject
    {
        [Header("Weapon")]
        [CanBeNull] public GameObject weaponPrefab;

        [Header("Hair")]
        [CanBeNull] public GameObject hairPrefab;

        [Header("Tail")]
        [CanBeNull] public GameObject tailPrefab;

        [Header("Wing")]
        [CanBeNull] public GameObject wingPrefab;

        [Header("Icon")]
        [CanBeNull] public Texture2D icon;

        [Header("Pant")]
        [CanBeNull] public Texture2D pant;

        [Header("Skin Texture")]
        [CanBeNull] public Texture2D skinTexture;

        [Header("Skin Material")]
        [CanBeNull] public Material skinMaterial;

        [Header("Skin Features")]
        public bool hasWeapon;
        public bool hasHair;
        public bool hasWing;
        public bool hasPant;
        public bool hasTail;
        public bool hasSkinTexture;

        [Header("Buff")]
        public float rangeIncrease = 0.2f;
        public float attackSpeedIncrease = 0f;
        public float movementIncrease = 1f;

        [Header("Shopping")]
        public int price;
    }
}
