using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace CardMatch.Game.Cards
{
    // Handles visual representation and animations of a card
    public class CardView : MonoBehaviour
    {
        [SerializeField] private Image image;
        [SerializeField] private RectTransform cardTransform;

        // TODO: Move to grid-specific configuration
        // These timing values should be part of a GridConfig/LevelConfig ScriptableObject
        private const float FLIP_DURATION = 0.75f; // Total duration in seconds
        private bool isAnimating = false;

        public bool IsAnimating => isAnimating;

        #region Unity Lifecycle
        private void Awake()
        {
            if (image == null)
            {
                image = GetComponent<Image>();
            }
            if (cardTransform == null)
            {
                cardTransform = GetComponent<RectTransform>();
            }
        }
        #endregion

        #region View Updates         
        public void UpdateSprite(Sprite sprite)
        {
            if (image != null && sprite != null)
            {
                image.sprite = sprite;
            }
        }

        public void SetMatchCardUI()
        {
            if (image != null)
            {
                Color color = image.color;
                color.a = 0.5f; 
                image.color = color;
            }
        }

        public void Reset()
        {
            if (image != null)
            {
                Color color = image.color;
                color.a = 1f;
                image.color = color;
            }
            isAnimating = false;
        }
        #endregion

        #region Animations
        public void PlayFlipAnimation(Action onMidpoint)
        {
            if (!isAnimating && gameObject.activeSelf)
            {
                StartCoroutine(FlipAnimation(onMidpoint));
            }
        }

        private IEnumerator FlipAnimation(Action onMidpoint)
        {
            isAnimating = true;

            float elapsed = 0f;
            bool midpointTriggered = false;

            // Store initial and end rotation
            Quaternion startRotation = cardTransform.localRotation;
            Quaternion endRotation = startRotation * Quaternion.Euler(0, 180, 0);

            while (elapsed < FLIP_DURATION)
            {
                elapsed += Time.deltaTime;
                float progress = Mathf.Clamp01(elapsed / FLIP_DURATION);                
                
                cardTransform.localRotation = Quaternion.Slerp(startRotation, endRotation, progress);
                // Trigger midpoint at 90 degrees
                if (!midpointTriggered && progress >= 0.5f)
                {
                    midpointTriggered = true;
                    onMidpoint?.Invoke();
                }

                yield return null;
            }

            // Complete the rotation to exactly 180 degrees
            cardTransform.localRotation = startRotation * Quaternion.Euler(0, 180f, 0);

            isAnimating = false;
        }        
        #endregion
    }
}
