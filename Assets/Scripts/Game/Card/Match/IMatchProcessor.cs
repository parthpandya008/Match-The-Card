using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using static UnityEngine.InputSystem.InputControlScheme;

namespace CardMatch.Game.Cards
{
    // Interface for match processing logic for  different matching rules
    public interface IMatchProcessor
    {
        bool IsProcessing { get; }
        bool ProcessSelection(ICard card);
        Task<MatchResult> CheckMatchAsync(int waitDuration);
        void Reset();
    }
}
