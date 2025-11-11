using System;
using System.Collections.Generic;
using System.IO;
using MoveStopMove.DataSaver;
using MoveStopMove.Extensions.Singleton;
using UnityEngine;

namespace MoveStopMove.Manager
{
    public class SaveDataManager : Singleton<SaveDataManager>
    {
        [SerializeField] private string persistentFileName = "userGameplayData.json";
        [SerializeField] private string resourcesTemplateName = "userGameplayData";

        public PlayerSaveData Data { get; private set; } = new PlayerSaveData();
        public event Action OnSaveChanged;

        private string PersistentFilePath => Path.Combine(Application.persistentDataPath, persistentFileName);

        protected override void Awake()
        {
            base.Awake();

            if (!HasPersistentFile())
            {
                CreateFromTemplateOrNew();
            }
            else
            {
                Load();
                EnsureDefaults(Data);
            }
        }

        public bool HasPersistentFile() => File.Exists(PersistentFilePath);

        /// <summary>Đọc từ file JSON tại persistentDataPath → Data.</summary>
        public void Load()
        {
            try
            {
                if (!HasPersistentFile())
                {
                    Debug.Log("[SaveDataManager] No persistent file. Creating new data.");
                    Data = new PlayerSaveData();
                    return;
                }

                Debug.Log("[SaveDataManager] Loading: " + PersistentFilePath);
                string json = File.ReadAllText(PersistentFilePath);
                Data = JsonUtility.FromJson<PlayerSaveData>(json) ?? new PlayerSaveData();
                EnsureDefaults(Data);
            }
            catch (Exception exception)
            {
                Debug.LogWarning($"[SaveDataManager] Load failed, use new data. {exception}");
                Data = new PlayerSaveData();
            }
        }

        /// <summary>Ghi Data → file JSON tại persistentDataPath (safe save với file tạm).</summary>
        public void Save()
        {
            try
            {
                string directory = Path.GetDirectoryName(PersistentFilePath);
                if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);

                string json = JsonUtility.ToJson(Data, prettyPrint: true);
                string tempPath = PersistentFilePath + ".tmp";
                File.WriteAllText(tempPath, json);

                if (File.Exists(PersistentFilePath))
                {
                    try { File.Replace(tempPath, PersistentFilePath, null); }
                    catch
                    {
                        File.Copy(tempPath, PersistentFilePath, overwrite: true);
                        File.Delete(tempPath);
                    }
                }
                else
                {
                    File.Move(tempPath, PersistentFilePath);
                }

                Debug.Log("[SaveDataManager] Saved: " + PersistentFilePath);
            }
            catch (Exception exception)
            {
                Debug.LogError($"[SaveDataManager] Save failed: {exception}");
            }
        }

        /// <summary>Import JSON string → Data (có thể kèm Save).</summary>
        public void ImportFromJson(string json, bool writeToDisk = false)
        {
            if (string.IsNullOrEmpty(json))
            {
                Debug.LogWarning("[SaveDataManager] ImportFromJson: json is null/empty");
                return;
            }

            try
            {
                PlayerSaveData imported = JsonUtility.FromJson<PlayerSaveData>(json);
                if (imported == null)
                {
                    Debug.LogWarning("[SaveDataManager] ImportFromJson: parsed result is null");
                    return;
                }

                Data = imported;
                EnsureDefaults(Data);
                if (writeToDisk) Save();
                OnSaveChanged?.Invoke();
            }
            catch (Exception exception)
            {
                Debug.LogError($"[SaveDataManager] ImportFromJson failed: {exception}");
            }
        }

        /// <summary>Xuất Data hiện tại thành JSON (pretty).</summary>
        public string ExportToJson() => JsonUtility.ToJson(Data, prettyPrint: true);

        /// <summary>Xoá file persistent hiện tại.</summary>
        public void DeletePersistentFile()
        {
            if (File.Exists(PersistentFilePath))
            {
                File.Delete(PersistentFilePath);
                Debug.Log("[SaveDataManager] Deleted: " + PersistentFilePath);
            }
            else
            {
                Debug.Log("[SaveDataManager] No file to delete at: " + PersistentFilePath);
            }
        }

        /// <summary>Reset: Xoá persistent & tạo lại từ template Resources (nếu có) hoặc tạo data mới rồi Save.</summary>
        public void ResetToTemplate()
        {
            DeletePersistentFile();
            CreateFromTemplateOrNew();
            OnSaveChanged?.Invoke();
        }

        /// <summary>Patch Data bằng code (hàm truyền vào), sau đó Save + bắn sự kiện.</summary>
        public void PatchAndSave(Action<PlayerSaveData> patchAction)
        {
            if (Data == null) Data = new PlayerSaveData();
            patchAction?.Invoke(Data);
            Save();
            OnSaveChanged?.Invoke();
        }

        public string GetPersistentPathForDebug() => PersistentFilePath;

        // ====== Internals ======

        private void CreateFromTemplateOrNew()
        {
            // Thử lấy TextAsset trong Resources
            TextAsset template = Resources.Load<TextAsset>(resourcesTemplateName);
            if (template != null && !string.IsNullOrEmpty(template.text))
            {
                Debug.Log("[SaveDataManager] Create from Resources template and save.");
                ImportFromJson(template.text, writeToDisk: true);
                return;
            }

            // Không có template -> tạo mới & save
            Debug.Log("[SaveDataManager] No template. Create new data and save.");
            Data = new PlayerSaveData();
            EnsureDefaults(Data);
            Save();
        }

        /// <summary>Đảm bảo các trường không null và có giá trị mặc định hợp lý.</summary>
        private void EnsureDefaults(PlayerSaveData data)
        {
            if (data == null)
            {
                Data = new PlayerSaveData();
                return;
            }

            // Sửa theo kiểu dữ liệu bạn định nghĩa trong PlayerSaveData (khuyến nghị List<string>)
            data.unlockedSkin   ??= new List<string>();
            data.lockedSkin     ??= new List<string>();
            data.unlockedPant   ??= new List<string>();
            data.lockedPant     ??= new List<string>();
            data.unlockedWeapon ??= new List<string>();
            data.lockedWeapon   ??= new List<string>();
            data.unlockedHair   ??= new List<string>();
            data.lockedHair     ??= new List<string>();

            data.settings ??= new PlayerSettings();

            if (string.IsNullOrWhiteSpace(data.equippedSkin))   data.equippedSkin   = null;
            if (string.IsNullOrWhiteSpace(data.equippedPant))   data.equippedPant   = null;
            if (string.IsNullOrWhiteSpace(data.equippedWeapon)) data.equippedWeapon = null;
            if (string.IsNullOrWhiteSpace(data.equippedHair))   data.equippedHair   = null;
        }

        private void OnApplicationQuit() => Save();

        private void OnApplicationPause(bool pause)
        {
#if UNITY_ANDROID || UNITY_IOS
            if (pause) Save();
#endif
        }
    }
}