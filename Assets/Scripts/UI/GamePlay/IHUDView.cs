using System;
using CardMatch.UI;
using UnityEngine;

namespace CardMatch.UI
{
    public interface IHUDView : IScreenController
    {
        void SetRemainingPairs(string value);
        void SetTime(string value);
        event Action OnMenuButtonClicked;
    }
}
