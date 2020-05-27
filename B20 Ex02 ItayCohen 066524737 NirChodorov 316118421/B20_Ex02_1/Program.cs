using System;

namespace B20_Ex02_1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            GameCli m_MemoryGame = new GameCli();
            m_MemoryGame.InitializeGame();
            m_MemoryGame.Start();
            Console.WriteLine("Thank you for playing !");
            Console.WriteLine("Press any key to exit .. ");
            Console.ReadKey();
        }
    }
}
