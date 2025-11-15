using System;
using System.Collections.Generic;
using UnityEngine.Serialization;

namespace MoveStopMove.DataPersistence.Data
{
    [Serializable]
    public class GameData
    {
        #region - Player -

        public int coins;
        public int kills;

        public string equippedCustom;
        public string equippedPant;
        public string equippedHair;
        public string equippedWeapon;

        #endregion

        #region - Inventory -

        public List<string> unlockedCustom = new();
        public List<string> unlockedPant = new();
        public List<string> unlockedHair = new();
        public List<string> unlockedWeapon = new();

        #endregion

        #region - Shop -

        public List<string> lockedCustom = new();
        public List<string> lockedPant = new();
        public List<string> lockedHair = new();
        public List<string> lockedWeapon = new();

        #endregion

        #region - System -

        public float masterVolume = 100f;
        public float sfxVolume = 100f;
        public float musicVolume = 100f;

        #endregion

        public static GameData CreateDefault()
        {
            var startingData = new GameData();

            startingData.coins = 0;
            startingData.kills = 0;

            startingData.unlockedCustom.AddRange(new[] { "none" });
            //startingData.unlockedCustom.AddRange(new[] { "devil" });
            //startingData.unlockedCustom.AddRange(new[] { "angel" });
            //startingData.unlockedCustom.AddRange(new[] { "thor" });
            startingData.unlockedPant.AddRange(new[] { "chambi" });
            startingData.unlockedHair.AddRange(new[] { "arrow" });
            startingData.unlockedWeapon.AddRange(new[] { "z" });

            startingData.lockedCustom.AddRange(new[]
                { "devil", "angel", "thor" });
            /*startingData.lockedCustom.AddRange(new[]
                { "none", "angel", "thor" });*/
            /*startingData.lockedCustom.AddRange(new[]
                { "none", "devil", "thor" });*/
            /*startingData.lockedCustom.AddRange(new[]
                { "none", "angel", "devil" });*/
            startingData.lockedPant.AddRange(new[]
                { "batman", "comy", "dabao", "onion", "pokemon", "rainbow", "skull", "vantim", "none" });
            startingData.lockedHair.AddRange(new[]
                { "cowboy", "ear", "hat", "cap", "hat_yellow", "headphone", "horn", "none" });
            startingData.lockedWeapon.AddRange(new[]
                { "arrow", "axe_0", "axe_1", "boomerang", "candy_0", "candy_1", "harmer", "knife", "uzi" });


            startingData.equippedCustom = "none";
            //startingData.equippedCustom   = "devil";
            //startingData.equippedCustom   = "angel";
            //startingData.equippedCustom   = "thor";
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