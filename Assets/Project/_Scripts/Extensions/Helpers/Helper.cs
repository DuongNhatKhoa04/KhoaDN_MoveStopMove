using System;
using System.Collections.Generic;
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
}