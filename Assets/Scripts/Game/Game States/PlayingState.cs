using CardMatch.GameEvent;
using CardMatch.GameState;
using UnityEngine;

namespace CardMatch.GameState
{
    // Playing state - main gameplay
    public class PlayingState : GameStateBase
    {
        public PlayingState(GameController controller, GameEvents events) : 
            base(controller, events)    { }
        public override void Enter()
        {
            Debug.Log("STATE: Playing");
        }
    }
}
