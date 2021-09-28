using System.Windows;
using System.Windows.Controls;

namespace SharpTest
{
    /// <summary>
    /// Interaction logic for EnterCard.xaml
    /// </summary>
    public partial class EnterCard : Window
    {
        private Deck _deck;
        private int _cardToAddAmount;
        private string _word;
        private string _translation;



        public EnterCard(Deck deck)
        {
            _deck = deck;
            _cardToAddAmount = _deck.DeckLenght;
            InitializeComponent();
        }



        //Enter word
        private void WordTextBoxChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            _word = textBox.Text;
        }



        //Translation
        private void TranslationTextBoxChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            _translation = textBox.Text;
        }



        //Save
        private void ClickSave(object sender, RoutedEventArgs e)
        {
            if (CheckIfTextBoxesFilled())
            {
                if (_cardToAddAmount == 1)
                {
                    AddCard(CreateCard());
                    SayCardAdded();
                    SayDeckIsFull();
                    Close();
                }
                if (_cardToAddAmount > 1)
                {
                    AddCard(CreateCard());
                    SayCardAdded();
                    _cardToAddAmount--;
                }
            }
            else
            {
                SayFillInTheForm();
            }
        }



        private void ResetBoxes(params TextBox[] boxes )
        {

        }



        private bool CheckIfTextBoxesFilled()
        {
            return _translation != null && _translation != "" && _word != null && _word != "";
        }



        private Card CreateCard()
        {
            Card card = new Card(_word, _translation);
            return card;
        }



        private void AddCard(Card card)
        {
            _deck.SetCard(card, _cardToAddAmount - 1);
        }



        private void SayDeckIsFull()
        {
            _ = MessageBox.Show("The deck is full", "Notification", MessageBoxButton.OK, MessageBoxImage.Information);
        }



        private void SayCardAdded()
        {
            _ = MessageBox.Show("This card was added succesfully", "Notification", MessageBoxButton
                    .OK, MessageBoxImage.Information);
        }



        private void SayFillInTheForm()
        {
            _ = MessageBox.Show("Enter a word and its translation", "Warning", MessageBoxButton
                    .OK, MessageBoxImage.Warning);
        }
    }
}
