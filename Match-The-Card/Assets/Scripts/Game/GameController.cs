using System;
using CardMatch.GameEvent;
using CardMatch.GameState;
using Unity.IO.LowLevel.Unsafe;
using UnityEditor.PackageManager;
using UnityEngine;

namespace CardMatch
{
    public class GameController : MonoBehaviour
    {
        [Header("Scene References")]
        [SerializeField] private RectTransform gamePanel;

        private GameEvents gameEvents;
        private ISpriteProvider spriteProvider;
        private IGameState currentState;

        private int currentRows;
        private int currentCols;

        public void Initialize(GameEvents events, ISpriteProvider spriteProvider)
        {
            gameEvents = events;
            this.spriteProvider = spriteProvider;
            ChangeState(new IdleState(this, gameEvents));
            Debug.Log("GameController initialized");
        }

        public void StartGame(int rows, int cols)
        {
            currentRows = rows;
            currentCols = cols;
            ChangeState(new InitializingState(this, gameEvents));
        }

        public void StopGame()
        {
            ChangeState(new IdleState(this, gameEvents));
        }

        public void ChangeState(IGameState newState)
        {
            currentState?.Exit();
            currentState = newState;
            currentState.Enter();
        }
    }
}
