using CardMatch.Game.Events;

namespace CardMatch.Game.State
{
    public class CompletedState : GameStateBase
    {              
        public CompletedState(GameController controller, GameEvents events)
                                : base(controller, events) {}

        public override void Enter()
        {
            if(gameController != null)
            {
                float completionTime = gameController.CardMatchTimer;
                Logger.Log($"STATE: Completed in {completionTime}s ");
                gameEvents?.RaiseGameCompleted(completionTime);
            }            
        }
    }
}
