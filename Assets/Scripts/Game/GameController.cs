using System;
using System.Collections.Generic;
using CardMatch.GameEvent;
using CardMatch.GameState;
using CardMatch.Layout;
using Unity.IO.LowLevel.Unsafe;
using UnityEditor.PackageManager;
using UnityEngine;

namespace CardMatch
{
    // Game controller - handles all game logic and flow
    public class GameController : MonoBehaviour
    {
        [Header("Scene References")]
        [SerializeField] private GameObject cardPrefab;
        [SerializeField] private Transform cardContainer;
        [SerializeField] private RectTransform gamePanel;

        // Injected dependencies
        private GameEvents gameEvents;
        private ISpriteProvider spriteProvider;
        private ObjectPoolManager objectPoolManager;


        // Internal systems
        private CardLayoutManager layoutManager;

        // Game state
        private List<Card> cards = new List<Card>();
        private IGameState currentState;

        private int currentRows;
        private int currentCols;

        public void Initialize(GameEvents events, ISpriteProvider spriteProvider, ObjectPoolManager objectPoolManager)
        {
            gameEvents = events;
            this.spriteProvider = spriteProvider;
            this.objectPoolManager = objectPoolManager;
            layoutManager = new CardLayoutManager(gamePanel);
            ChangeState(new IdleState(this, gameEvents));
            Debug.Log("GameController initialized");
        }

        #region Card Setup
        // Initialize cards based on grid size
        public void InitializeCards()
        {
            // Clear existing
            foreach (var card in cards)
                Destroy(card.gameObject);
            cards.Clear();

            // Calculate layout ( supports rows x cols)
            CardLayoutData layout = layoutManager?.CalculateLayout(currentRows, currentCols);            

            // Spawn cards
            for (int i = 0; i < layout.TotalCards; i++)
            {
                Vector3 pos = layoutManager.GetCardPosition(layout, i);
                GameObject cardObj = objectPoolManager?.Spawn(cardPrefab);
                if(cardObj == null)
                {
                    Debug.LogWarning("Failed to spawn card from pool");
                    return;
                }
                cardObj.transform.SetParent (cardContainer);
                cardObj.transform.localPosition = pos;
                cardObj.transform.localScale = Vector3.one * layout.CellScale;

                Card card = cardObj.GetComponent<Card>();
                card.Initialize(spriteProvider);
                card.ID = i;
                cards.Add(card);
            }

            Debug.Log($"Setup {cards.Count} cards");
            gameEvents.RaiseGameStarted();
        }
        #endregion

        #region Game Callbacks 
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
        #endregion

        public void ChangeState(IGameState newState)
        {
            currentState?.Exit();
            currentState = newState;
            currentState.Enter();
        }
    }
}
