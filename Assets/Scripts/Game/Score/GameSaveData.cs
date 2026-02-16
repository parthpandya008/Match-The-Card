using System;
using System.Collections.Generic;
using UnityEngine;

namespace CardMatch.Score
{
    // Root save data
    [Serializable]
    public class GameSaveData
    {
        public int version = 1;
        public List<GridBestScores> gridScores = new List<GridBestScores>();

        public GridBestScores GetOrCreateGrid(string gridSize)
        {
            var existing = gridScores.Find(grid => grid.gridSize.Equals(gridSize));

            if (existing == null)
            {
                existing =  new GridBestScores 
                { 
                    gridSize = gridSize 
                };
                gridScores.Add(existing);
            }

            return existing;
        }

        public GridBestScores GetGridBestScores(string gridSize)
        {
            return gridScores.Find(g => g.gridSize.Equals(gridSize));
        }
    }
}
    
