using System;
using CardMatch.GameEvent;
using CardMatch.GameState;
using UnityEngine;

namespace CardMatch.GameState
{
    // Playing state - main gameplay
    public class PlayingState : GameStateBase
    {
        private const float TIME_UPDATE_INTERVAL = 1f;
        private float elapsedTime = 0f;
        private float nextTimeUpdate = 0f;
        private GameController controller;
        
        public PlayingState(GameController controller, GameEvents events) : 
            base(controller, events)    
        {
            this.controller = controller;
            if(gameEvents != null)
            {
                gameEvents.OnCardsMismatched += OnCardsMismatched;
            }
        }

        private void OnCardsMismatched(int arg1, int arg2)
        {
            elapsedTime += controller != null ? controller.MisMatchPanelty : 1; // Add penalty time for mismatch
            controller?.SetCardMatchTimer(elapsedTime);
        }

        ~PlayingState()
        {
            if (gameEvents != null)
            {
                gameEvents.OnCardsMismatched -= OnCardsMismatched;
            }
        }
        public override void Enter()
        {
            Logger.Log("STATE: Playing");
            elapsedTime = 0;
            controller?.SetCardMatchTimer(elapsedTime);
        }

        public override void Update(float deltaTime)
        {           
            elapsedTime += deltaTime;
            controller?.SetCardMatchTimer(elapsedTime);
            
            if (elapsedTime >= nextTimeUpdate)
            {
                elapsedTime = MathF.Round(elapsedTime, 4);
                gameEvents.RaiseTimeUpdated(elapsedTime);
                nextTimeUpdate = elapsedTime + TIME_UPDATE_INTERVAL;
            }                
        }
    }
}
