using System;
using UnityEngine;

namespace MoveStopMove.Utility.Audio
{
    public enum ESfxType
    {
        ButtonClick,
        BuySuccess,
        NotEnoughCoins,
        EquipSuccess,
        ProjectileHit,
        PlayerDie,
        PlayerDance,
        PlayerRun,
        PlayerAttack,
        Notification,
        Win,
        Lose
    }

    [Serializable]
    public class SfxEntry
    {
        public ESfxType type;
        public AudioClip clip;
    }
}