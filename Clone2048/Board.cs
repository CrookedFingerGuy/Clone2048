using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clone2048
{
    public class Board
    {
        public Board()
        {

        }

        public void DrawBoard(GameStateData lgsd, BoardSpot lbs)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.SetCursorPosition(9, 2);
            Console.Write("╔═════╦═════╦═════╦═════╗");
            Console.SetCursorPosition(9, 3);
            Console.Write("║     ║     ║     ║     ║");
            Console.SetCursorPosition(9, 4);
            Console.Write("║     ║     ║     ║     ║");
            Console.SetCursorPosition(9, 5);
            Console.Write("║     ║     ║     ║     ║");
            Console.SetCursorPosition(9, 6);
            Console.Write("╠═════╬═════╬═════╬═════╣");
            Console.SetCursorPosition(9, 7);
            Console.Write("║     ║     ║     ║     ║");
            Console.SetCursorPosition(9, 8);
            Console.Write("║     ║     ║     ║     ║");
            Console.SetCursorPosition(9, 9);
            Console.Write("║     ║     ║     ║     ║");
            Console.SetCursorPosition(9, 10);
            Console.Write("╠═════╬═════╬═════╬═════╣");
            Console.SetCursorPosition(9, 11);
            Console.Write("║     ║     ║     ║     ║");
            Console.SetCursorPosition(9, 12);
            Console.Write("║     ║     ║     ║     ║");
            Console.SetCursorPosition(9, 13);
            Console.Write("║     ║     ║     ║     ║");
            Console.SetCursorPosition(9, 14);
            Console.Write("╠═════╬═════╬═════╬═════╣");
            Console.SetCursorPosition(9, 15);
            Console.Write("║     ║     ║     ║     ║");
            Console.SetCursorPosition(9, 16);
            Console.Write("║     ║     ║     ║     ║");
            Console.SetCursorPosition(9, 17);
            Console.Write("║     ║     ║     ║     ║");
            Console.SetCursorPosition(9, 18);
            Console.Write("╚═════╩═════╩═════╩═════╝");

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    Console.SetCursorPosition(10 + 6 * i, 4 + 4 * j);
                    Console.ForegroundColor = (ConsoleColor)((int)Math.Log((double)lgsd.boardValues[i, j], (double)2) % 16 + 1);
                    if (lbs.X == i && lbs.Y == j)
                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.Write(lgsd.boardValues[i, j]);
                }
            }

            Console.SetCursorPosition(15, 1);
            Console.Write("Score: " + lgsd.score);
        }
    }
}
