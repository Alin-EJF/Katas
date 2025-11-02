namespace Trivia
{
    using System;
    using System.Collections.Generic;

    enum Category { 
        Pop = 0,
        Science = 1,
        Sports = 2,
        Rock = 3
    }

    public class Game
    {
        private const int MaxPlayers = 6;
        private const int BoardSize = 12;
        private const int WinningPurse = 6;

        private int _currentPlayer;
        private bool _isGettingOutOfPenaltyBox;
        private readonly int[] _places = new int[MaxPlayers];
        private readonly int[] _purses = new int[MaxPlayers];
        private readonly bool[] _inPenaltyBox = new bool[MaxPlayers];

        private readonly List<string> _players = new List<string>();
        private readonly Dictionary<Category, Queue<string>> _questions =
         new()
         {
             [Category.Pop] = new Queue<string>(),
             [Category.Science] = new Queue<string>(),
             [Category.Sports] = new Queue<string>(),
             [Category.Rock] = new Queue<string>()
         };

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

        //public bool IsPlayable()  //move to Game.Start method
        //{
        //    return (_players.Count >= 2);
        //}

        public void Add(string playerName)
        {
            if (_players.Count >= MaxPlayers)
            {
                Console.WriteLine($"Cannot add '{playerName}': max {MaxPlayers} players reached.");

                return;
            }

            _players.Add(playerName);

            _places[_players.Count] = 0;
            _purses[_players.Count] = 0;
            _inPenaltyBox[_players.Count] = false;

            Console.WriteLine(playerName + " was added");
            Console.WriteLine("They are player number " + _players.Count);
        }

        public void Roll(int roll)
        {
            Console.WriteLine(_players[_currentPlayer] + " is the current player");
            Console.WriteLine("They have rolled a " + roll);

            if (!_inPenaltyBox[_currentPlayer])
            {
                MoveCurrentPlayer(roll);

                Console.WriteLine($"{_players[_currentPlayer]}'s new location is {_places[_currentPlayer]}");
                Console.WriteLine("The category is " + CurrentCategory());

                AskQuestion();

                return;
            }

            if (roll % 2 == 0)
            {
                Console.WriteLine($"{_players[_currentPlayer]} is not getting out of the penalty box");
                _isGettingOutOfPenaltyBox = false;

                return;
            }

            _isGettingOutOfPenaltyBox = true;

            MoveCurrentPlayer(roll);

            Console.WriteLine($"{_players[_currentPlayer]} is getting out of the penalty box");
            Console.WriteLine($"{_players[_currentPlayer]}'s new location is {_places[_currentPlayer]}");
            Console.WriteLine("The category is " + CurrentCategory());

            AskQuestion();
        }

        private void MoveCurrentPlayer(int roll)
        {
            _places[_currentPlayer] = (_places[_currentPlayer] + roll) % BoardSize;
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
            if (_inPenaltyBox[_currentPlayer] && !_isGettingOutOfPenaltyBox)
            {
                NextPlayer();

                return true;
            }

            Console.WriteLine("Answer was corrent!!!!");

            _purses[_currentPlayer]++;

            Console.WriteLine($"{_players[_currentPlayer]} now has {_purses[_currentPlayer]} Gold Coins.");

            var winner = _purses[_currentPlayer] < WinningPurse;

            NextPlayer();

            return winner;
        }

        public void WrongAnswer()
        {
            Console.WriteLine("Question was incorrectly answered");
            Console.WriteLine($"{_players[_currentPlayer]} was sent to the penalty box");

            _inPenaltyBox[_currentPlayer] = true;

            NextPlayer();
        }

        private void NextPlayer()
        {
            _currentPlayer++;

            if (_currentPlayer == _players.Count)
            {
                _currentPlayer = 0;
            }
        }
    }
}