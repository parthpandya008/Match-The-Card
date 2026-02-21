using UnityEngine;

namespace CardMatch.Layout
{
    // Calculates card positions and sizes for grid layout
    public class CardLayoutManager
    {
        private readonly RectTransform panelTransform;

        public CardLayoutManager(RectTransform panelTransform)
        {
            this.panelTransform = panelTransform;
        }

        #region Layout Calculation
        // Calculate grid layout for cards (rectangular grid)
        // Supports any rows x cols combination (2x3, 5x6, etc.)               
        public CardLayoutData CalculateLayout(int rows, int cols)
        {
            int totalCards = rows * cols;
            bool isOdd = totalCards % 2 == 1;
            if (isOdd)
            {
                totalCards -= 1;
            }

            Vector2 panelSize = panelTransform.sizeDelta;

            float cellWidth = panelSize.x / cols;
            float cellHeight = panelSize.y / rows;

            // Scale each axis independently to fill the panel correctly
            float scaleX = cellWidth / panelSize.x;  
            float scaleY = cellHeight / panelSize.y; 
            
            // Calculate starting position
            float startX = -cellWidth * (cols / 2.0f);
            float startY = -cellHeight * (rows / 2.0f);           
            startX += cellWidth / 2;
            startY += cellHeight / 2;

            return new CardLayoutData
            {
                TotalCards = totalCards,
                Rows = rows,
                Cols = cols,
                CellScale = new Vector2(scaleX, scaleY),                
                CellWidth = cellWidth,
                CellHeight = cellHeight,
                StartX = startX,
                StartY = startY,
                IsOdd = isOdd
            };
        }

        #endregion

        #region Position Queries
        
        // Get position for a specific card index        
        public Vector3 GetCardPosition(CardLayoutData layout, int index)
        {
            // If grid is odd, swap center card to last slot
            if (layout.IsOdd)
            {
               int centerIndex = layout.Rows * layout.Cols / 2; // Middle index for odd card count
               int lastIndex = layout.TotalCards;

                if(index == centerIndex)
                {
                    index = lastIndex; // Place the odd card in the center
                }
            }

           int row = index/ layout.Cols;
           int col = index % layout.Cols;

           float x = layout.StartX + col * layout.CellWidth;
           float y = layout.StartY + row * layout.CellHeight;

           return new Vector3(x, y, 0);
        }
          
        #endregion
    }

    public class CardLayoutData
    {
        public int TotalCards;
        public int Rows;
        public int Cols;
        public float CellWidth;
        public float CellHeight;
        public Vector2 CellScale;        
        public float StartX;
        public float StartY;
        public bool IsOdd;
    }
}
