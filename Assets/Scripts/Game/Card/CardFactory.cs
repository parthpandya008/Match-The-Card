using System.Collections.Generic;
using UnityEngine;


namespace CardMatch.Factory
{
    public class CardFactory 
    {
        #region Fields
        // Factory for creating card instances
        private readonly Transform parentTransform;
        private readonly ISpriteProvider spriteProvider;   
        private readonly ObjectPoolManager objectPoolManager;
        private readonly Dictionary<CardType, CardFactoryConfig.CardTypeData> cardTypeData;
        #endregion

        public CardFactory(
            Transform parentTransform,
            ISpriteProvider spriteProvider,
            ObjectPoolManager objectPoolManager,
            CardFactoryConfig config)
        {
            this.parentTransform = parentTransform;
            this.spriteProvider = spriteProvider; 
            this.objectPoolManager = objectPoolManager;

            cardTypeData = new Dictionary<CardType, CardFactoryConfig.CardTypeData>();
            if (config != null && config.cardTypeData != null)
            {
                foreach (var data in config.cardTypeData)
                {
                    if(data.Prefab != null)
                    {
                        cardTypeData[data.CardType] = data;
                    }
                    else
                    {
                        Logger.LogWarning($"CardType {data.CardType} has null prefab in config");
                    }
                }
            }
            else
            {
                Logger.LogError("CardFactoryConfig is null or has no card types!");
            }
        }

        #region Card Creation
        //Create a card
        public ICard CreateCard(CardType cardType, int cardId, Vector3 position, float scale)
        {
            if (!cardTypeData.TryGetValue(cardType, out var data))
            {
                Logger.LogError($"No configuration for card type: {cardType}");
                return null;
            }

            return CreateCard(data, cardId, position, new Vector2(scale,scale));
        }

        public ICard CreateCard(CardType cardType, int cardId, Vector3 position, Vector2 scale)
        {
            if (!cardTypeData.TryGetValue(cardType, out var data))
            {
                Logger.LogError($"No configuration for card type: {cardType}");
                return null;
            }

            return CreateCard(data, cardId, position, scale);
        }

        //Internal creation method
        private ICard CreateCard(CardFactoryConfig.CardTypeData data, int cardId, Vector3 position, Vector2 scale)
        {
            GameObject cardObject = objectPoolManager?.Spawn(data.Prefab);

            if (cardObject == null)
            {
                Logger.LogError("Failed to spawn card from pool");
                return null;
            }
            if (parentTransform == null)
            {
                Logger.LogError("Parent transform is null");
                DestroyCard(cardObject, data.CardType);
                return null;
            }

            // Get Card component
            ICard card = cardObject.GetComponent<ICard>();
            if (card == null)
            {
                DestroyCard(cardObject, data.CardType);
                Logger.LogError("Failed to get ICard component from prefab");
                return null;
            }
            // Set transform properties
            cardObject.transform.SetParent(parentTransform);
            cardObject.transform.localPosition = position;
            cardObject.transform.localScale = new Vector3(scale.x, scale.y, 1f);

            // Initialize with dependencies
            if (card is ICardInitializer setup)
            {
                setup.Initialize(spriteProvider);
                setup.ID = cardId;
            }                    

            return card;
        }
        #endregion

        #region Card Destruction
        //Despawn a specific card
        public void DestroyCard(GameObject cardObject, CardType cardType)
        {
            if (cardObject == null || objectPoolManager == null)
            {
                return;
            }
           
            if (cardTypeData.TryGetValue(cardType, out var data))
            {
                objectPoolManager.Despawn(data.Prefab, cardObject);
            }
            else
            {
                Logger.LogWarning($"Cannot despawn card - unknown type: {cardType}");
            }
        }

        //Despawn a specific card type
        public void DestroyAllTypeCard(CardType cardType)
        {
            if (!cardTypeData.TryGetValue(cardType, out var data))
            {
                Logger.LogWarning($"No configuration for card type: {cardType}");
                return;
            }

            if (parentTransform == null || objectPoolManager == null)
            {
                return;
            }           

            List<GameObject> cardsToDestroy = new List<GameObject>();

            foreach (Transform child in parentTransform)
            {
                ICard card = child.GetComponent<ICard>();
                if (card != null && card.CardType == cardType)
                {
                    cardsToDestroy.Add(child.gameObject);
                }
            }

            foreach (GameObject cardObject in cardsToDestroy)
            {
                objectPoolManager.Despawn(data.Prefab, cardObject);
            }
        }

        // Despawn all cards
        public void DestroyAllCards()
        {
            if (cardTypeData != null)
            {
                foreach(var type in cardTypeData.Keys)
                {
                    DestroyAllTypeCard(type);
                }
            }
        }
        #endregion
    }
}
