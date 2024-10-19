using System.ComponentModel;
using System.Diagnostics;

namespace Hangman
{
    public partial class MainPage : ContentPage, INotifyPropertyChanged
    {
        #region UI Properties

        public string Spotlight
        {
            get { return spotlight; }
            set
            {
                spotlight = value;
                OnPropertyChanged();

            }
        }

        public List<char> Letters
        {
            get { return letters; }
            set
            {
                letters = value;
                OnPropertyChanged();
            }
        }

        public string Message
        {
            get { return message; }
            set
            {
                message = value;
                OnPropertyChanged();
            }
        }

        public string GameStatus
        {
            get { return gameStatus; }
            set
            {
                gameStatus = value;
                OnPropertyChanged();
            }
        }

        public string CurrentImage
        {
            get { return currentImage; }
            set
            {
                currentImage = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Fields

        private List<string> words =
            ["apple", "banana", "cherry", "date", "elderberry", 
                "fig", "grape", "honeydew", "kiwi", "lemon",
                "mango", "nectarine", "orange", "papaya", "quince",
                "raspberry", "strawberry", "tangerine", "watermelon"];

        private string answer = "";
        private string spotlight;
        private List<char> guessed = new List<char>();
        private List<char> letters = new List<char>();
        private string message;
        private int mistakes = 0;
        private int maxMistakes = 6;
        private string gameStatus;
        private string currentImage = "img0.png";

        #endregion Fields

        public MainPage()
        {
            InitializeComponent();
            Letters.AddRange("abcdefghijklmnopqrstuvwxyz");
            BindingContext = this;
            PickWord();
            CalculateWord(answer, guessed);
        }

        #region Game Engine

        private void PickWord()
        {
            answer = words[new Random().Next(words.Count)];
            Debug.WriteLine(answer);
        }

        private void CalculateWord(string answer, List<char> guessed)
        {
            var temp = answer.Select(x => (guessed.IndexOf(x) >= 0) ? x : '_').ToArray();
            Spotlight = string.Join(" ", temp);
        }

        #endregion

        private void Button_OnClicked(object? sender, EventArgs e)
        {
            var btn = sender as Button;
            if (btn != null)
            {
                var letter = btn.Text;
                btn.IsEnabled = false;
                HandleGuess(letter[0]);
            }
        }

        private void HandleGuess(char letter)
        {
            if (guessed.IndexOf(letter) == -1)
            {
                guessed.Add(letter);
            }

            if (answer.IndexOf(letter) >= 0)
            {
                CalculateWord(answer, guessed);
                CheckifGameWon();
            } else if (answer.IndexOf(letter) == -1)
            {
                mistakes++;
                UpdateStatus();
                CheckifGameLost();
            }
        }

        private void CheckifGameLost()
        {
            if (mistakes == maxMistakes)
            {
                Message = "You Lost!";
                DisableAllButtons();
            }
        }

        private void CheckifGameWon()
        {
            if (Spotlight.Replace(" ", "") == answer)
            {
                Message = "You Won!";
                DisableAllButtons();
            }
        }

        private void UpdateStatus()
        {
            GameStatus = $"Erorrs: {mistakes} of {maxMistakes}";
            CurrentImage = $"img{mistakes}.png";
        }

        private void DisableAllButtons()
        {
            // disable all buttons
            foreach (var child in FlexLayout.Children)
            {
                if (child is Button btn)
                {
                    btn.IsEnabled = false;
                }
            }
        }
    }
}