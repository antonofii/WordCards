using Newtonsoft.Json;
using System.Collections.Generic;

namespace SharpTest
{
    public class Deck
    {
        [JsonProperty]
        private Card[] _cards;

        public string DeckName { get; private set; }

        public int DeckLenght => _cards.Length;

        public int MemorizedCardAmount { get; private set; }



        public Deck(int deckLenght, string deckName)
        {
            DeckName = deckName;
            _cards = new Card[deckLenght];
        }



        public void SetCard(Card card, int index)
        {
            _cards[index] = card;
        }



        public List<Card> FindNonMemorizedCards()
        {
            List<Card> cards = new List<Card>();

            foreach (Card card in _cards)
            {
                if (!card.IsMemorized)
                {
                    cards.Add(card);
                }
            }
            return cards;
        }



        public void RefreshMemorizedCardAmount()
        {
            int MemorizedCards = 0;
            foreach (Card card in _cards)
            {
                if (card.IsMemorized)
                {
                    MemorizedCards += 1;
                }
            }
            MemorizedCardAmount = MemorizedCards;
        }



        public bool IsFullyStudied()
        {
            foreach (Card card in _cards)
            {
                if (!card.IsMemorized)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
