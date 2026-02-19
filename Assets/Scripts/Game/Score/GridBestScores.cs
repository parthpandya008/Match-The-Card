using System;
using UnityEngine;

namespace CardMatch.Score
{
    // Best scores for a specific grid size
    [Serializable]
    public class GridBestScores 
    {
        public string GridSize;              // "2x2", "4x4", etc.
        //Double storage gives clean serialization
        public double BestTime = double.MaxValue;        

        public bool HasBestTime => BestTime != float.MaxValue;
    }
}
