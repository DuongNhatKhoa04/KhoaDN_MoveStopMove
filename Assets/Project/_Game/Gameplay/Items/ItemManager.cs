using System.Collections.Generic;
using System.Linq;
using MoveStopMove.Core.Events;
using MoveStopMove.Core.Interfaces;
using MoveStopMove.Core.SaveLoad.Data;
using MoveStopMove.Core.Stats;
using MoveStopMove.Utility.Extension;
using UnityEngine;

namespace MoveStopMove.Gameplay.Items
{
    public class ItemManager : Singleton<ItemManager>, IDataPersistence
    {
        #region -- Fields --

        private readonly List<WeaponData> m_unlockedWeapons = new();
        private readonly List<WeaponData> m_lockedWeapons = new();

        private readonly List<PantData> m_unlockedPants = new();
        private readonly List<PantData> m_lockedPants = new();

        private readonly List<HairData> m_unlockedHairs = new();
        private readonly List<HairData> m_lockedHairs = new();

        private readonly List<CustomData> m_unlockedCustoms = new();
        private readonly List<CustomData> m_lockedCustoms = new();

        private WeaponData m_equippedWeapon;
        private PantData m_equippedPant;
        private HairData m_equippedHair;
        private CustomData m_equippedCustom;

        private GameData m_gameData;
        private int m_coins;

        #endregion

        #region -- Properties --

        public bool IsDataLoaded { get; private set; }

        public IReadOnlyList<WeaponData> UnlockedWeapons => m_unlockedWeapons;
        public IReadOnlyList<WeaponData> LockedWeapons => m_lockedWeapons;
        public WeaponData EquippedWeapon => m_equippedWeapon;

        public IReadOnlyList<PantData> UnlockedPants => m_unlockedPants;
        public IReadOnlyList<PantData> LockedPants => m_lockedPants;
        public PantData EquippedPant => m_equippedPant;

        public IReadOnlyList<HairData> UnlockedHairs => m_unlockedHairs;
        public IReadOnlyList<HairData> LockedHairs => m_lockedHairs;
        public HairData EquippedHair => m_equippedHair;

        public IReadOnlyList<CustomData> UnlockedCustoms => m_unlockedCustoms;
        public IReadOnlyList<CustomData> LockedCustoms => m_lockedCustoms;
        public CustomData EquippedCustom => m_equippedCustom;

        public int Coins => m_coins;

        #endregion

        #region -- Methods --

        private void CheckAndAddItem<T>(List<string> lockedItemsInFile, List<string> unlockedItemsInFile,
            List<T> lockItemsToList, List<T> unlockItemsToList,
            string path) where T : ScriptableObject
        {
            if (unlockedItemsInFile != null && unlockedItemsInFile.Count > 0)
                AddItems(unlockedItemsInFile, unlockItemsToList, path);

            if (lockedItemsInFile != null && lockedItemsInFile.Count > 0)
                AddItems(lockedItemsInFile, lockItemsToList, path);
        }

        private void AddItems<T>(List<string> itemsInFile, List<T> itemsToList, string path)
            where T : ScriptableObject
        {
            foreach (var itemName in itemsInFile)
            {
                T item = PlayerSaveLoader.GetDecoratorData<T, T>(
                    itemName,
                    path,
                    data => data
                );

                if (item != null)
                    itemsToList.Add(item);
            }
        }

        private static TData LoadEquipped<TData>(string equippedName, string path)
            where TData : ScriptableObject
        {
            if (string.IsNullOrEmpty(equippedName))
                return null;

            return PlayerSaveLoader.GetDecoratorData<TData, TData>(
                equippedName,
                path,
                data => data
            );
        }

        public void LoadData(GameData gameData)
        {
            //Debug.Log("Loading Weapons");
            m_gameData = gameData;
            m_coins = gameData.coins;

            m_unlockedWeapons.Clear();
            m_lockedWeapons.Clear();
            m_unlockedPants.Clear();
            m_lockedPants.Clear();
            m_unlockedHairs.Clear();
            m_lockedHairs.Clear();
            m_unlockedCustoms.Clear();
            m_lockedCustoms.Clear();

            CheckAndAddItem(
                gameData.lockedWeapon, gameData.unlockedWeapon,
                m_lockedWeapons, m_unlockedWeapons,
                PlayerSaveLoader.SO_WEAPON_PATH
            );

            CheckAndAddItem(
                gameData.lockedPant, gameData.unlockedPant,
                m_lockedPants, m_unlockedPants,
                PlayerSaveLoader.SO_PANTS_PATH
            );

            CheckAndAddItem(
                gameData.lockedHair, gameData.unlockedHair,
                m_lockedHairs, m_unlockedHairs,
                PlayerSaveLoader.SO_HAIRS_PATH
            );

            CheckAndAddItem(
                gameData.lockedCustom, gameData.unlockedCustom,
                m_lockedCustoms, m_unlockedCustoms,
                PlayerSaveLoader.SO_CUSTOMS_PATH
            );

            m_equippedWeapon = LoadEquipped<WeaponData>(
                gameData.equippedWeapon,
                PlayerSaveLoader.SO_WEAPON_PATH
            );

            m_equippedPant = LoadEquipped<PantData>(
                gameData.equippedPant,
                PlayerSaveLoader.SO_PANTS_PATH
            );

            m_equippedHair = LoadEquipped<HairData>(
                gameData.equippedHair,
                PlayerSaveLoader.SO_HAIRS_PATH
            );

            m_equippedCustom = LoadEquipped<CustomData>(
                gameData.equippedCustom,
                PlayerSaveLoader.SO_CUSTOMS_PATH
            );

            IsDataLoaded = true;
            //Debug.Log("Done Loading Weapons");
        }

