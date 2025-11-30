using System;
using System.Collections.Generic;

namespace MoveStopMove.Core.SaveLoad.Data
{
    [Serializable]
    public class GameData
    {
        #region -- Fields --

        #region - Player -

        public int coins;
        //public int kills;

        public string equippedCustom;
        public string equippedPant;
        public string equippedHair;
        public string equippedWeapon;

        #endregion

        #region - Shop -

        public List<string> unlockedCustom = new();
        public List<string> unlockedPant = new();
        public List<string> unlockedHair = new();
        public List<string> unlockedWeapon = new();

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

        #endregion

        #region -- Methods --

        /// <summary>
        /// Create default data when first time start the game
        /// </summary>
        /// <returns>GameData</returns>
        public static GameData CreateDefault()
        {
            var startingData = new GameData();

            startingData.coins = 0;
            //startingData.kills = 0;

            startingData.unlockedCustom.Clear();
            //startingData.unlockedCustom.AddRange(new[] { "devil" });
            //startingData.unlockedCustom.AddRange(new[] { "angel" });
            //startingData.unlockedCustom.AddRange(new[] { "thor" });
            startingData.unlockedPant.AddRange(new[] { "chambi" });
            startingData.unlockedHair.AddRange(new[] { "arrow" });
            startingData.unlockedWeapon.AddRange(new[] { "z", "boomerang" });

            startingData.lockedCustom.AddRange(new[]
                { "devil", "angel", "thor" });
            /*startingData.lockedCustom.AddRange(new[]
                { "none", "angel", "thor" });*/
            /*startingData.lockedCustom.AddRange(new[]
                { "none", "devil", "thor" });*/
            /*startingData.lockedCustom.AddRange(new[]
                { "none", "angel", "devil" });*/
            startingData.lockedPant.AddRange(new[]
                { "batman", "comy", "dabao", "onion", "pokemon", "rainbow", "skull", "vantim" });
            startingData.lockedHair.AddRange(new[]
                { "cowboy", "ear", "hat", "cap", "hat_yellow", "headphone" });
            startingData.lockedWeapon.AddRange(new[]
                { "arrow", "axe_0", "axe_1", "candy_0", "candy_1", "harmer", "knife", "uzi" });


            startingData.equippedCustom = "";
            //startingData.equippedCustom   = "devil";
            //startingData.equippedCustom   = "angel";
            //startingData.equippedCustom   = "thor";
            startingData.equippedPant   = "chambi";
            startingData.equippedHair   = "arrow";
            startingData.equippedWeapon = "boomerang";

            startingData.masterVolume = 100f;
            startingData.sfxVolume    = 100f;
            startingData.musicVolume  = 100f;

            return startingData;
        }

        #endregion
    }
}