namespace Trivia
{
    using System;

    public class GameRunner
    {
        private static bool _notAWinner;

        public static void Main(string[] args)
        {
            var game = new Game();

            game.Add("Chet");
            game.Add("Pat");
            game.Add("Sue");

            var rand = new Random();

            do
            {
                game.Roll(rand.Next(5) + 1);

                if (rand.Next(9) == 7)
                {
                    game.WrongAnswer();
                }
                else
                {
                    _notAWinner = game.WasCorrectlyAnswered();
                }
            } while (_notAWinner);
        }
    }
}