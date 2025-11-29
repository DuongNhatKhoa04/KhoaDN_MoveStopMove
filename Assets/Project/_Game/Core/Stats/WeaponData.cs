using MoveStopMove.Gameplay.Projectiles;
using UnityEngine;

namespace MoveStopMove.Core.Stats
{
    public enum EWeaponAttackType
    {
        Normal,
        Piercing,
        Chainable,
        Returnable
    }

    public enum ESpecialSkill
    {
        SpecializedUnyielding,
        SpecializedEvolve,
        Revenge,
        DmgReflect,
        None
    }

    [CreateAssetMenu(fileName = "WeaponData", menuName = "MoveStopMove/WeaponData")]
    public class WeaponData : ScriptableObject
    {
        [Header("Type")]
        public EWeaponAttackType weaponType = EWeaponAttackType.Normal;

        [Header("Icon")]
        public Sprite icon;

        [Header("Prefab")]
        public GameObject prefab;

        [Header("Projectile")]
        public ProjectileBase projectilePrefab;

        [Header("Buff")]
        public float rangeIncrease = 0.2f;
        public float maxAttackRange = 8f;

        [Header("Special Skill")]
        public ESpecialSkill specialSkill = ESpecialSkill.None;

        [Header("Shopping")]
        public int price;
    }
}