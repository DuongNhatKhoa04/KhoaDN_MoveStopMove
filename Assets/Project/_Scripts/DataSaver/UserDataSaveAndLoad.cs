using System;
using System.Collections.Generic;

namespace MoveStopMove.DataSaver
{
    [Serializable]
    public class SaveData
    {
        public int version = 1;

        public int currentLevel = 0;
        public int coins = 0;

        public List<string> unlockedSkin = new();
        public List<string> lockedSkin = new();

        public List<string> unlockedPant = new();
        public List<string> lockedPant = new();

        public List<string> unlockedWeapon = new();
        public List<string> lockedWeapon = new();

        public string equippedPant;
        public string equippedSkin;
        public string equippedWeapon;

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