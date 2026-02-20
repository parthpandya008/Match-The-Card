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
        
        
        public PlayingState(GameController controller, GameEvents events) : 
            base(controller, events) {  }

        private void OnCardsMismatched(int arg1, int arg2)
        {
            elapsedTime += gameController != null ? gameController.MisMatchPanelty : 1; // Add penalty time for mismatch
            gameController?.SetCardMatchTimer(elapsedTime);
        }
       
        public override void Enter()
        {
            Logger.Log("STATE: Playing");
            elapsedTime = 0;
            nextTimeUpdate = 0f;
            gameController?.SetCardMatchTimer(elapsedTime);
            if (gameEvents != null)
            {
                gameEvents.OnCardsMismatched += OnCardsMismatched;
            }
        }

        public override void Exit()
        {
            if (gameEvents != null)
            {
                gameEvents.OnCardsMismatched += OnCardsMismatched;
            }           
        }

        public override void Update(float deltaTime)
        {           
            elapsedTime += deltaTime;
            gameController?.SetCardMatchTimer(elapsedTime);
            
            if (elapsedTime >= nextTimeUpdate)
            {
                elapsedTime = MathF.Round(elapsedTime, 1);
                gameEvents?.RaiseTimeUpdated(elapsedTime);
                nextTimeUpdate = elapsedTime + TIME_UPDATE_INTERVAL;
            }                
        }
    }
}
