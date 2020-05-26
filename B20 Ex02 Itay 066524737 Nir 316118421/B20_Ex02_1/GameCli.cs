using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B20_Ex02_1
{
    public class GameCli 
    {
#region props
        private const int SLEEP_TIME = 2000;
        private Logic m_GameLogic;
        #endregion
#region c'tor
        public GameCli()
        {
            m_GameLogic = new Logic();
        }

        #endregion
#region Public Methods
        public void InitializeGame()
        {
            m_GameLogic = new Logic();
            initializePlayers();
            initializeGrid();
        }

        public void Start()
        {
            //CR ::Guys dll
            Ex02.ConsoleUtils.Screen.Clear();
            System.Console.Clear();
            Console.WriteLine("Enjoy the match :)");
            playGames();
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
                Console.WriteLine(string.Format(@"Enter number of {0} , between 4 and 6 (include) :", i_DimensionNameForUserInput));
                dimensionInput = Console.ReadLine();
            }
            while(!int.TryParse(dimensionInput, out dimension));
            
            return dimension;
        }
       
        private void playGames()
        {
            string rematchUserDesicion = "1";
            playGame();
            Console.WriteLine(string.Format(@"Well, thats it.. or you can press 1 if {0} wants to win a rematch! (else press anything else..)", m_GameLogic.GetLoser().Name));
            rematchUserDesicion = Console.ReadLine();

            while (rematchUserDesicion.Equals("1"))
            {
                InitializeGame();
                playGame();
                Console.WriteLine(string.Format(@"Well, thats it.. or you can press 1 if {0} wants to win a rematch! (else press anything else..)", m_GameLogic.GetLoser().Name));
                rematchUserDesicion = Console.ReadLine();
            }
        }

        private void playGame() 
        {
            Player currentPlayingPlayer;
            while (m_GameLogic.IsGameOn())
            {
                printCurrentGrid();
                currentPlayingPlayer = m_GameLogic.GetActivePlayer();
                if (currentPlayingPlayer.IsHuman)
                {
                    playHumanTurn(currentPlayingPlayer.Id);
                }
                else
                {
                    playComputerTurn();
                }

                Ex02.ConsoleUtils.Screen.Clear();
            }

            announceWinner();
        }

        private void announceWinner()
        {
            //TODO:: add getloser
            Player winner = m_GameLogic.GetWinner();
            if (winner != null)
            {
                Console.WriteLine(string.Format(@"Congratulations {0} You won the game!", winner.Name));
            }
        }

        private void playHumanTurn(int i_playerId)
        {
            int[] firstPick = new int[2];
            int[] secondPick = new int[2];
            Player activePlayer = m_GameLogic.GetActivePlayer();
            Console.WriteLine(string.Format(string.Format(@"{0}, you are up again! Just in case you forgot - so far you have {1} points", activePlayer.Name, activePlayer.NumOfHits)));
            firstPick = handlePick();
            if (!m_GameLogic.IsGameOver)
            {
                printCurrentGrid();
                secondPick = handlePick();
                if (!m_GameLogic.IsGameOver)
                {
                    while (secondPick[0] == firstPick[0] && secondPick[1] == firstPick[1])
                    {
                        Console.WriteLine("You are not supposed to pick the same card twice! Please do it again!");
                        firstPick = handlePick();
                        printCurrentGrid();
                        secondPick = handlePick();
                    }

                    bool v_IsHit = m_GameLogic.TryUpdateForEquality(firstPick[0], firstPick[1], secondPick[0], secondPick[1]);
                    if (!v_IsHit)
                    {
                        printCurrentGrid(firstPick, secondPick);
                        System.Threading.Thread.Sleep(SLEEP_TIME);
                    }
                }
            }
        }

        private int[] handlePick()
        {
            int[] res = getUserPick();
            while ((res[0] != -1 && res[1] != -1) && !m_GameLogic.TryFlipCard(res[0], res[1]))
            {
                Console.WriteLine(string.Format(@"Sorry, you picked a wrong card placement, please choose again"));
                res = getUserPick();
            }

            return res;
        }
        
        private int[] getUserPick()
        {
            int rowIndex = 0;
            char colIndexInAlphBet = ' ';
            string userInput;
            int[] userPicks = new int[2];
            bool v_IsQuit = !true;

            userInput = getInputFrommUser(new StringBuilder().AppendFormat("Type your row choice for the card between 1 and {0}:", m_GameLogic.GetGridRows()).ToString());
            v_IsQuit = m_GameLogic.TryQuitGame(userInput);

            while ((!v_IsQuit) && (!int.TryParse(userInput, out rowIndex) || rowIndex > m_GameLogic.GetGridRows()))
            {
                userInput = getInputFrommUser(new StringBuilder().AppendFormat("Invalid input, Please Type your row choice for the card between 1 and {0}: ", m_GameLogic.GetGridRows()).ToString());
                v_IsQuit = m_GameLogic.TryQuitGame(userInput);
            }

            userPicks[0] = v_IsQuit ? -1 : rowIndex - 1;
            if (!v_IsQuit)
            {
                userInput = getInputFrommUser(new StringBuilder().AppendFormat("Type your column choice for the card between A and {0}:", (char)(m_GameLogic.GetGridCols() + 'A' - 1)).ToString());
                v_IsQuit = m_GameLogic.TryQuitGame(userInput);

                while ((!v_IsQuit && !char.TryParse(userInput.ToUpper(), out colIndexInAlphBet)) || ((int)(colIndexInAlphBet - 'A') > m_GameLogic.GetGridCols()))
                {
                    userInput = getInputFrommUser(new StringBuilder().AppendFormat("Invalid input, Please Type your column choice for the card between A and {0}: ", (char)(m_GameLogic.GetGridCols() + 65)).ToString());
                    v_IsQuit = m_GameLogic.TryQuitGame(userInput);
                }
            }

            userPicks[1] = v_IsQuit ? -1 : (int)(colIndexInAlphBet - 'A');

            return userPicks;
        }

        private string getInputFrommUser(string i_messageToShowUser)
        {
            Console.WriteLine(i_messageToShowUser);
            string userInput = Console.ReadLine();
            return userInput;
        }

        private void printCurrentGrid(int[] i_FirstCardIndexes = null, int[] i_SecondCardIndexes = null)
        {
            Cell[,] gameGrid = m_GameLogic.GameGrid;
            Console.Write("   ");
            for (int i = 0; i < m_GameLogic.GetGridCols(); i++)
            {
                Console.Write("{0}    ", (char)(i + 'A'));
            }

            printLnSeperator();
            for (int i = 0; i < m_GameLogic.GetGridRows(); i++)
            {
                Console.Write(string.Format(@"{0} |", i + 1));
                for (int j = 0; j < m_GameLogic.GetGridCols(); j++)
                {
                    if ((gameGrid[i, j].IsVisable == true) || (i_FirstCardIndexes != null && i_SecondCardIndexes != null) &&
                        ((i == i_FirstCardIndexes[0]) && (j == i_FirstCardIndexes[1]) || (i == i_SecondCardIndexes[0]) && (j == i_SecondCardIndexes[1])))
                    {
                        Console.Write(string.Format(@" {0} |", gameGrid[i, j].Letter));
                    }
                    else
                    {
                        Console.Write(string.Format(@"   |"));
                    }
                }

                printLnSeperator();      
            }
        }

        private void printLnSeperator()
        {
            Console.Write("\n   ");
            for (int j = 0; j < m_GameLogic.GetGridCols(); j++)
            {
                Console.Write("====");
            }

            Console.WriteLine();
        }

        //need to go out ?!?!?
        private void playComputerTurn()
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
                Console.WriteLine("Great!\nIn order to play against another player - press 1 , in case you prefer to lay against comuter - press any other key");
                inputString = Console.ReadLine();
                int.TryParse(inputString, out userChoice);
            } 
            while (userChoice != 0 && userChoice != 1);

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
