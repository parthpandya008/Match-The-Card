using UnityEngine;
using UnityEngine.UI;

namespace CardMatch
{
    // Handles visual representation and animations of a card
    public class CardView : MonoBehaviour
    {
        [SerializeField] private Image image;

        public GameObject GameObject => gameObject;

        #region Unity Lifecycle
        private void Awake()
        {
            if (image == null)
            {
                image = GetComponent<Image>();
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
        }
        #endregion
    }
}
