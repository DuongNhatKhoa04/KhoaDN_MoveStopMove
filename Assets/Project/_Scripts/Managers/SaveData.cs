using System;
using System.Collections.Generic;
using System.IO;
using MoveStopMove.DataSaver;
using MoveStopMove.Extensions.Singleton;
using UnityEngine;

namespace MoveStopMove.Manager
{
    public class SaveManager : Singleton<SaveManager>
    {
        public SaveData Data { get; private set; } = new();

        [SerializeField] private string fileName = "userGameplayData.json";
        [SerializeField] private string resourceTemplateName = "userGameplayData";

        public event Action OnSaveChanged;

        private string FilePath => Path.Combine(Application.persistentDataPath, fileName);

        protected override void Awake()
        {
            base.Awake();

            if (!HasSave())
            {
                EnsureDefaultSaveFromResourcesOrNew();
            }
            else
            {
                Load();
                EnsureDefaults(Data);
            }
        }

        #region Save / Load basic

        public bool HasSave() => File.Exists(FilePath);

        public void Load()
        {
            try
            {
                if (!HasSave())
                {
                    Data = new SaveData();
                    return;
                }

                string json = File.ReadAllText(FilePath);
                Data = JsonUtility.FromJson<SaveData>(json) ?? new SaveData();

                EnsureDefaults(Data);
            }
            catch (System.Exception e)
            {
                Debug.LogWarning($"[SaveManager] Load failed -> new SaveData. {e}");
                Data = new SaveData();
            }
        }

        public void Save()
        {
            try
            {
                var dir = Path.GetDirectoryName(FilePath);
                if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

                string json = JsonUtility.ToJson(Data, prettyPrint: true);
                string tmp = FilePath + ".tmp";
                File.WriteAllText(tmp, json);
                if (File.Exists(FilePath)) File.Replace(tmp, FilePath, null);
                else File.Move(tmp, FilePath);
            }
            catch (System.Exception e)
            {
                Debug.LogError($"[SaveManager] Save failed: {e}");
            }
        }

        #endregion

        #region Import / Export utilities

