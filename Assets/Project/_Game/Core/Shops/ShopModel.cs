using System.Collections.Generic;
using MoveStopMove.Core.Interfaces;
using MoveStopMove.Core.SaveLoad;
using MoveStopMove.Core.SaveLoad.Data;
using MoveStopMove.Core.Stats;
using MoveStopMove.Utility.Extension;
using UnityEngine;

namespace MoveStopMove.Core.Shops
{
    public class ShopModel : IDataPersistence
    {
        private List<WeaponData> m_unlockedWeapons = new();
        private List<WeaponData> m_lockedWeapons = new();

        private List<PantData> m_unlockedPants = new();
        private List<PantData> m_lockedPants = new();

        private List<HairData> m_unlockedHairs = new();
        private List<HairData> m_lockedHairs = new();

        private List<CustomData> m_unlockedCustoms = new();
        private List<CustomData> m_lockedCustoms = new();

        public int Coins { get; private set; }
        public bool IsDataLoaded { get; private set; }

        public WeaponData EquippedWeapon { get; private set; }

        public PantData EquippedPant { get; private set; }

        public HairData EquippedHair { get; private set; }

        public CustomData EquippedCustom { get; private set; }

        public void LoadData(GameData gameData)
        {
            Coins = gameData.coins;

            CheckAndAddItem(
                gameData.lockedWeapon, gameData.unlockedWeapon,
                m_lockedWeapons, m_unlockedWeapons,
                PlayerSaveLoader.SO_WEAPON_PATH);

            CheckAndAddItem(
                gameData.lockedPant, gameData.unlockedPant,
                m_lockedPants, m_unlockedPants,
                PlayerSaveLoader.SO_PANTS_PATH);

            CheckAndAddItem(
                gameData.lockedHair, gameData.unlockedHair,
                m_lockedHairs, m_unlockedHairs,
                PlayerSaveLoader.SO_HAIRS_PATH);

            CheckAndAddItem(
                gameData.lockedCustom, gameData.unlockedCustom,
                m_lockedCustoms, m_unlockedCustoms,
                PlayerSaveLoader.SO_CUSTOMS_PATH);

            EquippedWeapon = PlayerSaveLoader.GetDecoratorData<WeaponData, WeaponData>(
                gameData.equippedWeapon,
                PlayerSaveLoader.SO_WEAPON_PATH,
                data => data);

            EquippedPant = PlayerSaveLoader.GetDecoratorData<PantData, PantData>(
                gameData.equippedPant,
                PlayerSaveLoader.SO_PANTS_PATH,
                data => data);

            EquippedHair = PlayerSaveLoader.GetDecoratorData<HairData, HairData>(
                gameData.equippedHair,
                PlayerSaveLoader.SO_HAIRS_PATH,
                data => data);

            EquippedCustom = PlayerSaveLoader.GetDecoratorData<CustomData, CustomData>(
                gameData.equippedCustom,
                PlayerSaveLoader.SO_CUSTOMS_PATH,
                data => data);

            IsDataLoaded = true;
        }

        public void SaveData(GameData data)
        {
            throw new System.NotImplementedException();
        }

        private void CheckAndAddItem<T>(List<string> lockedItemsInFile, List<string> unlockedItemsInFile,
            List<T> lockItemsToList, List<T> unlockItemsToList, string path) where T : ScriptableObject
        {
            if (unlockedItemsInFile != null && unlockedItemsInFile.Count > 0)
            {
                AddItems<T>(unlockedItemsInFile, unlockItemsToList, path);
                //Debug.Log(unlockItemsToList[0]);
            }

            if (lockedItemsInFile != null && lockedItemsInFile.Count > 0)
            {
                AddItems<T>(lockedItemsInFile, lockItemsToList, path);
                //Debug.Log(lockItemsToList[0]);
            }
        }

        private void AddItems<T>(List<string> itemsInFile, List<T> itemsToList, string path)
            where T : ScriptableObject
        {
            foreach (var lockedItem in itemsInFile)
            {
                T item = PlayerSaveLoader.GetDecoratorData<T, T>(lockedItem, path, data => data);
                itemsToList.Add(item);
            }
        }

        public List<WeaponData> GetLockedWeapons()
        {
            return m_lockedWeapons;
        }

        public List<WeaponData> GetUnlockedWeapons()
        {
            return m_unlockedWeapons;
        }

        public List<PantData> GetLockedPants()
        {
            return m_lockedPants;
        }

        public List<PantData> GetUnlockedPants()
        {
            return m_unlockedPants;
        }

        public List<HairData> GetLockedHairs()
        {
            return m_lockedHairs;
        }

        public List<HairData> GetUnlockedHairs()
        {
            return m_unlockedHairs;
        }

        public List<CustomData> GetLockedCustoms()
        {
            return m_lockedCustoms;
        }

        public List<CustomData> GetUnlockedCustoms()
        {
            return m_unlockedCustoms;
        }
    }
}