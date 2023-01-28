using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.SceneManagement;
using System.Collections;

namespace RPG.Saving
{
    public class SavingSystem : MonoBehaviour
    {
        public IEnumerator LoadOnStart(string saveFile)
        {
            Dictionary<string, object> state = LoadFile(saveFile);

            if (state.ContainsKey("lastSceneBuildIndex"))
            {
                int buildIndex = (int)state["lastSceneBuildIndex"];

                if (buildIndex != SceneManager.GetActiveScene().buildIndex) yield return SceneManager.LoadSceneAsync(buildIndex);
            }

            RestoreState(state);
        }

        public void Save(string saveFile)
        {
            Dictionary<string, object> state = LoadFile(saveFile);
            CaptureState(state);
            SaveFile(saveFile, state);
        }

        public void Load(string saveFile)
        {
            RestoreState(LoadFile(saveFile));
        }

        public void Delete(string saveFile)
        {
            File.Delete(GetPathFromSaveFile(saveFile));
        }

        private Dictionary<string, object> LoadFile(string saveFile)
        {
            string savePath = GetPathFromSaveFile(saveFile);

            if (!File.Exists(savePath)) return new Dictionary<string, object>();

            using (FileStream fileStream = File.Open(savePath, FileMode.Open))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                return (Dictionary<string, object>)formatter.Deserialize(fileStream);
            }
        }

        private void RestoreState(Dictionary<string, object> state)
        {
            foreach (SavableEntity entity in FindObjectsOfType<SavableEntity>())
            {
                string id = entity.GetUniqueIdentifier();

                if (state.ContainsKey(id))
                {
                    entity.RestoreState(state[id]);
                }
            }
        }

        private void SaveFile(string saveFile, object state)
        {
            string savePath = GetPathFromSaveFile(saveFile);
            using (FileStream fileStream = File.Open(savePath, FileMode.Create))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(fileStream, state);
            }
        }

        private void CaptureState(Dictionary<string, object> state)
        {
            foreach (SavableEntity entity in FindObjectsOfType<SavableEntity>())
            {
                state[entity.GetUniqueIdentifier()] = entity.CaptureState();
            }

            state["lastSceneBuildIndex"] = SceneManager.GetActiveScene().buildIndex;
        }

        public string GetPathFromSaveFile(string savefile)
        {
            return Path.Combine(Application.persistentDataPath, savefile + ".sav");
        }
    }
}