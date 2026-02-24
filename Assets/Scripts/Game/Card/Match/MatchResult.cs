using UnityEngine;

namespace CardMatch.Game.Cards
{
    public struct MatchResult
    {
        public bool IsMatch { get; }
        public int FirstCardId { get; }
        public int SecondCardId { get; }

        public MatchResult(bool isMatch, int first, int second)
        {
            IsMatch = isMatch;
            FirstCardId = first;
            SecondCardId = second;
        }
    }
}
