using CardMatch.GameEvent;
using CardMatch;
using UnityEngine;

namespace CardMatch.GameState
{
    public class IdleState : GameStateBase
    {
        public IdleState(GameController controller, GameEvents events)
            : base(controller, events) { }

        public override void Enter()
        {
            Debug.Log("STATE: Idle");
        }
    }
}
