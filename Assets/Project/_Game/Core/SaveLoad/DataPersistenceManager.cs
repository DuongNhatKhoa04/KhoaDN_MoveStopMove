using System.Collections.Generic;
using System.Linq;
using MoveStopMove.Core.Interfaces;
using MoveStopMove.Core.SaveLoad.Data;
using MoveStopMove.Utility.Extension;
using UnityEngine;

namespace MoveStopMove.Core.SaveLoad
{
    public class DataPersistenceManager : Singleton<DataPersistenceManager>
    {
        #region -- Fields --

        [Header("File Storage Config")]
        [SerializeField] private string fileName;
        [SerializeField] private bool useEncryption;

        private List<IDataPersistence> m_dataPersistenceObjects;
        private FileDataHandler m_dataHandler;
        private GameData m_gameData;

        #endregion

        #region -- Properties --

        public GameData GameData
        {
            get => m_gameData;
            set => m_gameData = value;
        }

        public bool IsLoaded { get; private set; }

        #endregion

        #region -- Methods --

        private void Awake()
        {
            base.Awake();
            this.m_dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, useEncryption);
        }

        private void Start()
        {
            this.m_dataPersistenceObjects = FindAllDataPersistenceObjects();
            LoadGame();
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
            IsLoaded = false;

            m_gameData = m_dataHandler.Load();

            if (m_gameData == null)
            {
                Debug.Log("No data was found. Initializing data to defaults.");
                NewGame();
            }

            foreach (IDataPersistence dataPersistenceObj in m_dataPersistenceObjects)
            {
                dataPersistenceObj.LoadData(m_gameData);
            }

            IsLoaded = true;
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

        /// <summary>
        /// Find all the IDataPersistence implementations
        /// </summary>
        /// <returns>List of class implemented IDataPersistence</returns>
        private List<IDataPersistence> FindAllDataPersistenceObjects()
        {
            IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>()
                .OfType<IDataPersistence>();

            return new List<IDataPersistence>(dataPersistenceObjects);
        }

        #endregion
    }
}