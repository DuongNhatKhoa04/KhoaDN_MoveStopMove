using JetBrains.Annotations;
using MoveStopMove.Gameplay.Projectiles;
using UnityEngine;

namespace MoveStopMove.Core.Stats
{
    [CreateAssetMenu(fileName = "CustomData", menuName = "MoveStopMove/CustomData")]
    public class CustomData : ScriptableObject
    {
        [Header("Weapon")]
        [CanBeNull] public GameObject weaponPrefab;
        [CanBeNull] public ProjectileBase projectile;

        [Header("Hair")]
        [CanBeNull] public GameObject hairPrefab;

        [Header("Tail")]
        [CanBeNull] public GameObject tailPrefab;

        [Header("Wing")]
        [CanBeNull] public GameObject wingPrefab;

        [Header("Icon")]
        [CanBeNull] public Sprite icon;

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
        public float movementIncrease = 1f;

        [Header("Shopping")]
        public int price;
    }
}