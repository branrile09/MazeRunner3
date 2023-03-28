
using System.ComponentModel;
using System.Diagnostics;

namespace MazeRunner3
{
    internal class Program
    {
        public static string[,] maze = defaultMaze();
        public static int[,] charPositions = defaultPositions();

        public static int player0Score = 0;
        public static int player1Score = 0;

        public static bool InGame = false;


        static void Main(string[] args)
        {
            //declare and start a new thread that will manage the AI characters
            Multiplayer();
            //SinglePlayer();

        }

        // we need to fix score point
      

        //we need to get multiplayer to work
        static void Multiplayer()
        {
            InGame = true;
            Thread AI_Manager_Thread = new Thread(AI_Manager); 
            AI_Manager_Thread.Start();

            Thread GameRender = new Thread(() => DisplayMazeThread(120));
            GameRender.Start();

            Thread Player1 = new Thread(MultiPlayerManager);
            Player1.Start();

            do
            {
                if (ScorePoint(0))
                {
                    player0Score++;                
                }
                if (ScorePoint(1))
                {
                    player1Score++;                
                }
            } while (player0Score != 50 && player1Score != 50);

            InGame = false;
            AI_Manager_Thread.Join();
            GameRender.Join();
            Player1.Join();



        }

        //create a thread to display the maze
        static void DisplayMazeThread(int fps = 60)
        {
            int delay = 1000 / fps;

            do
            {
                DisplayMaze();
                Thread.Sleep(delay);
            } while (InGame);
            Thread.Sleep(150);
         
            
        }
        static bool ScorePoint(int playerID)
        {
            int playerX = charPositions[playerID,0];
            int playerY = charPositions[playerID,1];
            //collision check
            for (int i = 2; i < charPositions.GetLength(0); i++)
            {
                if (charPositions[i, 0] == playerX && charPositions[i, 1] == playerY)
                {
                    charPositions[i, 0] = 9;
                    charPositions[i, 1] = 9;
                    return true;
                }           
            }

            return false;
        }

        static int[,] defaultPositions()
        {

            //charPositions[charID, X/Y]; 
            int[,] charPositions = {
            {1,1}, // player  (charPositions[0,0] & charPositions[0,1])
            {8,9}, // AI1 (charPositions[1,0] & charPositions[1,1])
            {9,9}, // AI2 (charPositions[2,0] & charPositions[2,1])
            {10,9}, // AI3  (charPositions[3,0] & charPositions[3,1])          
            {10,2}, // AI3  (charPositions[3,0] & charPositions[3,1])          
            };


            return charPositions;
        }

       

        static void DisplayMaze()
        {

            //we are re-writing over entire screen to reduce flicker
            //since we are doing this, we can only have 1 instance of writing
            //this is why we need mutex
            Console.SetCursorPosition(0, 0);
            //displays score
            Console.WriteLine("Score:" + player0Score + "   Score:" + player1Score);

            //display the maze with the players and AI current position
            for (int row = 0; row < maze.GetLength(0); row++)
            {
                for (int col = 0; col < maze.GetLength(1); col++)
                {
                    if (row == charPositions[0, 1] && col == charPositions[0, 0])
                    {
                        Console.Write("1");
                    }

                    else if (row == charPositions[1, 1] && col == charPositions[1, 0])
                    {
                        Console.Write("2");
                    }
                    else if (row == charPositions[2, 1] && col == charPositions[2, 0])
                    {
                        Console.Write("@");
                    }
                    else if (row == charPositions[3, 1] && col == charPositions[3, 0])
                    {
                        Console.Write("@");
                    }
                    else if (row == charPositions[4, 1] && col == charPositions[4, 0])
                    {
                        Console.Write("@");
                    }

                    else
                    {
                        Console.Write(maze[row, col]);
                    }

                }

                Console.WriteLine();
            }
            Console.Write("use arrows or WASD to move");
        }


