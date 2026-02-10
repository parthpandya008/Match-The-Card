using CardMatch.GameEvent;
using CardMatch.GameState;
using CardMatch;
using UnityEngine;

namespace CardMatch.GameState
{
    public class CompletedState : GameStateBase
    {
        public CompletedState(GameController controller, GameEvents events)
            : base(controller, events) { }

        public override void Enter()
        {
            Logger.Log("STATE: Completed - YOU WIN!");
            gameEvents.RaiseGameCompleted();
        }
    }
}
