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

        private Logic m_GameLogic;

        public Cli()
        {
            m_GameLogic = new Logic();
            
        }
        // GET ONLY THE INPUT FROM USER, LOGIX CHECKS NUMBER AND SUM OF ALL GRID CELL
        public int GetDimension(string i_DimensionNameForUserInput)
        {
            string i_StrInput;
            int i_NumInput;
            do
            {
                Console.WriteLine(String.Format(@"Enter number of {0} , between 4 and 6 (include) :", i_DimensionNameForUserInput));
                i_StrInput = Console.ReadLine();
                int.TryParse(i_StrInput, out i_NumInput);
            }
            while (m_GameLogic.TryCreateGrid(i_NumInput));
            return i_NumInput;
        }
       
        public void Start()
        {
            playGame();
        }

        private void playGame() {
            int playerTurnId = 0;

            while (m_GameLogic.IsGameOn())
            {
                playerTurnId = m_GameLogic.GetPlayerIdTurn();

                if (m_GameLogic.GetPlayerType(playerTurnId).IsHuman) {
                    playHumanTurn(playerTurnId);
                }
                else {
                    playComputerTurn();
                }
                Console.Clear(); // need to use guys dll
                printCurrentGrid();
            }
        }

        private void playHumanTurn(int i_playerId)
        {
            List<int> playersCardsPicks = new List<int>(m_GameLogic.GetSameCardsCount());

            playersCardsPicks[0] = (int)getPlayerPick(m_GameLogic.GetPlayerType(i_playerId).Name,
                "row", (char)1, (char)m_GameLogic.GetRowsLength());
            //playersCardsPicks[1] = getPlayerPick(m_GameManager.GetPlayer()i_playerId).GetName(),
            //    "column", 'A', (char)('A' + m_GameManager.GetColsLength());

            bool isHit = m_GameLogic.CheckIsHit(playersCardsPicks);

            if (!isHit)
            {
                printCurrentGrid(playersCardsPicks);
                System.Threading.Thread.Sleep(2000); // nedd to check if this the way guy asked
                Console.Clear(); // need to use guys dll
            }
        }

        private void printCurrentGrid(List<int> playersCardsPicks = null)
        {
            // need to get the matrix  instance
            Cell[,] gameGrid = m_GameLogic.m_Grid;


            for (int i = 0; i < m_GameLogic.GetRowsLength(); i++)
            {
                for (int j = 0; j < m_GameLogic.GetColsLength(); j++)
                {
                    if(playersCardsPicks!= null && i == playersCardsPicks[0] && j == playersCardsPicks[j])
                    {
                        Console.Write(@" {0} ", gameGrid[i ,j]);
                    }
                    else
                    {
                        Console.Write(@" {0} ", gameGrid[i, j]);
                    }
                }
                Console.WriteLine();
            }
        }

        private void  playComputerTurn()
        {
            throw new NotImplementedException();
        }

        private char getPlayerPick(string i_PlayerName,string i_dimensionString, char i_pickHighLimitToPrint, char i_pickLowLimitToPrint)
        {
            string usersInput;
            char inputChoice;
            do
            {
                Console.WriteLine(@"Player {0}, Please type your wanted {1} num from {2} to {3}",
                 i_PlayerName,i_dimensionString, i_pickLowLimitToPrint, i_pickHighLimitToPrint);
                usersInput = Console.ReadLine();
                char.TryParse(usersInput, out inputChoice);
            } while (inputChoice < i_pickLowLimitToPrint || inputChoice > i_pickHighLimitToPrint);
            return inputChoice;
        }
    
     

        private List<Player> InitPlayersProps()
        {
            string inputString;
            string playerName;
            int userChoice;
            List<Player> players = new List<Player>();
            Console.WriteLine("Please Type your name:");
            playerName = Console.ReadLine();
            do
            {
                Console.WriteLine("Great!\nIn order to play againg the computer please type 0,to play against other player type 1");
                inputString = Console.ReadLine();
                int.TryParse(inputString, out userChoice);
            } while (userChoice != 0 && userChoice != 1);
            players.Add(new Player(0, playerName, true));
            if (userChoice == 0)
            {
                players.Add(new Player(1, "Computer", !true));
            }
            else
            {
                Console.WriteLine("Player 2, Please Type your name:");
                playerName = Console.ReadLine();
                players.Add(new Player(1, playerName, true));
            }

            /// pass to logic list with the players
            return players;
        }
        

    }
}
