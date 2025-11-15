using System;
using System.IO;
using MoveStopMove.DataPersistence.Data;
using UnityEngine;

namespace MoveStopMove.DataPersistence
{
    public class FileDataHandler
    {
        private string m_dataDirPath;
        private string m_dataFileName;
        private bool m_useEncryption;
        private readonly string m_encryptionCodeWord = "word";

        public FileDataHandler(string dataDirPath, string dataFileName, bool useEncryption)
        {
            this.m_dataDirPath = dataDirPath;
            this.m_dataFileName = dataFileName;
            this.m_useEncryption = useEncryption;
        }

        /// <summary>
        /// Load data form file
        /// </summary>
        /// <returns>Game data</returns>
        public GameData Load()
        {
            string fullPath = Path.Combine(m_dataDirPath, m_dataFileName);
            GameData loadedData = null;
            if (File.Exists(fullPath))
            {
                try
                {
                    string dataToLoad = "";
                    using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            dataToLoad = reader.ReadToEnd();
                        }
                    }

                    if (m_useEncryption)
                    {
                        dataToLoad = EncryptDecrypt(dataToLoad);
                    }

                    loadedData = JsonUtility.FromJson<GameData>(dataToLoad);
                }
                catch (Exception e)
                {
                    Debug.LogError("Error occured when trying to load data from file: " + fullPath + "\n" + e);
                }
            }
            return loadedData;
        }

        /// <summary>
        /// Save data to file
        /// </summary>
        /// <param name="data">Game data</param>
        public void Save(GameData data)
        {
            string fullPath = Path.Combine(m_dataDirPath, m_dataFileName);
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

                string dataToStore = JsonUtility.ToJson(data, true);

                if (m_useEncryption)
                {
                    dataToStore = EncryptDecrypt(dataToStore);
                }

                using (FileStream stream = new FileStream(fullPath, FileMode.Create))
                {
                    using (StreamWriter writer = new StreamWriter(stream))
                    {
                        writer.Write(dataToStore);
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError("Error occured when trying to save data to file: " + fullPath + "\n" + e);
            }
        }

        /// <summary>
        /// A simple implementation of XOR encryption
        /// </summary>
        /// <param name="data">Data input</param>
        /// <returns>Encrypt data</returns>
        private string EncryptDecrypt(string data)
        {
            string modifiedData = "";
            for (int i = 0; i < data.Length; i++)
            {
                modifiedData += (char) (data[i] ^ m_encryptionCodeWord[i % m_encryptionCodeWord.Length]);
            }
            return modifiedData;
        }
    }
}