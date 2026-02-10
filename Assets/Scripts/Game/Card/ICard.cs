using System;
using CardMatch.Factory;
using UnityEngine;

namespace CardMatch
{
    /// Interface for any clickable card implementation
    public interface ICard
    {
        int ID { get; set; }
        int SpriteID { get; set; }
        CardType CardType { get;}

        bool IsFlipped { get;} //// true = face up
        void Initialize(ISpriteProvider provider);
        void Flip(bool faceUp);
        void SetMatched();

        // Event for card click
        event Action<ICard> OnCardClicked;
    }
}
