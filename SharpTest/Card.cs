
namespace SharpTest
{
    public class Card
    {
        public bool IsMemorized { get; set; }
        public string Word { get; private set; }
        public string Translation { get; private set; }

        public Card(string word, string translation)
        {
            Word = word;
            Translation = translation;
            IsMemorized = false;
        }
    }
}
