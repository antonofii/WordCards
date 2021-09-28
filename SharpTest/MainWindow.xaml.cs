using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace SharpTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string _deckName;
        private int _deckLenght;
        private int _numberOfWords;
        private string _choosenDeckName;
        private readonly DeckContainer _deckContainer;



        public int DeckLenght
        {
            get => _deckLenght;

            private set
            {
                if (value < 1)
                {
                    SayWrongNumberEntered();
                }
                else
                {
                    _deckLenght = value;
                }
            }
        }



        public int NumberOfWords
        {
            get => _numberOfWords;

            private set
            {
                if (value < 1)
                {
                    SayWrongNumberEntered();
                }
                else
                {
                    _numberOfWords = value;
                }
            }
        }



        public MainWindow()
        {
            InitializeComponent();
            _deckContainer = new DeckContainer();
            DeckBoxUpdate();
        }



        //Enter deck name
        private void DeckNameBoxChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            _deckName = textBox.Text;
        }



        //Enter deck lenght
        private void DeckLenghtBoxChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                TextBox textBox = sender as TextBox;
                if (textBox.Text == "")
                {
                    textBox.Text = null;
                }
                else
                {
                    _deckLenght = int.Parse(textBox.Text);
                }
            }
            catch
            {
                SayWrongNumberEntered();
            }
        }



        //Create new deck
        private void ClickCreateNewDeck(object sender, RoutedEventArgs e)
        {
            if (_deckLenght != 0 & _deckName != null)
            {
                Deck deck = CreateNewDeck();
                FillNewDeckWindow(deck);
                _deckContainer.AddDeck(deck);
                _deckContainer.Save();
                DeckBoxUpdate();
            }
            else
            {
                SayFillCardFields();
            }
        }



        //List of decks names
        private void DecksNamesBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _choosenDeckName = (string)decksComboBox.SelectedItem;
        }



        //List of decks
        private void DeckBoxUpdate()
        {
            ClearComboBox(decksComboBox);
            FIllComboBoxWithDeckNames(decksComboBox);
        }



        //Enter number of words
        private void WordsAmountBoxChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                TextBox textBox = sender as TextBox;
                if (textBox.Text == "")
                {
                    textBox.Text = null;
                }
                else
                {
                    NumberOfWords = int.Parse(textBox.Text);
                }
            }
            catch
            {
                SayWrongNumberEntered();
            }
        }



        //Play button
        private void PlayButtonClick(object sender, RoutedEventArgs e)
        {
            if (_choosenDeckName != null && NumberOfWords != 0)
            {
                Deck deck = _deckContainer.GetDeck(_choosenDeckName);

                if (!deck.IsFullyStudied())
                {
                    Play(deck);
                }
                else
                {
                    SayDeckIsFullyStudied();
                }
            }
            else
            {
                SayFillBoxesBeforePlay();
            }
        }



        //Show progress
        private void ShowProgressClick(object sender, RoutedEventArgs e)
        {
            RefreshDeckProgress();
            ShowProgress();
        }



        private void FIllComboBoxWithDeckNames(ComboBox comboBox)
        {
            string[] deckList = _deckContainer.GetDecksNames();

            foreach (string deckName in deckList)
            {
                comboBox.Items.Add(deckName);
            }
        }



        private void RefreshDeckProgress()
        {
            _deckContainer.RefreshProgress();
        }



        private void ClearComboBox(ComboBox comboBox)
        {
            comboBox.Items.Clear();
        }



        private void FillNewDeckWindow(Deck deck)
        {
            EnterCard enterWindow = new EnterCard(deck);
            enterWindow.ShowDialog();
        }



        private Deck CreateNewDeck()
        {
            Deck deck = new Deck(_deckLenght, _deckName);
            return deck;
        }


        private void ShowMessageBox(string text, string title, MessageBoxButton buttonType, MessageBoxImage imageType)
        {
            MessageBox.Show(text, title, buttonType, imageType);
        }


        private void ShowProgress()
        {
            string text = "Your progress is " + _deckContainer.TotalMemorizedCardsAmount + " words of "
                            + _deckContainer.TotalCardsAmount;
            ShowMessageBox(text, "You can always do it better!", MessageBoxButton.OK, MessageBoxImage.Information);
        }



        private void SayFillBoxesBeforePlay()
        {
            ShowMessageBox("Firstly, enter a correct number of words and choose or create a deck", "Warning",
                    MessageBoxButton.OK, MessageBoxImage.Error);
        }



        private void SayDeckIsFullyStudied()
        {
            ShowMessageBox("This deck is fully studied. Try another one or create a new one", "Wrong choice",
                        MessageBoxButton.OK, MessageBoxImage.Information);
        }



        private void SayWrongNumberEntered()
        {
            ShowMessageBox("Please, use positive integers only", "Wrong input", MessageBoxButton.OK, MessageBoxImage.Error);
        }



        private void SayFillCardFields()
        {
            ShowMessageBox("Please, fill in all the fields", "Wrong input", MessageBoxButton.OK, MessageBoxImage.Error);
        }



        private void ShowCardTranslation(Card card)
        {
            ShowMessageBox(card.Word + " means " + card.Translation, "That's easy!",
                    MessageBoxButton.OK, MessageBoxImage.Question);
        }



        private void SayTheWordsAreSrudied()
        {
            ShowMessageBox("Congrats", "You have just studied all the words",
                MessageBoxButton.OK, MessageBoxImage.Information);
        }



        private MessageBoxResult ShowCard(Card card)
        {
            return MessageBox.Show(card.Word, "Do you remember this word? ",
                    MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
        }



        private void Play(Deck deck)
        {
            int numberOfAttempts = _numberOfWords;
            List<Card> cards;
            Card card;

            while (numberOfAttempts != 0 & !deck.IsFullyStudied())
            {
                cards = deck.FindNonMemorizedCards();
                for (int j = 0; j < cards.Count && numberOfAttempts > 0; j++)
                {
                    card = cards[j];
                    MessageBoxResult result = ShowCard(card);
                    switch (result)
                    {
                        case MessageBoxResult.Yes :
                            card.IsMemorized = true;
                            _deckContainer.Save();
                            break;
                        case MessageBoxResult.Cancel:
                            return;
                        case MessageBoxResult.No:
                            ShowCardTranslation(card);
                            break;
                    }
                    numberOfAttempts--;
                }
            }
            SayTheWordsAreSrudied();
        }
    }
}
