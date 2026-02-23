using System;
using UnityEngine;

namespace CardMatch.UI.Events
{
    public class UIEvents
    {
        public event Action OnStartButtonClicked;
        public event Action OnStopButtonClicked;

        public void RaiseStartButtonClicked() => OnStartButtonClicked?.Invoke();
        public void RaiseStopButtonClicked() => OnStopButtonClicked?.Invoke();

    }
}
