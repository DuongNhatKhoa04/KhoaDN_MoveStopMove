using System;
using System.Collections.Generic;

namespace MoveStopMove.DataPersistence.Data
{
    [Serializable]
    public class GameData
    {
        public long lastUpdated;
        public int coins;
        public int kills;

        public List<string> unlockedSkin = new();
        public List<string> lockedSkin = new();

        public List<string> unlockedPant = new();
        public List<string> lockedPant = new();

        public List<string> unlockedHair = new();
        public List<string> lockedHair = new();

        public List<string> unlockedWeapon = new();
        public List<string> lockedWeapon = new();

        public string equippedSkin;
        public string equippedPant;
        public string equippedHair;
        public string equippedWeapon;

        public float masterVolume = 100f;
        public float sfxVolume = 100f;
        public float musicVolume = 100f;

        public static GameData CreateDefault()
        {
            var startingData = new GameData();

            startingData.coins = 0;
            startingData.kills = 0;

            startingData.unlockedSkin.AddRange(new[] { "none" });
            startingData.unlockedPant.AddRange(new[] { "chambi" });
            startingData.unlockedHair.AddRange(new[] { "arrow" });
            startingData.unlockedWeapon.AddRange(new[] { "z" });

            startingData.lockedSkin.AddRange(new[]
                { "devil", "angel", "deadpool", "thor" });
            startingData.lockedPant.AddRange(new[]
                { "batman", "comy", "dabao", "onion", "pokemon", "rainbow", "skull", "vantim" });
            startingData.lockedHair.AddRange(new[]
                { "cowboy", "ear", "hat", "cap", "hat_yellow", "headphone", "horn" });
            startingData.lockedWeapon.AddRange(new[]
                { "arrow", "axe_0", "axe_1", "boomerang", "candy_0", "candy_1", "harmer", "knife", "uzi" });


            startingData.equippedSkin   = "none";
            startingData.equippedPant   = "chambi";
            startingData.equippedHair   = "arrow";
            startingData.equippedWeapon = "z";

            startingData.masterVolume = 100f;
            startingData.sfxVolume    = 100f;
            startingData.musicVolume  = 100f;

            return startingData;
        }
    }
}