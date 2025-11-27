using System.Collections.Generic;
using MoveStopMove.Core.SaveLoad;
using MoveStopMove.Core.SaveLoad.Data;
using MoveStopMove.Core.Stats;
using MoveStopMove.Utility.Extension;
using UnityEngine;

namespace MoveStopMove.Gameplay.Items
{
    public class ItemManager : Singleton<ItemManager>
    {
        #region -- Fields --

        private List<WeaponData> m_unlockedWeapons = new();
        private List<WeaponData> m_lockedWeapons = new();

        private List<PantData> m_unlockedPants = new();
        private List<PantData> m_lockedPants = new();

        private List<HairData> m_unlockedHairs = new();
        private List<HairData> m_lockedHairs = new();

        private List<CustomData> m_unlockedCustoms = new();
        private List<CustomData> m_lockedCustoms = new();

        private GameData m_gameData;

        #endregion

        #region -- Properties --

        public bool IsDataLoaded { get; private set; }

        #endregion

        #region -- Methods --

        private void Awake()
        {
            base.Awake();
        }

        private void Start()
        {
            m_gameData = DataPersistenceManager.Instance.GameData;

            var lockedWeapon = m_gameData.lockedWeapon;
            var lockedPant = m_gameData.lockedPant;
            var lockedHair = m_gameData.lockedHair;
            var lockedCustom = m_gameData.lockedCustom;

            var unlockedWeapon = m_gameData.unlockedWeapon;
            var unlockedPant = m_gameData.unlockedPant;
            var unlockedHair = m_gameData.unlockedHair;
            var unlockedCustom = m_gameData.unlockedCustom;

            if (m_gameData != null)
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
                IsDataLoaded = true;
            }
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

        #endregion
    }
}