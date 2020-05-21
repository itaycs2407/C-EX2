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

        private int m_Rows;
        private int m_Cols;
        private Logic m_GameLogic;
        private Cell[,] m_Grid;

        public Cli()
        {
            m_GameLogic = new Logic();
            Console.WriteLine(@"Hello and welcome to our mmory cards game!");
            m_GameLogic.AddNewPlayer(createPlayer(!true));
            switch (GetPlayersCount())
            {
                case 1:
                    {
                        m_GameLogic.AddNewPlayer(createPlayer(!true));
                        break;
                    }
                default:
                    {
                        m_GameLogic.AddNewPlayer(createPlayer(true));
                        break;
                    }
            }
        }

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
            while (m_GameLogic.IsValidGrid(i_NumInput));
            return i_NumInput;
        }
        
        public void InitPlayers(int humanPlayersCount, int computerPlayersCount)
        {
            for(int i = 0; i< humanPlayersCount; i++)
            {
                m_GameLogic.AddNewPlayer(createPlayer(true));
            }
            for(int i =0; i < computerPlayersCount; i++)
            {
                m_GameLogic.AddNewPlayer(createPlayer(!true));
            }
        }

        private Player createPlayer(bool v)
        {
            Console.WriteLine("Hi Player, please type your name");
            return Player;
        }

        public int GetPlayersCount()
        {
            string i_StrInput;
            int playersCountFromInput;

            do
            {
                Console.WriteLine(String.Format(@"Enter number of players to play this game,
In order to play against the computer pls type 1."));
                i_StrInput = Console.ReadLine();
                int.TryParse(i_StrInput, out playersCountFromInput);
            }
            while (m_GameLogic.IsValidPlayersCount(playersCountFromInput));

            return playersCountFromInput;
        }
        public void Start()
        {
            playGame();
        }

        private void playGame() {
            int playerTurnId = 0;
            while (m_GameLogic.IsGameOn())
            {
                playerTurnId = m_GameLogic.GetIdPlayerTurn();

                if (m_GameLogic.GetPlayer(playerTurnId).IsHuman) {
                    playHumanTurn(playerTurnId);
                }
                else {
                    playComputerTurn();
                }
                Console.Clear(); // need to use guys dll
                printCurrentGrid();
                m_GameLogic.FlipTurn(playerTurnId);
            }
        }

        private void playHumanTurn(int i_playerId)
        {
            List<int> playersCardsPicks = new List<int>(m_GameLogic.GetSameCardsCount());

            playersCardsPicks[0] = (int)getPlayerPick(m_GameLogic.GetPlayer(i_playerId).Name,
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
            return players;
        }
            
        private int getPlayersCount()
        {
            string i_StrInput;
            int i_NumInput;
            do
            {
                Console.WriteLine(String.Format(@"Enter number of players to play this game,\n In order to play against the computer pls type 1."));
                i_StrInput = Console.ReadLine();
                int.TryParse(i_StrInput, out i_NumInput);
            }
            while ((i_NumInput > 6 || i_NumInput < 4) && (i_NumInput % 2 != 0));

            return i_NumInput;
        }

        
        public void GetGrid()
        {
            m_Rows = getDimension("rows");
            m_Cols = getDimension("columns");
            m_Grid = new Cell[m_Rows, m_Cols];
        }

        private int getDimension(string i_DimensionNameForUserInput)
        {
            string i_StrInput;
            int i_NumInput;
            do
            {
                Console.WriteLine(String.Format(@"Enter number of {0} , between 4 and 6 (include) :", i_DimensionNameForUserInput));
                i_StrInput = Console.ReadLine();
                int.TryParse(i_StrInput, out i_NumInput);
            }
            while (i_NumInput > 6 || i_NumInput < 4);

            return i_NumInput;
        }

        public void GetInputFromUser()
        {
            throw new NotImplementedException();
        }

        public void getPlayersDetails()
        {
            throw new NotImplementedException();
        }

    }
}
