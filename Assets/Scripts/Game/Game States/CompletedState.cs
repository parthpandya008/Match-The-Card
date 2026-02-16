using CardMatch.GameEvent;
using CardMatch.GameState;
using CardMatch;
using UnityEngine;

namespace CardMatch.GameState
{
    public class CompletedState : GameStateBase
    {
        private readonly float completionTime;
        private readonly bool isNewRecord;
        public CompletedState(GameController controller, GameEvents events, 
                              float completionTime, bool newRecord)
                                : base(controller, events) 
        {
            this.completionTime = completionTime;
            isNewRecord = newRecord;
        }

        public override void Enter()
        {
            Logger.Log($"STATE: Completed in {completionTime}s, " +
                       $"Is new record {isNewRecord} - YOU WIN!");
            gameEvents.RaiseGameCompleted(completionTime, isNewRecord);
        }
    }
}
