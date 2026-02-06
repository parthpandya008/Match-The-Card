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

            // Check if total is odd (need to remove one card for pairing)
            bool isOdd = totalCards % 2 == 1;
            if (isOdd)
            {
                totalCards -= 1;
            }

            Vector2 panelSize = panelTransform.sizeDelta;

            // Calculate cell dimensions based on panel size
            float cellWidth = panelSize.x / cols;
            float cellHeight = panelSize.y / rows;

            // Calculate scale - use smaller dimension to keep cards proportional
            float scaleX = 1.0f / cols;
            float scaleY = 1.0f / rows;
            float cellScale = Mathf.Min(scaleX, scaleY);

            // Calculate starting position (centered grid)
            float startX = -cellWidth * (cols / 2.0f);
            float startY = -cellHeight * (rows / 2.0f);

            // Adjust for even grids to center properly
            if (cols % 2 == 0)
            {
                startX += cellWidth / 2;
            }
            if (rows % 2 == 0)
            {
                startY += cellHeight / 2;
            }

            return new CardLayoutData
            {
                TotalCards = totalCards,
                Rows = rows,
                Cols = cols,
                CellScale = cellScale,
                CellWidth = cellWidth,
                CellHeight = cellHeight,
                StartX = startX,
                StartY = startY,
                IsOdd = isOdd
            };
        }
        #endregion

        #region Position Queries
        /// Get position for a specific card index        
        public Vector3 GetCardPosition(CardLayoutData layout, int index)
        {
            int row = index / layout.Cols;
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
        public float CellScale;
        public float StartX;
        public float StartY;
        public bool IsOdd;
    }
}
