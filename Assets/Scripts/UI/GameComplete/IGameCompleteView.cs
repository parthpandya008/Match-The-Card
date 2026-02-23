using UnityEngine;

namespace CardMatch.UI
{
    public interface IGameCompleteView : IScreenController
    {
        void SetWinLabel(string value);
        void SetBestTimeLabel(string value);       
    }
}
