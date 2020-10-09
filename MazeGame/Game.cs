using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MazeGame
{
    public class Game
    {
        //primitives
        private string[] inputFromMap;
        private int linesInput, yAxis, xAxis, count = 0, level = 1;
        private bool smartMode = true;
        private int[] startPoint = new int[2];
        private int[] guardStartPoint = new int[2];
        private int[] finishPoint = new int[2];
        private bool[,] path;
        private int score;
        //objects
        private Cell[,] mapCells;
        private StreamReader sr;
        private Graphics g;
        private SmartGuard guard;
        private Player player;
        SoundPlayer music;
        public delegate void CollisionEventHandler(object source, Lives args);
        public event CollisionEventHandler Collision;
        private Lives lives = new Lives();


        public Game()
        {
            //load map data, play music
            this.readInMapData("..\\..\\map.txt");
            music = new SoundPlayer("..\\..\\resources\\mus5.wav");
            music.PlayLooping();

        }
        //properties 
        public int mapXLength { get { return inputFromMap.Length; } set { } }
        public int mapYLength { get { return linesInput; } set { } }
        public string[] Input { get { return inputFromMap; } set { inputFromMap = value; } }
        public int LinesInput { get { return linesInput; } }
        public int CurrentLevel { get { return level; } }
        public bool SmartMode { get { return smartMode; } }
        public Graphics G { get { return g; } set { g = value; } }
        public Player Player { get { return player; } }
        public SmartGuard Guard { get { return guard; } }
        public Cell[,] MapCells { get { return mapCells; } set { } }
        public Lives Lives { get { return lives; } set { } }
        public int Score { get { return score; } set { } }
        private void readInMapData(string path)
        {
            sr = new StreamReader(path);
            // count the number of lines of data
            linesInput = 0;
            string method = "";
            while (sr.EndOfStream == false)
            {
                method = sr.ReadLine();
                linesInput++;
            }
            yAxis = linesInput;
            //read in each line into a string array
            sr = new StreamReader(path);
            inputFromMap = new string[linesInput];
            for (int x = 0; x < linesInput; x++)
            {
                inputFromMap[x] = sr.ReadLine();
            }
            xAxis = inputFromMap[0].Length;
            Console.WriteLine("X axis = {0} and Y axis = {1}", xAxis, yAxis);
        }



        public void BuildLogicalMaze()
        {
            //use read in map data to build the maze in memory
            // y=28 x=49
            int yAxis = linesInput;
            int xAxis = inputFromMap[0].Length;


            mapCells = new Cell[yAxis, xAxis];
            char[] chars = new char[xAxis];

            for (int y = 0; y < yAxis; y++)
            {
                chars = inputFromMap[y].ToCharArray();
                for (int x = 0; x < xAxis; x++)
                {
                    mapCells[y, x] = new Cell(x, y, chars[x]);
                    Console.WriteLine("Cell " + x + "," + y + " created");
                    //record to mapType variable

                    //record player start point
                    if (chars[x] == 's')
                    {
                        startPoint[0] = x;
                        startPoint[1] = y;
                    }
                    //record guard start point
                    if (chars[x] == 'g')
                    {
                        guardStartPoint[0] = x;
                        guardStartPoint[1] = y;
                    }
                    //record finish point
                    if (chars[x] == 'f')
                    {
                        finishPoint[0] = x;
                        finishPoint[1] = y;
                    }

                }
            }


        }
        protected virtual void OnCollision()
        {
            Console.WriteLine("Collision");
            if (Collision != null)
            {
                Collision(this, lives);
            }
        }
        public void UpdateGame(int range, Timer timer)
        {
            //check distance
            Console.WriteLine("Distance between player and guard: " + this.Distance(Player.X, Player.Y, Guard.X, Guard.Y));
            Console.WriteLine("slider is at {0}", range);
            //should smart mode be activated??
            this.ChooseSmartMode(range);

            //smartmode on
            if (smartMode)
            {

                // find new path every 110 steps
                if (count % 110 == 0)
                {
                    //Create a previously searched array
                    bool[,] alreadySearched = new bool[yAxis, xAxis];
                    path = new bool[yAxis, xAxis];
                    //Starts the recursive map solver. If false maze can not be solved.
                    if (!solveMaze(Guard.X, Guard.Y, alreadySearched, path))
                        Console.WriteLine("Maze can not be solved.");

                }

                try
                {
                    //try moving on correct path
                    guard.CheckForCorrectPath(ref path);
                    guard.MoveGuard(guard.D);

                }
                catch
                {
                    // null reference catch
                    guard.MoveGuard();
                    count = -1;
                }


            }
            else
            {

                //move guard
                guard.MoveGuard();
            }
            // check for win condition
            if (Player.X == finishPoint[0] && Player.Y == finishPoint[1])
            {
                this.NextLevel(timer);
            }

            //check lives
            if (lives.HowManyLives == 0)
            {
                //gameOver 
                timer.Stop();
                MessageBox.Show("Game over");

            }
            // check player postion in relation to guard position 
            if (Player.X == guard.X && Player.Y == guard.Y)
            {
                this.OnCollision();
            }
            //increment update count
            count++;
        }


        private void NextLevel(Timer timer)
        {
            if (level == 1)
            {
                Console.WriteLine("You win");
                this.readInMapData("..\\..\\map2.txt");
                music.Stop();
                music = new SoundPlayer("..\\..\\resources\\mus1.wav");
                music.PlayLooping();
                
            }
            else if (level == 2)
            {
                Console.WriteLine("You win");
                this.readInMapData("..\\..\\map.txt");
                
            }
            else if (level == 3)
            {
                MessageBox.Show("You win!");
                timer.Stop();
            }

            this.BuildLogicalMaze();
            this.AddPlayer();
            this.AddGuard();
            this.OriginalRender();
            guard.MoveGuard();
            level++;
            score = score + 50;
        }

        public void AddGuard()
        {
            guard = new SmartGuard(guardStartPoint[0], guardStartPoint[1], this);
        }

        public void AddPlayer()
        {
            //actual
            player = new Player(startPoint[0], startPoint[1], this);
            //testing
            //player = new Player(41, 25, this);
        }
        public void OriginalRender()
        {
            // y=28 x=49

            for (int y = 0; y < linesInput; y++)
            {
                for (int x = 0; x < 49; x++)
                {
                    //draw map
                    mapCells[y, x].DrawBackground(g);
                }
            }
            //render player and guard
            player.Render();
            guard.Render();
        }
        public void Render()
        {

            //render player and guard
            player.Render();
            guard.Render();


        }
        private bool solveMaze(int xPos, int yPos, bool[,] alreadySearched, bool[,] path)
        {
            //g.FillRectangle(Brushes.Orange, xPos * 20, yPos * 20, 20, 20);
            bool correctPath = false;
            //should the computer check this tile
            bool shouldCheck = true;

            Console.WriteLine("x position {0} y position {1}", xPos, yPos);
            //Check for out of boundaries
            if (xPos > xAxis || yPos > yAxis)
            {
                shouldCheck = false;
                Console.WriteLine("Not checking");
            }
            else
            {
                //Check if at finish, not (0,0 and colored light blue)
                if (Player.X == xPos && Player.Y == yPos && (xPos != 0 && yPos != 0))
                {
                    correctPath = true;
                    shouldCheck = false;
                }

                //Check for a wall
                Console.WriteLine("checking for wall");
                if (mapCells[yPos, xPos].CellType == 'w')
                    shouldCheck = false;

                //Check if previously searched
                if (alreadySearched[yPos, xPos])
                    shouldCheck = false;
            }

            //Search the Tile
            if (shouldCheck)
            {
                Console.WriteLine("searching");
                //mark tile as searched
                alreadySearched[yPos, xPos] = true;



                //check up tile
                if (mapCells[yPos - 1, xPos].CellType != 'w')
                    correctPath = correctPath || solveMaze(xPos, yPos - 1, alreadySearched, path);
                //Check down tile
                if (mapCells[yPos + 1, xPos].CellType != 'w')
                    correctPath = correctPath || solveMaze(xPos, yPos + 1, alreadySearched, path);
                //check left tile
                if (mapCells[yPos, xPos - 1].CellType != 'w')
                    correctPath = correctPath || solveMaze(xPos - 1, yPos, alreadySearched, path);
                //Check right tile
                if (mapCells[yPos, xPos + 1].CellType != 'w')
                    correctPath = correctPath || solveMaze(xPos + 1, yPos, alreadySearched, path);


            }

            //make correct path purple
            if (correctPath)
            {
                path[yPos, xPos] = true;
                g.FillRectangle(Brushes.Purple, xPos * 20, yPos * 20, 20, 20);
            }
            return correctPath;
        }
        private void ChooseSmartMode(int range)
        {   //how foar is player from guard??
            int playerDistance = Distance(player.X, player.Y, guard.X, guard.Y);

            //off
            if (range == 0)
            {
                smartMode = false;
                count = 0;
            }
            //slider position 1
            if (range == 1 && playerDistance < 10)
            {
                smartMode = true;
                Console.WriteLine("smartmode activated!");
            }
            else if (range == 1)
            {
                smartMode = false;
            }
            // slider position 2
            if (range == 2 && playerDistance < 20)
            {
                smartMode = true;
            }
            else if (range == 2)
            {
                smartMode = false;
            }
            // slider position 3
            if (range == 3 && playerDistance < 30)
            {
                smartMode = true;
            }
            else if (range == 3)
            {
                smartMode = false;
            }
            // slider position 4
            if (range == 4 && playerDistance < 40)
            {
                smartMode = true;
            }
            else if (range == 4)
            {
                smartMode = false;
            }
            // SMARTMODE ACTIVATE!!?!?
            if (range == 5)
            {
                smartMode = true;
            }




        }

        public int Distance(int px, int py, int gx, int gy)
        {
            int row = px - gx;
            int col = py - gy;
            double working = Math.Round(Math.Sqrt(Math.Pow(row, 2) + Math.Pow(col, 2)));
            int ans = (int)working;
            return ans;
        }
    }

}