        static void MultiPlayerManager()
        {
            do
            {
                int move = KeyPressedPlayer2();
                int player = 0;
                if (move > 4)
                {
                    player++;
                    move -= 4;
                }

                //where we are moving towards

                int newCol = charPositions[player, 0]; //X
                int newRow = charPositions[player, 1]; //Y

                //basic movement checks
                if (move == 1)
                {
                    newRow--;
                }
                else if (move == 2)
                {
                    newRow++;
                }
                else if (move == 3)
                {
                    newCol--;
                }
                else if (move == 4)
                {
                    newCol++;
                }
                if (newCol < 0)
                {
                    newCol = 18;
                }
                else if (newCol > 18)
                {
                    newCol = 0;
                }
                //check for clear path in maze
                if (maze[newRow, newCol] == " ")
                {//updating player postioning
                 //                    
                    charPositions[player, 0] = newCol;
                    charPositions[player, 1] = newRow;

                }

            } while (InGame);

        }


        static int KeyPressedPlayer2()
        {
            do
            {
                //cachine the variable
                ConsoleKey keypressed = Console.ReadKey(true).Key;
                //compare what the keystroke is
                if (keypressed == ConsoleKey.UpArrow)
                {
                    return 1;
                }
                else if (keypressed == ConsoleKey.DownArrow)
                {
                    return 2;
                }
                else if (keypressed == ConsoleKey.LeftArrow)
                {
                    return 3;
                }
                else if (keypressed == ConsoleKey.RightArrow)
                {
                    return 4;
                }
                else if (keypressed == ConsoleKey.W)
                {
                    return 5;
                }
                else if (keypressed == ConsoleKey.S)
                {
                    return 6;
                }
                else if (keypressed == ConsoleKey.A)
                {
                    return 7;
                }
                else if (keypressed == ConsoleKey.D)
                {
                    return 8;
                }
                else
                { return 0; }

            }
            while (false);
        }


        static string[,] defaultMaze()
        {

            string[,] maze = {
                { "#", "#", "#", "#", "#", "#", "#", "#", "#", "#","#", "#", "#", "#", "#", "#", "#", "#", "#" },//
                { "#", " ", " ", " ", " ", " ", " ", " ", " ", "#"," ", " ", " ", " ", " ", " ", " ", " ", "#" },//
                { "#", " ", "#", "#", " ", "#", "#", "#", " ", "#"," ", "#", "#", "#", " ", "#", "#", " ", "#" },//
                { "#", " ", " ", " ", " ", " ", " ", " ", " ", " "," ", " ", " ", " ", " ", " ", " ", " ", "#" },//
                { "#", " ", "#", "#", " ", "#", " ", "#", "#", "#","#", "#", " ", "#", " ", "#", "#", " ", "#" },//
                { "#", " ", " ", " ", " ", "#", " ", " ", " ", "#"," ", " ", " ", "#", " ", " ", " ", " ", "#" },//
                { "#", "#", "#", "#", " ", "#", "#", "#", " ", "#"," ", "#", "#", "#", " ", "#", "#", "#", "#" },//
                { "#", "#", "#", "#", " ", "#", " ", " ", " ", " "," ", " ", " ", "#", " ", "#", "#", "#", "#" },//
                { "#", "#", "#", "#", " ", "#", " ", "#", "#", " ","#", "#", " ", "#", " ", "#", "#", "#", "#" },//
                { " ", " ", " ", " ", " ", " ", " ", "#", " ", " "," ", "#", " ", " ", " ", " ", " ", " ", " " },//
                { "#", "#", "#", "#", " ", "#", " ", "#", "#", "#","#", "#", " ", "#", " ", "#", "#", "#", "#" },//
                { "#", "#", "#", "#", " ", "#", " ", " ", " ", " "," ", " ", " ", "#", " ", "#", "#", "#", "#" },//
                { "#", "#", "#", "#", " ", "#", "#", "#", " ", "#"," ", "#", "#", "#", " ", "#", "#", "#", "#" },//
                { "#", " ", " ", " ", " ", "#", " ", " ", " ", "#"," ", " ", " ", "#", " ", " ", " ", " ", "#" },//
                { "#", " ", "#", "#", " ", "#", " ", "#", "#", "#","#", "#", " ", "#", " ", "#", "#", " ", "#" },//
                { "#", " ", " ", " ", " ", " ", " ", " ", " ", " "," ", " ", " ", " ", " ", " ", " ", " ", "#" },//
                { "#", " ", "#", "#", " ", "#", "#", "#", " ", "#"," ", "#", "#", "#", " ", "#", "#", " ", "#" },//
                { "#", " ", " ", " ", " ", " ", " ", " ", " ", "#"," ", " ", " ", " ", " ", " ", " ", " ", "#" },//                
                { "#", "#", "#", "#", "#", "#", "#", "#", "#", "#","#", "#", "#", "#", "#", "#", "#", "#", "#" },//     
            };
            return maze;

        }

