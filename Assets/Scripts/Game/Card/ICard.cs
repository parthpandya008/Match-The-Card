using System;
using CardMatch.Game.Cards.Factory;

namespace CardMatch.Game.Cards
{
    /// Interface for any clickable card implementation
    public interface ICard
    {
        //read-only access to ID and SpriteID, because game logic should only observe, never mutate it.
        // To set ID or SpriteID, use ICardInitializer 
        int ID { get;}
        int SpriteID { get;}
        CardType CardType { get;}

        bool IsAnimating { get; }

        bool IsFlipped { get;} //// true = face up
       
        void Flip(bool faceUp);
        void SetMatched();

        void Reset();

        // Event for card click
        event Action<ICard> OnCardClicked;
    }
}
