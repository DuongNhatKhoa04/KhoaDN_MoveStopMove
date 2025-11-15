using System;
using System.Collections.Generic;
using UnityEngine.Serialization;

namespace MoveStopMove.DataSaver
{
    [Serializable]
    public class PlayerSaveData
    {
        public int version = 1;

        public int currentLevel = 0;
        public int coins = 0;

        public List<string> unlockedCustom = new();
        public List<string> lockedCustom = new();

        public List<string> unlockedPant = new();
        public List<string> lockedPant = new();

        public List<string> unlockedWeapon = new();
        public List<string> lockedWeapon = new();

        public List<string> unlockedHair = new();
        public List<string> lockedHair = new();

        public string equippedPant;
        public string equippedCustom;
        public string equippedWeapon;
        public string equippedHair;

        public PlayerSettings settings = new();
    }

    [Serializable]
    public class PlayerSettings
    {
        public float masterVolume = 100f;
        public float sfxVolume = 100f;
        public float musicVolume = 100f;
    }
}