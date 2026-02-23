using UnityEngine;

namespace CardMatch.Game.Cards
{
    // Setup-time interface. Used only by CardFactory and allocation strategies.
    // Do not use in game logic — use ICard instead.
    public interface ICardInitializer
    {
        // To get ID & SpriteID  use ICard instead.
        int ID { set; }
        int SpriteID { set; }
        void Initialize(ISpriteProvider provider);
    }
}
