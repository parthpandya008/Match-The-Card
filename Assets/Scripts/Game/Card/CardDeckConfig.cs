using UnityEngine;

[CreateAssetMenu(fileName = "CardDeckConfig", menuName = "Card Match/Card Deck Config")]
public class CardDeckConfig : ScriptableObject
{
    [SerializeField] private Sprite cardBackSprite;
    [SerializeField] private Sprite[] cardFaceSprites;

    public Sprite Back => cardBackSprite;
    public Sprite[] Faces => cardFaceSprites;
}
