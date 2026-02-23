using CardMatch.Game.Events;

namespace CardMatch.Game.State
{
    public class IdleState : GameStateBase
    {
        public IdleState(GameController controller, GameEvents events)
            : base(controller, events) { }

        public override void Enter()
        {
            Logger.Log("STATE: Idle");
        }
    }
}
