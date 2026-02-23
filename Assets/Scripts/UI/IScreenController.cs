using Unity.VisualScripting;
using UnityEngine;

namespace CardMatch.UI
{
    // Defines the minimum contract for any UI screen panel.
    // can drive back-button, history, etc logic
    public interface IScreenController
    {
        void Show();
        void Hide();
    }
}