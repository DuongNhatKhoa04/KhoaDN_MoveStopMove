using System;
using System.Collections.Generic;
using System.IO;
using MoveStopMove.DataSaver;
using MoveStopMove.Extensions.ObjectPooling;
using MoveStopMove.SO;
using MoveStopMove.Weapon;
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

    public static class WeaponBinder
    {
        public const string SO_WEAPON_PATH = "SO/Weapon";
        private static readonly Dictionary<string, WeaponData> cache = new();

        public static void ResetCacheOnPlay()
        {
            cache.Clear();
        }

        public static WeaponData GetWeaponDataById(string weaponId)
        {
            if (string.IsNullOrEmpty(weaponId))
            {
                Debug.LogError("CachedWeaponDataProvider: weaponId rỗng.");
                return null;
            }

            if (cache.TryGetValue(weaponId, out WeaponData cached)) return cached;

            WeaponData loaded = Resources.Load<WeaponData>($"{SO_WEAPON_PATH}/{weaponId}");
            if (loaded == null)
            {
                Debug.LogError($"CachedWeaponDataProvider: Không tìm thấy WeaponData '{weaponId}' trong Resources/{SO_WEAPON_PATH}");
                return null;
            }

            cache[weaponId] = loaded;
            return loaded;
        }
    }

    public static class PlayerSaveLoader
    {
        public static PlayerSaveData LoadFromJsonString(string jsonString)
        {
            if (string.IsNullOrEmpty(jsonString))
            {
                Debug.LogError("PlayerSaveLoader: Chuỗi JSON rỗng.");
                return null;
            }
            return JsonUtility.FromJson<PlayerSaveData>(jsonString);
        }

        public static PlayerSaveData LoadFromPersistentFile(string fileName = "userGameplayData.json")
        {
            string filePath = Path.Combine(Application.persistentDataPath, fileName);
            if (!File.Exists(filePath))
            {
                Debug.LogError($"PlayerSaveLoader: Không tìm thấy file {filePath}");
                return null;
            }
            string jsonString = File.ReadAllText(filePath);
            return LoadFromJsonString(jsonString);
        }

        public static PlayerSaveData LoadFromResourcesText(string resourcesPath)
        {
            TextAsset textAsset = Resources.Load<TextAsset>(resourcesPath);
            if (textAsset == null)
            {
                Debug.LogError($"PlayerSaveLoader: Không tìm thấy TextAsset tại Resources/{resourcesPath}");
                return null;
            }
            return LoadFromJsonString(textAsset.text);
        }
    }
}