using CardMatch.GameEvent;
using CardMatch;
using UnityEngine;

namespace CardMatch.GameState
{
    public class InitializingState : GameStateBase
    {
        public InitializingState(GameController controller, GameEvents events)
            : base(controller, events) { }

        public override void Enter()
        {
            Debug.Log("STATE: InitializingState");
        }
    }
}
