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
        void Initialize(ISpriteProvider provider);
    }
}
