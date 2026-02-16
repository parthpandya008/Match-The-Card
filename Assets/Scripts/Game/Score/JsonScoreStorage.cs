using System.IO;
using UnityEngine;

namespace CardMatch.Score
{
    // Stores scores as JSON
    public class JsonScoreStorage : IScoreStorage
    {
        private readonly string saveFilePath;
        private readonly string SaveFileName = "CardMatch_Scores.json";

        public string FilePath => saveFilePath;

        public JsonScoreStorage()
        {
            saveFilePath = Path.Combine(Application.persistentDataPath, SaveFileName);
            Logger.Log($"JsonScoreStorage SaveFilePath: {saveFilePath}");
        }

        public void Save(GameSaveData data)
        {
            try
            {
                string json = JsonUtility.ToJson(data, prettyPrint: true);
                File.WriteAllText(saveFilePath, json);
                Logger.Log($"Saved scores to {json}");                
            }
            catch (System.Exception e)
            {
                Logger.LogError($"Failed to save scores: {e.Message}");
            }
        }

        public bool HasSaveFile()
        {
            return File.Exists(saveFilePath);
        }

        public GameSaveData Load()
        {
            if(!HasSaveFile())
            {
                Logger.Log("No save file found, returning new GameSaveData");
                return new GameSaveData();
            }
            try
            {
                string json = File.ReadAllText(saveFilePath);
                GameSaveData data = JsonUtility.FromJson<GameSaveData>(json);
                Logger.Log($"Scores loaded from: {saveFilePath}");
                return data;
            }
            catch (System.Exception e)
            {
                Logger.LogError($"Failed to load scores: {e.Message}");
                return new GameSaveData();
            }
        }

        public void Delete()
        {
            if(HasSaveFile())
            {
                File.Delete(saveFilePath);
            }
            Logger.Log("Score save file deleted!");
        }                      
    }
}