        //task designed to run in own thread
        static void AI_Manager()
        {
            //initialize current direction
            int[] AI_Direction = { 0, 0, 0 };

            do
            {//generate ai movement,
                AI_Manager(AI_Direction);
            } while (InGame);
            Thread.Sleep(100);

        }

        static int[] AI_Manager(int[] direction)
        {
            //loops through each AI character, and generates movement
            for (int i = 2; i < charPositions.GetLength(0); i++)
            {
                //call AI movement function, receive movement info that has been validated
                int newDirection = AI_Movement(direction[i - 2], i);
                direction[i - 2] = newDirection;

                //where we are moving towards

                int newCol = charPositions[i, 0]; //X
                int newRow = charPositions[i, 1]; //Y

                //similar code for user navigation
                if (newDirection == 1)
                {
                    newRow--;
                }
                else if (newDirection == 2)
                {
                    newRow++;
                }
                else if (newDirection == 3)
                {
                    newCol--;
                }
                else if (newDirection == 4)
                {
                    newCol++;
                }

                if (newCol < 0)
                {
                    newCol = 18;
                }
                else if (newCol > 18)
                {
                    newCol = 0;
                }

                //update AI position
                charPositions[i, 0] = newCol;
                charPositions[i, 1] = newRow;
                Thread.Sleep(70);
            }
            //returns new directions to keep track of ai movement to create next move bias
            return direction;

        }

        static int AI_Movement(int lastDirection, int charID)
        {
            int randomNumber;
            int Attempts = 0;
            Random r = new Random();

            //we cache this as we dont want to be constantly fetching from array,
            //and we dont want to modify the array yet
            int tempX = charPositions[charID, 0];
            int tempY = charPositions[charID, 1];
            do
            {
                //generate random number
                randomNumber = r.Next(1, 5);
                // grabs new positions
                int newCol = tempX;
                int newRow = tempY;
                bool oppositeDirection = false; //for checking directional 

                //----------up
                if (randomNumber == 1)
                {
                    newRow--;
                    if (lastDirection == 2)
                    {
                        oppositeDirection = true;
                    }
                }
                //------------down
                else if (randomNumber == 2)
                {
                    newRow++;
                    if (lastDirection == 1)
                    {
                        oppositeDirection = true;
                    }
                }
                //--------------------left
                else if (randomNumber == 3)
                {
                    newCol--;
                    if (lastDirection == 4)
                    {
                        oppositeDirection = true;
                    }
                }
                //-------------right
                else if (randomNumber == 4)
                {
                    newCol++;
                    if (lastDirection == 3)
                    {
                        oppositeDirection = true;
                    }
                }

                if (newCol < 0)
                {
                    newCol = 18;
                }
                else if (newCol > 18)
                {
                    newCol = 0;
                }


                //check if tthe move is Valid
                bool validMove = MovementIsVaild(charID, newCol, newRow);

                bool moveInSameDirection = validMove && randomNumber == lastDirection; //move forward
                bool movePerpendicular = validMove && !oppositeDirection; //moves that are perpendicular
                bool forcedRotation = validMove && oppositeDirection && Attempts > 14; //forced rotation needs to take lower precedence over the other 2 conditions
                bool Stuck = !validMove && Attempts > 28; //we need to try a fair effort before calling it quits


                if (moveInSameDirection || movePerpendicular || forcedRotation)
                {
                    return randomNumber;
                }
                else if (Stuck)
                {
                    return 0; //wait for other player to move out of the way
                }
                Attempts++;

            } while (true);


        }

        static bool MovementIsVaild(int charID, int x, int y)
        {
            //check if path is clear on maze
            if (maze[y, x] != " ")
            {
                return false;
            }

            //check for coliding with other players or AI
            for (int i = 0; i < charPositions.GetLength(0); i++)
            {
                if (i == charID)
                {//we arent comparing against itself
                    continue;
                }
                if (charPositions[i, 0] == x && charPositions[i, 1] == y)
                {//if we see a collision, we return false
                    return false;
                }

            }
            //if  no collisions detected, we return true
            return true;

        }


    


    }
}