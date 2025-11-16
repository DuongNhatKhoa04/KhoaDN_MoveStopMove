using System;
using System.Collections.Generic;
using MoveStopMove.Core;
using MoveStopMove.DataPersistence.Data;
using MoveStopMove.Extensions.Decorator;
using MoveStopMove.Interfaces;
using MoveStopMove.MainCharacter;
using MoveStopMove.SO;
using UnityEngine;

namespace MoveStopMove.Extensions.Helpers
{
    #region -- Animation --

    public enum EAnim
    {
        Idle,
        Run,
        Attack,
        Dead,
        Dance,
        Ulti,
        Win
    }

    public static class AnimHashes
    {
        private static readonly int s_idle   = Animator.StringToHash("Idle");
        private static readonly int s_run    = Animator.StringToHash("Run");
        private static readonly int s_attack = Animator.StringToHash("Attack");
        private static readonly int s_dead   = Animator.StringToHash("Dead");
        private static readonly int s_dance  = Animator.StringToHash("Dance");
        private static readonly int s_ulti   = Animator.StringToHash("Ulti");
        private static readonly int s_win    = Animator.StringToHash("Win");

        public static readonly Dictionary<EAnim, int> Map = new()
        {
            { EAnim.Idle, s_idle },
            { EAnim.Run, s_run },
            { EAnim.Attack, s_attack },
            { EAnim.Dead, s_dead },
            { EAnim.Dance, s_dance },
            { EAnim.Ulti, s_ulti },
            { EAnim.Win, s_win },
        };
    }

    public static class EnumCacheToString<T> where T : Enum
    {
        private static readonly Dictionary<T, string> s_cache = new();

        public static string GetString(T value)
        {
            if (!s_cache.TryGetValue(value, out string result))
            {
                result = Enum.GetName(typeof(T), value);
                s_cache[value] = result;
            }
            return result;
        }
    }

    #endregion

    public static class GenericNotImplementedError<T>
    {
        public static T TryGet(T value, string name)
        {
            if(value != null)
            {
                return value;
            }

            Debug.LogError(typeof(T) + " not implemented on " + name);
            return default;
        }
    }

    public static class PlayerSaveLoader
    {
        public static readonly string SO_WEAPON_PATH = "SO/Weapons";
        public static readonly string SO_PANTS_PATH = "SO/Pants";
        public static readonly string SO_HAIRS_PATH = "SO/Hairs";
        public static readonly string SO_CUSTOMS_PATH = "SO/Customs";

        private static readonly int s_mainTex = Shader.PropertyToID("_MainTex");

        /// <summary>
        /// Load and save TData to TResult
        /// </summary>
        /// <param name="itemName">Item need to load</param>
        /// <param name="path">Path of item - SO/{item}</param>
        /// <param name="selector">Transfer func TData to TResult</param>
        /// <typeparam name="TData">ScriptableObject</typeparam>
        /// <typeparam name="TResult">Data type you want to get</typeparam>
        /// <returns>If data found, return data type input. If not, return data type with default value</returns>
        public static TResult GetDecoratorData<TData, TResult>(string itemName, string path,
                                                                Func<TData, TResult> selector)
            where TData : ScriptableObject
        {
            TData dataSo = Resources.Load<TData>($"{path}/{itemName}");

            if (dataSo != null)
            {
                return selector(dataSo);
            }

            Debug.LogWarning($"[GetDecoratorData] Không tìm thấy {typeof(TData).Name} tại {path}/{itemName}");
            return default;
        }

        public static void CheckSkinToApply(bool hasSkinTexture)
        {
            if (hasSkinTexture)
            {

            }
        }

        public static void SetAlbedoForMaterial(SkinnedMeshRenderer skinMesh,Texture2D texture)
        {
            if (!skinMesh)
            {
                Debug.LogWarning("SetAlbedoForMaterial: SkinnedMeshRenderer bị null!");
                return;
            }

            if (!texture)
            {
                Debug.LogWarning($"SetAlbedoForMaterial: Texture null trên {skinMesh.name}");
                return;
            }

            var materialArray = skinMesh.materials;
            if (materialArray.Length == 0)
            {
                Debug.LogWarning($"SetAlbedoForMaterial: {skinMesh.name} không có materials!");
                return;
            }

            var material = materialArray[0];

            material.SetTexture(s_mainTex, texture);
            skinMesh.materials = materialArray;
        }

        public static void SetNewMaterialForSkin(SkinnedMeshRenderer skinMesh, Material skinMaterial)
        {
            /*if (isEquippedSkin)
            {
                var materialArray = skinMesh.materials;
                materialArray[0] = SkinMaterial;
                skinMesh.materials = materialArray;
            }
            else
            {
                var materialArray = skinMesh.materials;
                materialArray[0] = DefaultSkinMaterial;
                skinMesh.materials = materialArray;
            }*/
            var materialArray = skinMesh.materials;
            materialArray[0] = skinMaterial;
            skinMesh.materials = materialArray;
        }
    }

    #region -- Decorator Builder --

    public static class CharacterDecoratorBuilder
    {
        public static IDecoratable Build(
            GameData gameData,
            CustomVisualContext customContext,
            RendererReferences references,
            AttachmentReferences attachment,
            IVisualProvider provider)
        {
            var nullDeco = new NullDecoratable();

            var skin = new SkinDecorator(nullDeco)
            {
                SkinSetRenderer     = references.SkinRenderer,
                DefaultSkinMaterial = provider.DefaultSkinMaterial,
                SkinMaterial        = customContext.skinMaterial,
                SkinTexture         = customContext.skinTexture,
                HasTexture          = customContext.hasTextureInSkin
            };

            var pant = new PantDecorator(skin)
            {
                PantsRenderer = references.PantsRenderer,
                PantTexture   = customContext.pantTexture ??
                                provider.GetPantTextureFromData(gameData.equippedPant)
            };

            var tail = new TailDecorator(pant)
            {
                TailAttachment = attachment.TailAttachment,
                TailPrefab     = provider.GetTailPrefabFromData()
            };

            var wing = new WingDecorator(tail)
            {
                WingAttachment = attachment.WingAttachment,
                WingPrefab     = provider.GetWingPrefabFromData()
            };

            var hair = new HairDecorator(wing)
            {
                HairAttachment = attachment.HairAttachment,
                HairPrefab     = customContext.hairPrefab ??
                                 provider.GetHairPrefabFromData(gameData.equippedHair)
            };

            var weapon = new WeaponDecorator(hair)
            {
                WeaponAttachment = attachment.WeaponAttachment,
                WeaponPrefab     = customContext.weaponPrefab ??
                                   provider.GetWeaponPrefabFromData(gameData.equippedWeapon)
            };

            return weapon;
        }
    }

    public class AttachmentReferences
    {
        public GameObject WeaponAttachment;
        public GameObject HairAttachment;
        public GameObject WingAttachment;
        public GameObject TailAttachment;
    }

    public class RendererReferences
    {
        public SkinnedMeshRenderer PantsRenderer;
        public SkinnedMeshRenderer SkinRenderer;
    }

    #endregion
}