using System.Collections.Generic;
using System.Linq;
using MoveStopMove.Core.Interfaces;
using MoveStopMove.Core.SaveLoad.Data;
using MoveStopMove.Core.Stats;
using MoveStopMove.Utility.Extension;
using UnityEngine;

namespace MoveStopMove.Core.SaveLoad
{
    public class DataPersistenceManager : Singleton<DataPersistenceManager>
    {
        #region -- Fields --

        [Header("File Storage Config")] [SerializeField]
        private string fileName;

        [SerializeField] private bool useEncryption;
        [SerializeField] private CharacterData characterData;

        private List<IDataPersistence> m_dataPersistenceObjects;
        private FileDataHandler m_dataHandler;
        private GameData m_gameData;

        private float m_maxAttackRange;
        private float m_maxRangeIncrease;
        private float m_maxMovement;

        #endregion

        #region -- Properties --

        public GameData GameData => m_gameData;

        public CharacterData CharacterData => characterData;

        public float MaxAttackRange => m_maxAttackRange;

        public float MaxRangeIncrease => m_maxRangeIncrease;

        public float MaxMovement => m_maxMovement;

        public bool IsLoaded { get; private set; }

        #endregion

        #region -- Methods --

        private void Awake()
        {
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

        public void UpdateMaxRange()
        {
            var newRange = PlayerSaveLoader.GetDecoratorData<WeaponData, float>(
                m_gameData.equippedWeapon,
                PlayerSaveLoader.SO_WEAPON_PATH,
                data => data.maxAttackRange);

            m_maxAttackRange = Mathf.Max(characterData.attackRangeRadius, newRange);
        }

        public void UpdateRangeIncreasement()
        {
            var custom = m_gameData.equippedCustom;
            var hair = m_gameData.equippedHair;
            var weapon = m_gameData.equippedWeapon;
            float rangeIncrease = 0;

            if (string.IsNullOrEmpty(custom))
            {
                rangeIncrease += 0;
            }
            else
            {
                var rangeFromCustom = PlayerSaveLoader.GetDecoratorData<CustomData, float>(
                    custom,
                    PlayerSaveLoader.SO_CUSTOMS_PATH,
                    data => data.rangeIncrease);

                rangeIncrease += rangeFromCustom;
            }

            var rangeFromHair = PlayerSaveLoader.GetDecoratorData<HairData, float>(
                hair,
                PlayerSaveLoader.SO_HAIRS_PATH,
                data => data.rangeIncrease);

            rangeIncrease += rangeFromHair;

            var rangeFromWeapon = PlayerSaveLoader.GetDecoratorData<WeaponData, float>(
                weapon,
                PlayerSaveLoader.SO_WEAPON_PATH,
                data => data.rangeIncrease);

            rangeIncrease += rangeFromWeapon;
            Debug.Log("range up: " + rangeIncrease);

            m_maxRangeIncrease = Mathf.Max(0.1f, rangeIncrease);
        }

        public void UpdateMaxMovement()
        {
            var pant = m_gameData.equippedPant;
            float movementIncrease = 0;

            var rangeFromHair = PlayerSaveLoader.GetDecoratorData<PantData, float>(
                pant,
                PlayerSaveLoader.SO_PANTS_PATH,
                data => data.movementIncrease);

            movementIncrease += rangeFromHair;

            m_maxMovement = characterData.speed + movementIncrease;
        }

        #endregion
    }
}