        /// <summary>Import JSON string into Data. If saveFile == true -> Save() to persistent path.</summary>
        public void ImportFromJson(string json, bool saveFile = false)
        {
            if (string.IsNullOrEmpty(json))
            {
                Debug.LogWarning("[SaveManager] ImportFromJson: json is null/empty");
                return;
            }

            try
            {
                var imported = JsonUtility.FromJson<SaveData>(json);
                if (imported != null)
                {
                    Data = imported;
                    EnsureDefaults(Data);
                    Debug.Log("[SaveManager] ImportFromJson: imported successfully.");
                    if (saveFile) Save();
                }
                else
                {
                    Debug.LogWarning("[SaveManager] ImportFromJson: parsed result is null");
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError($"[SaveManager] ImportFromJson failed: {e}");
            }
        }

        /// <summary>Get current save as JSON string (pretty).</summary>
        public string ExportToJson() => JsonUtility.ToJson(Data, prettyPrint: true);

        public void LoadFromFilePath(string path)
        {
            if (!File.Exists(path)) { Debug.LogWarning($"[SaveManager] LoadFromFilePath: not found {path}"); return; }
            try
            {
                var json = File.ReadAllText(path);
                ImportFromJson(json, saveFile: false);
            }
            catch (System.Exception e) { Debug.LogError($"[SaveManager] LoadFromFilePath error: {e}"); }
        }

        public void SaveToFilePath(string path)
        {
            try
            {
                var dir = Path.GetDirectoryName(path);
                if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
                File.WriteAllText(path, ExportToJson());
            }
            catch (System.Exception e) { Debug.LogError($"[SaveManager] SaveToFilePath error: {e}"); }
        }

        public string GetPersistentSavePath() => FilePath;

        #endregion

        #region Helpers: template & defaults

        /// <summary>
        /// If no persistent save exists, try to create one from Resources template.
        /// If Resources template not found, create empty SaveData and Save().
        /// </summary>
        private void EnsureDefaultSaveFromResourcesOrNew()
        {
            // Try Resources first
            var template = Resources.Load<TextAsset>(resourceTemplateName);
            if (template != null && !string.IsNullOrEmpty(template.text))
            {
                Debug.Log("[SaveManager] Found resource template. Importing and saving to persistent path.");
                ImportFromJson(template.text, saveFile: true); // import + Save() to persistent
                return;
            }

            // No resource template -> new default data and save
            Debug.Log("[SaveManager] No resource template. Creating new default SaveData and saving.");
            Data = new SaveData();
            EnsureDefaults(Data);
            Save();
        }

        /// <summary>
        /// Ensure lists/objects are non-null and set sane defaults after load/import.
        /// Call this after Load() or ImportFromJson().
        /// </summary>
        private void EnsureDefaults(SaveData data)
        {
            if (data == null)
            {
                Data = new SaveData();
                return;
            }

            data.unlockedSkin   ??= new List<string>();
            data.lockedSkin     ??= new List<string>();
            data.unlockedPant   ??= new List<string>();
            data.lockedPant     ??= new List<string>();
            data.unlockedWeapon ??= new List<string>();
            data.lockedWeapon   ??= new List<string>();

            if (data.settings == null) data.settings = new PlayerSettings();

            if (string.IsNullOrWhiteSpace(data.equippedPant)) data.equippedPant = null;
            if (string.IsNullOrWhiteSpace(data.equippedSkin)) data.equippedSkin = null;
            if (string.IsNullOrWhiteSpace(data.equippedWeapon)) data.equippedWeapon = null;
        }

        #endregion

        #region OnApp lifecycle

        private void OnApplicationQuit()  => Save();
        private void OnApplicationPause(bool pause)
        {
#if UNITY_ANDROID || UNITY_IOS
            if (pause) Save();
#endif
        }

        #endregion

        public int GetCoins() => Data.coins;

        public bool TrySpendCoins(int amount)
        {
            if (amount <= 0) return true;
            if (Data.coins < amount) return false;
            Data.coins -= amount;
            Save();
            OnSaveChanged?.Invoke();
            return true;
        }

        public void AddCoins(int amount)
        {
            if (amount <= 0) return;
            Data.coins += amount;
            Save();
            OnSaveChanged?.Invoke();
        }

        public bool IsPantUnlocked(string id) => Data.unlockedPant?.Contains(id) == true;

        public void UnlockPant(string id)
        {
            if (string.IsNullOrEmpty(id)) return;
            if (Data.unlockedPant == null) Data.unlockedPant = new List<string>();
            if (!Data.unlockedPant.Contains(id))
            {
                Data.unlockedPant.Add(id);
                Data.lockedPant?.Remove(id);
                Save();
                OnSaveChanged?.Invoke();
            }
        }

        public void EquipPant(string id)
        {
            if (string.IsNullOrEmpty(id)) return;
            if (!IsPantUnlocked(id)) return;
            Data.equippedPant = id;
            Save();
            OnSaveChanged?.Invoke();
        }

        public string GetEquippedPant() => Data.equippedPant;

        public IReadOnlyList<string> GetUnlockedPants() => Data.unlockedPant ?? (IReadOnlyList<string>)Array.Empty<string>();

        public bool IsSkinUnlocked(string id) => Data.unlockedSkin?.Contains(id) == true;
        public void UnlockSkin(string id) { /* giống UnlockPant */ }
        public void EquipSkin(string id) { /* giống EquipPant với check */ }
        public string GetEquippedSkin() => Data.equippedSkin;

        public PlayerSettings GetSettings() => Data.settings ??= new PlayerSettings();
        public void UpdateMasterVolume(float v)
        {
            Data.settings.masterVolume = Mathf.Clamp(v, 0f, 100f);
            Save();
            OnSaveChanged?.Invoke();
        }
    }
}