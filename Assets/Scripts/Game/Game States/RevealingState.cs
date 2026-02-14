using CardMatch.GameEvent;
using CardMatch.GameState;
using CardMatch;
using UnityEngine;

namespace CardMatch.GameState
{
    //Revealing state - showing cards briefly at start
    public class RevealingState : GameStateBase
    {
        private float revealTime;
        //TODO: Move this to a config or difficulty settings/ GameData (Scriptable Object)
        //OR [As per the requirement]
        private const float REVEAL_DURATION = 1f;

        public RevealingState(GameController controller, GameEvents events)
            : base(controller, events) { }

        public override void Enter()
        {
            Logger.Log("Game State: Revealing");
            revealTime = 0f;
            gameEvents.RaiseGameStarted();
            gameController.FlipAllCards(true);
        }

        public override void Update(float deltaTime)
        {
            revealTime += deltaTime;

            if (revealTime >= REVEAL_DURATION)
            {
                gameController.FlipAllCards(false);
                gameController.ChangeState(new PlayingState(gameController, gameEvents));
            }
        }
    }
}
