using CardMatch.GameEvent;
using CardMatch;
using UnityEngine;

namespace CardMatch.GameState
{
    // Initialization state - setting up cards
    public class InitializingState : GameStateBase
    {
        public InitializingState(GameController controller, GameEvents events)
            : base(controller, events) { }

        public override void Enter()
        {
            Debug.Log("STATE: InitializingState");
            gameController.InitializeCards();
            gameController.ChangeState(new RevealingState(gameController, gameEvents));
        }
    }
}
