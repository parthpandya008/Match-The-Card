using CardMatch.Game.Events;

namespace CardMatch.Game.State
{
    // Initialization state - setting up cards
    public class InitializingState : GameStateBase
    {
        public InitializingState(GameController controller, GameEvents events)
            : base(controller, events) { }

        public override void Enter()
        {
            Logger.Log("STATE: InitializingState");
            gameController?.InitializeCards();
            gameController?.ChangeState(gameController?.RevealState);            
        }
    }
}
