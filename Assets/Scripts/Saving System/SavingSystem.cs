using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;

namespace RPG.Saving
{
    public class SavingSystem : MonoBehaviour
    {
        public void Save(string saveFile)
        {
            string savePath = GetPathFromSaveFile(saveFile);
            using (FileStream fileStream = File.Open(savePath, FileMode.Create))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(fileStream, CaptureState());
            }
        }

        public void Load(string saveFile)
        {
            string savePath = GetPathFromSaveFile(saveFile);
            using (FileStream fileStream = File.Open(savePath, FileMode.Open))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                RestoreState(formatter.Deserialize(fileStream));
            }
        }

        private void RestoreState(object state)
        {
            Dictionary<string, object> stateDictionary = (Dictionary<string, object>)state;
            foreach (SavableEntity entity in FindObjectsOfType<SavableEntity>())
            {
                entity.RestoreState(stateDictionary[entity.GetUniqueIdentifier()]);
            }
        }

        private object CaptureState()
        {
            Dictionary<string, object> stateDictionary = new Dictionary<string, object>();
            foreach (SavableEntity entity in FindObjectsOfType<SavableEntity>())
            {
                stateDictionary[entity.GetUniqueIdentifier()] = entity.CaptureState();
            }
            return stateDictionary;
        }

        public string GetPathFromSaveFile(string savefile)
        {
            return Path.Combine(Application.persistentDataPath, savefile + ".sav");
        }
    }
}