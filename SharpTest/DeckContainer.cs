using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace SharpTest
{
    public class DeckContainer
    {
        public int TotalCardsAmount { get; private set; }
        public int TotalMemorizedCardsAmount { get; private set; }

        
        private List<Deck> _decks;
        private const string fileName = "data.json";



        public DeckContainer()
        {
            try
            {
                string json = ReadFromFile(fileName);
                _decks = JsonConvert.DeserializeObject<List<Deck>>(json);
            }
            catch
            {
                _decks = new List<Deck>();
            }
        }



        public void AddDeck(Deck deck)
        {
            _decks.Add(deck);
        }



        public string[] GetDecksNames()
        {
            string[] deckList = new string[_decks.Count];

            for (int i = _decks.Count - 1; i >= 0; i--)
            {
                deckList[i] = _decks[i].DeckName;
            }
            return deckList;
        }



        public Deck GetDeck(string deckName)
        {
            foreach (Deck deck in _decks)
            {
                if (deck.DeckName == deckName)
                {
                    return deck;
                }
            }
            return null;
        }



        public void RefreshProgress()
        {
            TotalMemorizedCardsAmount = 0;
            TotalCardsAmount = 0;
            foreach (Deck deck in _decks)
            {
                deck.RefreshMemorizedCardAmount();
                TotalMemorizedCardsAmount += deck.MemorizedCardAmount;
                TotalCardsAmount += deck.DeckLenght;
            }
        }



        public void Save()
        {
            string json = JsonConvert.SerializeObject(_decks);
            WriteToFile(json, fileName);
        }



        private void WriteToFile(string s, string fileName)
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
            FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs);
            sw.WriteLine(s);
            sw.Flush();
            sw.Close();
            fs.Close();
        }



        private string ReadFromFile(string fileName)
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
            FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Read);
            StreamReader sr = new StreamReader(fs);
            string s = sr.ReadLine();
            sr.Close();
            fs.Close();
            return s;
        }
    }
}
