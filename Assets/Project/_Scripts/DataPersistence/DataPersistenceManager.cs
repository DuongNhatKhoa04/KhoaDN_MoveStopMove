using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MoveStopMove.DataPersistence.Data;
using MoveStopMove.Extensions.Singleton;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MoveStopMove.DataPersistence
{
    public class DataPersistenceManager : Singleton<DataPersistenceManager>
    {
        [Header("File Storage Config")]
        [SerializeField] private string fileName;
        [SerializeField] private bool useEncryption;

        private GameData m_gameData;
        private List<IDataPersistence> m_dataPersistenceObjects;
        private FileDataHandler m_dataHandler;

        public GameData PlayerGameData
        {
            get => m_gameData;
            set => m_gameData = value;
        }

        private void Awake()
        {
            this.m_dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, useEncryption);
            this.m_dataPersistenceObjects = FindAllDataPersistenceObjects();
            LoadGame();
            // Debug.Log(PlayerGameData.equippedPant);
        }

        private void Start()
        {
            /*this.m_dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, useEncryption);
            this.m_dataPersistenceObjects = FindAllDataPersistenceObjects();
            LoadGame();
            Debug.Log(PlayerGameData.equippedPant);*/ //JsonUtility.ToJson(m_gameData, true));
        }

        /// <summary>
        /// Create new game data
        /// </summary>
        public void NewGame()
        {
            this.m_gameData = GameData.CreateDefault();
        }

        /// <summary>
        /// Load game data
        /// </summary>
        public void LoadGame()
        {
            this.m_gameData = m_dataHandler.Load();

            if (this.m_gameData == null)
            {
                Debug.Log("No data was found. Initializing data to defaults.");
                NewGame();
            }

            foreach (IDataPersistence dataPersistenceObj in m_dataPersistenceObjects)
            {
                dataPersistenceObj.LoadData(m_gameData);
            }
        }

        /// <summary>
        /// Save game data
        /// </summary>
        public void SaveGame()
        {
            foreach (IDataPersistence dataPersistenceObj in m_dataPersistenceObjects)
            {
                dataPersistenceObj.SaveData(m_gameData);
            }

            m_dataHandler.Save(m_gameData);
        }

        /// <summary>
        /// Save data when quit application
        /// </summary>
        private void OnApplicationQuit()
        {
            SaveGame();
        }

        private List<IDataPersistence> FindAllDataPersistenceObjects()
        {
            IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>()
                .OfType<IDataPersistence>();

            return new List<IDataPersistence>(dataPersistenceObjects);
        }
    }
}