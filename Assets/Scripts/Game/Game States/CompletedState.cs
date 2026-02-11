using CardMatch.GameEvent;
using CardMatch.GameState;
using CardMatch;
using UnityEngine;

namespace CardMatch.GameState
{
    public class CompletedState : GameStateBase
    {
        private readonly float completionTime;
        public CompletedState(GameController controller, GameEvents events, float completionTime)
            : base(controller, events) 
        {
            this.completionTime = completionTime;
        }

        public override void Enter()
        {
            Logger.Log($"STATE: Completed in {completionTime}s - YOU WIN!");
            gameEvents.RaiseGameCompleted();
        }
    }
}
