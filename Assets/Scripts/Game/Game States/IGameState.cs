using UnityEngine;

namespace CardMatch.GameState
{
    /// Interface for game state management
    public interface IGameState
    {
        void Enter();
        void Exit();
        void Update(float deltaTime);
    }
}
