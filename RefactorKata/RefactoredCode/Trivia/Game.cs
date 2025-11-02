namespace Trivia
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    enum Category { 
        Pop = 0,
        Science = 1,
        Sports = 2,
        Rock = 3
    }

    public class Game
    {
        private readonly List<string> _players = new List<string>();

        private readonly int[] _places = new int[6];
        private readonly int[] _purses = new int[6];
        private readonly bool[] _inPenaltyBox = new bool[6];


        private readonly Dictionary<Category, Queue<string>> _questions =
         new()
         {
             [Category.Pop] = new Queue<string>(),
             [Category.Science] = new Queue<string>(),
             [Category.Sports] = new Queue<string>(),
             [Category.Rock] = new Queue<string>()
         };

        private int _currentPlayer;
        private bool _isGettingOutOfPenaltyBox;

        public Game()
        {
            for (var i = 0; i < 50; i++)
            {
                _questions[Category.Pop].Enqueue($"Pop Question {i}");
                _questions[Category.Science].Enqueue($"Science Question {i}");
                _questions[Category.Sports].Enqueue($"Sports Question {i}");
                _questions[Category.Rock].Enqueue($"Rock Question {i}");
            }
        }

        //public bool IsPlayable()
        //{
        //    return (HowManyPlayers() >= 2);
        //}

        public bool Add(string playerName)
        {
            _players.Add(playerName);
            _places[HowManyPlayers()] = 0;
            _purses[HowManyPlayers()] = 0;
            _inPenaltyBox[HowManyPlayers()] = false;

            Console.WriteLine(playerName + " was added");
            Console.WriteLine("They are player number " + _players.Count);

            return true;
        }

        public int HowManyPlayers()
        {
            return _players.Count;
        }

        public void Roll(int roll)
        {
            Console.WriteLine(_players[_currentPlayer] + " is the current player");
            Console.WriteLine("They have rolled a " + roll);

            if (_inPenaltyBox[_currentPlayer])
            {
                if (roll % 2 == 0)
                {
                    Console.WriteLine(_players[_currentPlayer] + " is not getting out of the penalty box");
                    _isGettingOutOfPenaltyBox = false;

                    return;
                }

                _isGettingOutOfPenaltyBox = true;

                Console.WriteLine(_players[_currentPlayer] + " is getting out of the penalty box");
                _places[_currentPlayer] = _places[_currentPlayer] + roll;

                if (_places[_currentPlayer] > 11)
                {
                    _places[_currentPlayer] = _places[_currentPlayer] - 12;
                }

                Console.WriteLine(_players[_currentPlayer] + "'s new location is " + _places[_currentPlayer]);
                Console.WriteLine("The category is " + CurrentCategory());

                AskQuestion();

                return;
            }

            _places[_currentPlayer] = _places[_currentPlayer] + roll;

            if (_places[_currentPlayer] > 11) {
                _places[_currentPlayer] = _places[_currentPlayer] - 12;
             }

            Console.WriteLine(_players[_currentPlayer]+ "'s new location is " + _places[_currentPlayer]);
            Console.WriteLine("The category is " + CurrentCategory());

            AskQuestion();
        }

        private void AskQuestion()
        {
            var category = CurrentCategory();

            Console.WriteLine(_questions[category].Dequeue());
        }

        private Category CurrentCategory()
        {
            var categoryNumber = _places[_currentPlayer] % 4;

            return categoryNumber switch
            {
                0 => Category.Pop,
                1 => Category.Science,
                2 => Category.Sports,
                _ => Category.Rock
            };
        }

        public bool WasCorrectlyAnswered()
        {
            var winner = false;

            if (_inPenaltyBox[_currentPlayer])
            {
                if (_isGettingOutOfPenaltyBox)
                {
                    Console.WriteLine("Answer was correct!!!!");
                    _purses[_currentPlayer]++;

                    Console.WriteLine(_players[_currentPlayer] + " now has " + _purses[_currentPlayer] + " Gold Coins.");

                    winner = DidPlayerWin();

                    _currentPlayer++;

                    if (_currentPlayer == _players.Count) {
                        _currentPlayer = 0; 
                    }

                    return winner;
                }

                _currentPlayer++;

                if (_currentPlayer == _players.Count)
                {
                    _currentPlayer = 0;
                }

                return true;
            }

            Console.WriteLine("Answer was corrent!!!!");

            _purses[_currentPlayer]++;

            Console.WriteLine(_players[_currentPlayer] + " now has " + _purses[_currentPlayer] + " Gold Coins.");

            winner = DidPlayerWin();

            _currentPlayer++;

            if (_currentPlayer == _players.Count) {
                _currentPlayer = 0; 
            }

            return winner;
        }

        private bool DidPlayerWin()
        {
            return !(_purses[_currentPlayer] == 6); //what tf is this
        }

        public void WrongAnswer()
        {
            Console.WriteLine("Question was incorrectly answered");
            Console.WriteLine(_players[_currentPlayer] + " was sent to the penalty box");

            _inPenaltyBox[_currentPlayer] = true;
            _currentPlayer++;

            if (_currentPlayer == _players.Count)
            {
                _currentPlayer = 0;
            }
        }
    }
}