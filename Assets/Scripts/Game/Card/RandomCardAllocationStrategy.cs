using UnityEngine;

namespace CardMatch.Game.Cards
{
    /* 
     * Randomly allocates sprite pairs to cards 
     *  Uses Fisher-Yates shuffle algorithm for uniform random distribution.
     */
    public class RandomCardAllocationStrategy : ICardAllocationStrategy
    {
        public void AllocateSprites(ICard[] cards, int availableSpritesCount)
        {
            if (cards == null || cards.Length == 0) return;

            int pairCount = cards.Length / 2;
            // Select random unique sprites from available pool
            int[] selectedSpriteIds = SelectRandomSprites(pairCount, availableSpritesCount);

            // Create pairs array where each sprite ID appears twice consecutively
            int[] spriteIds = new int[cards.Length];
            for (int i = 0; i < pairCount; i++)
            {
                spriteIds[i * 2] = selectedSpriteIds[i];
                spriteIds[i * 2 + 1] = selectedSpriteIds[i];
            }

            // Shuffle the pairs using Fisher-Yates
            ShuffleArray(spriteIds);

            // Assign sprite IDs  to cards
            for (int i = 0; i < cards.Length; i++)
            {
                if (cards[i] is ICardInitializer setup)
                {
                    setup.SpriteID = spriteIds[i];
                }                                    
            }
        }

        // Selects a specified number of random unique sprites from the available pool.
        private int[] SelectRandomSprites(int count, int availableSpritesCount)
        {
            int[] allIds = new int[availableSpritesCount];
            for (int i = 0; i < availableSpritesCount; i++)
            {
                allIds[i] = i;
            }

            //  Fisher-Yates shuffle - only shuffle the first 'count' elements
            for (int i = 0; i < Mathf.Min(count, availableSpritesCount); i++)
            {
                int randomIndex = Random.Range(i, availableSpritesCount);
                int temp = allIds[i];
                allIds[i] = allIds[randomIndex];
                allIds[randomIndex] = temp;
            }

            // Take first 'count' elements
            int[] selectedIds = new int[count];
            for (int i = 0; i < count; i++)
            {
                selectedIds[i] = allIds[i % availableSpritesCount];
            }
            return selectedIds;
        }

        // Shuffles an array in-place using Fisher-Yates algorithm.
        private void ShuffleArray(int[] array)
        {
            for (int i = array.Length - 1; i > 0; i--)
            {
                int randomIndex = Random.Range(0, i + 1);
                // Swap
                int temp = array[i];
                array[i] = array[randomIndex];
                array[randomIndex] = temp;
            }
        }
    }
}