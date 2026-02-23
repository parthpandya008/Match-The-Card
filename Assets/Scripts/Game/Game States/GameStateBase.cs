using CardMatch.Game.Events;

namespace CardMatch.Game.State
{
    public class GameStateBase : IGameState
    {
        protected GameController gameController;
        protected GameEvents gameEvents;

        protected GameStateBase(GameController controller, GameEvents events)
        {
            gameController = controller;
            gameEvents = events;
        }

        public virtual void Enter() { }
        public virtual void Exit() { }
        public virtual void Update(float deltaTime) { }
    }
}
