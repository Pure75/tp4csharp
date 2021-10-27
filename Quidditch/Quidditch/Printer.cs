using System;

namespace Quidditch
{
    public class Printer
    {
        public void PrintGame(Game game)
        {
            Console.Clear();
            Console.CursorVisible = false;
            PrintPlayers(game);
            PrintBalls(game);
            PrintField(game);
            PrintInfo(game);
        }
        
        private void PrintField(Game game)
        {
            // Print the entire field
            PrintRect(0, 0, game.GetFieldWidth(), game.GetFieldHeight());
            // Print the left ring
            PrintRect(2, game.GetFieldHeight() / 2 - 1, 3, 3);
            // Print the right ring
            PrintRect(game.GetFieldWidth()-5,game.GetFieldHeight() / 2 - 1, 3, 3);
        }
        
        // Print a rectangle
        private void PrintRect(int x, int y, int width, int height)
        {
            PrintLine(x,y,width);
            PrintLine(x,y+height-1,width);
            PrintColumn(x,y,height);
            PrintColumn(x+width-1,y,height);
        }
        
        private static void PrintLine(int x, int y, int width)
        {
            Console.SetCursorPosition(x,y);
            Console.BackgroundColor = ConsoleColor.White;
            for (int i = 0; i < width; i++)
                Console.Write(" ");
        }
        private static void PrintColumn(int x, int y, int height)
        {
            Console.BackgroundColor = ConsoleColor.White;
            for (int i = 0; i < height; i++)
            {
                Console.SetCursorPosition(x,y+i);
                Console.Write(" ");
            }
        }
        
        private static void PrintPlayers(Game game)
        {
            Console.ResetColor();
            foreach (var player in game.players)
            {
                Console.SetCursorPosition(player.X, player.Y);
                Console.ForegroundColor = player.Color;
                Console.Write(player.ToChar);
            }
        }
        
        private static void PrintBalls(Game game)
        {
            Console.ResetColor();
            foreach (var ball in game.balls.all)
            {
                Console.SetCursorPosition(ball.X, ball.Y);
                Console.ForegroundColor = ball.Color;
                if (ball is Quaffle && ((Quaffle) ball).taker != null)
                    continue;
                Console.Write(ball.ToChar);
            }
        }
        
        private static void PrintInfo(Game game)
        {
            Console.ResetColor();
            Console.SetCursorPosition(game.GetFieldWidth() / 2 - 2, game.GetFieldHeight()+1);
            Console.Write(game.score[0] + "-" + game.score[1]);
            Console.SetCursorPosition(game.GetFieldWidth() / 2 - 2, game.GetFieldHeight()+2);
            Console.Write("Time : " + game.time);
            int i = 4;
            foreach (var player in game.players)
            {
                string str = player.ToChar.ToString();
                Console.ForegroundColor = player.Team != 0 ? ConsoleColor.Red : ConsoleColor.Green;
                if (player is Chaser)
                {
                    var c = (Chaser) player;
                    str += ": ko = " + c.Ko;
                    if (c.Quaffle)
                        str += ", Quaffle";
                }
                else if (player is Keeper)
                {
                    if (((Keeper) player).Quaffle)
                        str += ", Quaffle";
                }
                else
                    continue;
                Console.SetCursorPosition(game.GetFieldWidth() + 3, i);
                Console.Write(str);
                i++;
            }
        }
    }
}
