using System;
using System.Collections.Generic;
using UnityEngine;

namespace CardMatch.Score
{
    // Root save data
    [Serializable]
    public class GameSaveData
    {
        public int Version = 1;
        public List<GridBestScores> GridScores = new List<GridBestScores>();

        public GridBestScores GetOrCreateGrid(string gridSize)
        {
            var existing = GridScores.Find(grid => grid.GridSize.Equals(gridSize));

            if (existing == null)
            {
                existing =  new GridBestScores 
                { 
                    GridSize = gridSize 
                };
                GridScores.Add(existing);
            }

            return existing;
        }

        public GridBestScores GetGridBestScores(string gridSize)
        {
            return GridScores.Find(g => g.GridSize.Equals(gridSize));
        }
    }
}
    
