using UnityEngine;

namespace CardMatch.Game.State
{
    /// Interface for game state management
    public interface IGameState
    {
        void Enter();
        void Exit();
        void Update(float deltaTime);
    }
}
