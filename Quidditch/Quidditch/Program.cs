using System;

namespace Quidditch
{
    class Program
    {
        static void Main(string[] args)
        {
            Game game = new Game(40, 15);
            int winner=game.Play();
            Console.WriteLine("The Winner is the team {0}",winner);
        }
    }
}
