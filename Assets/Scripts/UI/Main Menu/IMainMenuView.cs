using System;
using CardMatch.UI;
using UnityEngine;

namespace CardMatch.UI
{
    public interface IMainMenuView : IScreenController
    {        
        event Action<int> OnRowsChanged;     
        event Action<int> OnColsChanged;

        event Action OnStartButtonClicked;

        void SetRowsLabel(string value);
        void SetColsLabel(string value);
    }
}
