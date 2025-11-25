using System.Collections.Generic;
using MoveStopMove.Core.Stats;
using MoveStopMove.Gameplay.SaveLoad;
using MoveStopMove.Gameplay.SaveLoad.Data;
using MoveStopMove.Utility.Extension;
using UnityEngine;

namespace MoveStopMove.Gameplay.Items
{
    public class ItemManager : Singleton<ItemManager>
    {
        private List<WeaponData> m_unlockedWeapons = new();
        private List<WeaponData> m_lockedWeapons = new();

        private List<PantData> m_unlockedPants = new();
        private List<PantData> m_lockedPants = new();

        private List<HairData> m_unlockedHairs = new();
        private List<HairData> m_lockedHairs = new();

        private List<CustomData> m_unlockedCustoms = new();
        private List<CustomData> m_lockedCustoms = new();

        private GameData m_data;

        private void Start()
        {
            m_data = DataPersistenceManager.Instance.PlayerGameData;

            var lockedWeapon = m_data.lockedWeapon;
            var lockedPant = m_data.lockedPant;
            var lockedHair = m_data.lockedHair;
            var lockedCustom = m_data.lockedCustom;

            var unlockedWeapon = m_data.unlockedWeapon;
            var unlockedPant = m_data.unlockedPant;
            var unlockedHair = m_data.unlockedHair;
            var unlockedCustom = m_data.unlockedCustom;

            if (m_data != null)
            {
                CheckAndAddItem(
                    lockedWeapon, unlockedWeapon,
                    m_lockedWeapons, m_unlockedWeapons,
                    PlayerSaveLoader.SO_WEAPON_PATH);

                CheckAndAddItem(
                    lockedPant, unlockedPant,
                    m_lockedPants, m_unlockedPants,
                    PlayerSaveLoader.SO_PANTS_PATH);

                CheckAndAddItem(
                    lockedHair, unlockedHair,
                    m_lockedHairs, m_unlockedHairs,
                    PlayerSaveLoader.SO_HAIRS_PATH);

                CheckAndAddItem(
                    lockedCustom, unlockedCustom,
                    m_lockedCustoms, m_unlockedCustoms,
                    PlayerSaveLoader.SO_CUSTOMS_PATH);
            }
        }

        private void CheckAndAddItem<T>(List<string> lockedItemsInFile, List<string> unlockedItemsInFile,
            List<T> lockItemsToList, List<T> unlockItemsToList, string path) where T : ScriptableObject
        {
            if (unlockedItemsInFile != null && unlockedItemsInFile.Count > 0)
            {
                AddItems<T>(unlockedItemsInFile, unlockItemsToList, path);
            }

            if (lockedItemsInFile != null && lockedItemsInFile.Count > 0)
            {
                AddItems<T>(lockedItemsInFile, lockItemsToList, path);
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