using UnityEngine;


namespace CardMatch.Score
{
    public class ScoreManager
    {
        private GameSaveData saveData;
        private IScoreStorage storage;

        public ScoreManager()
        {
            storage = new JsonScoreStorage();
            LoadData();
        }

        #region Public API 
        
        // Save best time for a grid size 
        public bool SaveBestTime(string gridSize, double completionTime)
        {
            var gridScores = saveData.GetOrCreateGrid(gridSize);

            // Only save if it's a new best
            if (completionTime < gridScores.bestTime)
            {
                gridScores.bestTime = completionTime;
                SaveData();
                Logger.Log($"New best time for {gridSize}: {completionTime:F2}s");
                return true;
            }
            return false;
        }
        
        // Get best time for a grid size       
        public float GetBestTime(string gridSize)
        {
            var gridScores = saveData.GetGridBestScores(gridSize);

            if (gridScores != null && gridScores.HasBestTime)
            {
                return (float)gridScores.bestTime;
            }

            return 0f;
        }
        
        // Check if this time is a new record        
        public bool IsNewBestTime(string gridSize, float completionTime)
        {
            float currentBest = GetBestTime(gridSize);
            return currentBest == 0f || completionTime < currentBest;
        }

        /* Backup save when the application is paused or closed.
           Currently not strictly required because we only store the best time.
            However, this will become important once we persist full game state data.*/
        public void OnApplicationPause(bool isPaused)
        {
            if (isPaused)
            {
                SaveData(); 
                Logger.Log("Backup save on pause");
            }            
        }
        public void OnApplicationQuit()
        {
            SaveData(); // Final backup save
            Debug.Log("Backup save on quit");
        }

        #endregion

        #region Storage Operations

        private void LoadData()
        {
            saveData = storage.Load();
        }

        private void SaveData()
        {
            storage.Save(saveData);
        }
        
        // Clear all scores (for testing or reset)        
        public void ClearAllScores()
        {
            saveData = new GameSaveData();
            SaveData();
            Logger.Log("All scores cleared");
        }

        public void DeleteSaveFile()
        {
            storage.Delete();
            saveData = new GameSaveData();
            Logger.Log("Save file deleted");
        }


        // Get save file path         
        public string GetSaveFilePath()
        {
            return storage.FilePath;
        }

        #endregion
    }
}