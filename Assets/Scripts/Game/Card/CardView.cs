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
        // Start is called once before the first execution of Update after the MonoBehaviour is created       
        public void UpdateSprite(Sprite sprite)
        {
            if (image != null && sprite != null)
            {
                image.sprite = sprite;
            }
        }
        #endregion
    }
}
