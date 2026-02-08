using System.Collections.Generic;
using UnityEngine;

namespace CardMatch.Factory
{
    [CreateAssetMenu(fileName = "CardFactoryConfig", menuName = "Card Match/Card Factory Config")]
    public class CardFactoryConfig : ScriptableObject
    {
        [System.Serializable]
        public class CardTypeData
        {
            [Header("Scene References")]
            [SerializeField] private CardType cardType = CardType.Default;
            [SerializeField] private GameObject prefab;
            [SerializeField] private float defaultScale = 1.0f;

            public CardType CardType => cardType;
            public GameObject Prefab => prefab;
            public float DefaultScale => defaultScale;
        }

        public CardTypeData[] cardTypeData;

        private void OnValidate()
        {
            // Check for duplicate card types
            HashSet<CardType> uniqueTypes = new HashSet<CardType>();
            foreach (var data in cardTypeData)
            {
                if (!uniqueTypes.Add(data.CardType))
                {
                    Debug.LogWarning($"Duplicate CardType found: {data.CardType} in {name}");
                }
            }
        }
    }

    public enum CardType
    {
        Default,
        Bonus,
        Special,
    }
}
