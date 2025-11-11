using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace MoveStopMove.SO
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

        [Header("Prefab")]
        public GameObject prefab;

        [Header("Buff")]
        public float rangeIncrease = 0.2f;
        public float attackSpeedIncrease = 0f;
        public float movementIncrease = 1f;
        public float maxAttackRange = 8f;

        [Header("Special Skill")]
        public ESpecialSkill specialSkill = ESpecialSkill.None;

        [Header("Shopping")]
        public int price;
    }
}