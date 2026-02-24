using CardMatch.Game.Events;

namespace CardMatch.Game.State
{
    //Revealing state - showing cards briefly at start
    public class RevealingState : GameStateBase
    {
        private float revealTime;
        // TODO: Move to grid-specific configuration
        // These timing values should be part of a GridConfig/LevelConfig ScriptableObject
        private const float REVEAL_DURATION = 1f;

        public RevealingState(GameController controller, GameEvents events)
            : base(controller, events) { }

        public override void Enter()
        {
            Logger.Log("Game State: Revealing");
            revealTime = 0f;
            gameEvents.RaiseGameStarted();
            gameController?.FlipAllCards(true);
        }

        public override void Update(float deltaTime)
        {
            revealTime += deltaTime;

            if (revealTime >= REVEAL_DURATION)
            {
                gameController?.FlipAllCards(false);
                gameController?.ChangeState(gameController?.PlayState);
            }
        }
    }
}
