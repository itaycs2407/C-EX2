using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B20_Ex02_1
{
    public class Cli 
    {
#region props
        private Logic m_GameLogic;
        private const int SLEEP_TIME = 2000;
        #endregion
#region c'tor
        public Cli()
        {
            m_GameLogic = new Logic();
            
        }
        #endregion
#region Public Methods
        public void InitializeGame()
        {
            initializePlayers();
            initializeGrid();
        }

        public void Start()
        {
            playGame();
        }
        #endregion
#region Private Methods
        private void initializeGrid()
        {
            int rowsCount = 0, colsCount = 0;
            do
            {
                rowsCount = getDimension("rows");
                colsCount = getDimension("columns");
            }
            while (!m_GameLogic.TryCreateGrid(rowsCount, colsCount));
            
        }

        private int getDimension(string i_DimensionNameForUserInput)
        {
            string dimensionInput;
            int dimension;
            do
            {
                Console.WriteLine(String.Format(@"Enter number of {0} , between 4 and 6 (include) :", i_DimensionNameForUserInput));
                dimensionInput = Console.ReadLine();
            }
            while(!int.TryParse(dimensionInput, out dimension));
            
            return dimension;
        }
       
    

        private void playGame() {
            int playerTurnId = 0;

            while (m_GameLogic.IsGameOn())
            {
                playerTurnId = m_GameLogic.GetPlayerIdTurn();

                if (m_GameLogic.GetPlayer(playerTurnId).IsHuman)
                {
                    playHumanTurn(playerTurnId);
                }
                else
                {
                    playComputerTurn();
                }
               //TODO:: get guys dll for cleaning screen
                printCurrentGrid();
            }
            announceWinner();
        }

        private void announceWinner()
        {
            Player winner = m_GameLogic.GetWinner();
            if (winner != null)
            {
                Console.WriteLine(@"Congratulations {0}
You won the game!", winner.GetName());
            }
        }

        private void playHumanTurn(int i_playerId)
        {
            int[] firstPick = new int[2];
            int[] secondPick = new int[2];
            firstPick = getUserPick();
            m_GameLogic.UpdatePickVisibilit(firstPick[0], firstPick[1]);
            secondPick = getUserPick();
            bool isHit = m_GameLogic.CheckIsHit(firstPick[0], firstPick[1], secondPick[0], secondPick[1]);

            if (!isHit)
            {
                //TODO :: how do you want to show the cards for two seconds?
                printCurrentGrid(firstPick, secondPick);
                System.Threading.Thread.Sleep(SLEEP_TIME);
                Console.Clear(); // need to use guys dll
            }
        }

        private int[] getUserPick()
        {
            int rowIndex = 0;
            char colIndexInAlphBet;
            string userInput;
            do { Console.WriteLine("Type your row choice for the card between 1 and {0}: ", m_GameLogic.GetRowsLength());
                userInput = Console.ReadLine();
            }
            while(!int.TryParse(userInput, out rowIndex) && rowIndex > m_GameLogic.GetRowsLength());
            do
            {
                Console.WriteLine("Type your row choice for the card between 1 and {0}: ", m_GameLogic.GetColsLength());
                userInput = Console.ReadLine();
            }
            while (!char.TryParse(userInput, out colIndexInAlphBet) && (int)colIndexInAlphBet > m_GameLogic.GetColsLength());
            return new int[]{ rowIndex, (int)colIndexInAlphBet };
        }

        private void printCurrentGrid(int[] i_FirstCardIndexes = null, int[] i_SecondCardIndexes = null)
        {
            Cell[,] gameGrid = m_GameLogic.m_Grid;


            for (int i = 0; i < m_GameLogic.GetRowsLength(); i++)
            {
                for (int j = 0; j < m_GameLogic.GetColsLength(); j++)
                {
                    if ((gameGrid[i, j].IsVisable == !true) || (i_FirstCardIndexes != null && i_SecondCardIndexes != null) &&
                        (i == i_FirstCardIndexes[0] && j == i_FirstCardIndexes[1] || i == i_SecondCardIndexes[0] && j == i_SecondCardIndexes[1]))
                    {
                        Console.Write(@"| {0} |", gameGrid[i ,j]);
                    }
                    else
                    {
                        Console.Write(@"|   |");
                    }
                }
                Console.WriteLine();
            }
        }

        private void  playComputerTurn()
        {
            throw new NotImplementedException();
        }
        
        private void initializePlayers()
        {
            string inputString;
            string playerName;
            int userChoice;
            Console.WriteLine("Please Type your name:");
            playerName = Console.ReadLine();
            do
            {
                Console.WriteLine("Great!\nIn order to play againg the computer please type 0,to play against other player type 1");
                inputString = Console.ReadLine();
                int.TryParse(inputString, out userChoice);
            } while (userChoice != 0 && userChoice != 1);
            m_GameLogic.AddNewPlayer(new Player(0, playerName, true));
            if (userChoice == 0)
            {
                m_GameLogic.AddNewPlayer(new Player(1, "Computer", !true));
            }
            else
            {
                Console.WriteLine("Player 2, Please Type your name:");
                playerName = Console.ReadLine();
                m_GameLogic.AddNewPlayer(new Player(1, playerName, true));
            }
           
        }
#endregion
    }
}