        public void SaveData(GameData data)
        {
            data.coins = m_coins;

            data.lockedWeapon = m_lockedWeapons.Select(weapon => weapon.name).ToList();
            data.unlockedWeapon = m_unlockedWeapons.Select(w => w.name).ToList();
            data.equippedWeapon = m_equippedWeapon != null ? m_equippedWeapon.name : "";

            data.lockedPant = m_lockedPants.Select(p => p.name).ToList();
            data.unlockedPant = m_unlockedPants.Select(p => p.name).ToList();
            data.equippedPant = m_equippedPant != null ? m_equippedPant.name : "";

            data.lockedHair = m_lockedHairs.Select(h => h.name).ToList();
            data.unlockedHair = m_unlockedHairs.Select(h => h.name).ToList();
            data.equippedHair = m_equippedHair != null ? m_equippedHair.name : "";

            data.lockedCustom = m_lockedCustoms.Select(c => c.name).ToList();
            data.unlockedCustom = m_unlockedCustoms.Select(c => c.name).ToList();
            data.equippedCustom = m_equippedCustom != null ? m_equippedCustom.name : "";
        }

        public bool TryBuyWeapon(WeaponData weapon)
        {
            if (weapon == null) return false;

            if (m_unlockedWeapons.Contains(weapon))
                return false;

            if (!m_lockedWeapons.Contains(weapon))
                return false;

            int price = weapon.price;

            if (m_coins < price)
            {
                return false;
            }

            m_coins -= price;
            m_lockedWeapons.Remove(weapon);
            m_unlockedWeapons.Add(weapon);

            return true;
        }

        public bool TryEquipWeapon(WeaponData weapon)
        {
            if (weapon == null) return false;

            if (!m_unlockedWeapons.Contains(weapon))
                return false;

            m_equippedWeapon = weapon;

            return true;
        }

        #region -- Buy / Equip Pant --

        public bool TryBuyPant(PantData pant)
        {
            if (pant == null) return false;

            if (m_unlockedPants.Contains(pant))
                return false;

            if (!m_lockedPants.Contains(pant))
                return false;

            int price = (int)pant.price;

            if (m_coins < price)
            {
                EventManager.Instance.Notify(
                    new NotificationPopUpEvent(EEventCode.NotEnoughCoins)
                );
                return false;
            }

            m_coins -= price;
            m_lockedPants.Remove(pant);
            m_unlockedPants.Add(pant);

            EventManager.Instance.Notify(
                new NotificationPopUpEvent(EEventCode.BuySuccess)
            );

            return true;
        }

        public bool TryEquipPant(PantData pant)
        {
            if (pant == null) return false;
            if (!m_unlockedPants.Contains(pant))
                return false;

            m_equippedPant = pant;

            EventManager.Instance.Notify(
                new NotificationPopUpEvent(EEventCode.EquipSuccess)
            );

            return true;
        }

        #endregion

        #region -- Buy / Equip Hair --

        public bool TryBuyHair(HairData hair)
        {
            if (hair == null) return false;

            if (m_unlockedHairs.Contains(hair))
                return false;

            if (!m_lockedHairs.Contains(hair))
                return false;

            int price = (int)hair.price;

            if (m_coins < price)
            {
                EventManager.Instance.Notify(
                    new NotificationPopUpEvent(EEventCode.NotEnoughCoins)
                );
                return false;
            }

            m_coins -= price;
            m_lockedHairs.Remove(hair);
            m_unlockedHairs.Add(hair);

            EventManager.Instance.Notify(
                new NotificationPopUpEvent(EEventCode.BuySuccess)
            );

            return true;
        }

        public bool TryEquipHair(HairData hair)
        {
            if (hair == null) return false;
            if (!m_unlockedHairs.Contains(hair))
                return false;

            m_equippedHair = hair;

            EventManager.Instance.Notify(
                new NotificationPopUpEvent(EEventCode.EquipSuccess)
            );

            return true;
        }

        #endregion

        #region -- Buy / Equip Custom --

        public bool TryBuyCustom(CustomData custom)
        {
            if (custom == null) return false;

            if (m_unlockedCustoms.Contains(custom))
                return false;

            if (!m_lockedCustoms.Contains(custom))
                return false;

            int price = (int)custom.price;

            if (m_coins < price)
            {
                EventManager.Instance.Notify(
                    new NotificationPopUpEvent(EEventCode.NotEnoughCoins)
                );
                return false;
            }

            m_coins -= price;
            m_lockedCustoms.Remove(custom);
            m_unlockedCustoms.Add(custom);

            EventManager.Instance.Notify(
                new NotificationPopUpEvent(EEventCode.BuySuccess)
            );

            return true;
        }

        public bool TryEquipCustom(CustomData custom)
        {
            if (custom == null) return false;
            if (!m_unlockedCustoms.Contains(custom))
                return false;

            m_equippedCustom = custom;

            EventManager.Instance.Notify(
                new NotificationPopUpEvent(EEventCode.EquipSuccess)
            );

            return true;
        }

        #endregion

        #endregion
    }
}
