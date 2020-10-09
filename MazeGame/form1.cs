using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MazeGame
{
    public enum Direction
    {
        UP, DOWN, LEFT, RIGHT
    }
    public partial class form1 : Form
    {

        private int time = 1, range = 0, count = 0;
        private bool running = false;
        private Graphics g;
        private Game gameLogic;




        public form1()
        {
            // game class + read in level maps

            gameLogic = new Game();
            //temp get variables
            string[] input;
            int linesInput;
            input = gameLogic.Input;
            linesInput = gameLogic.LinesInput;
            gameLogic.Collision += GameCollision;



            InitializeComponent();
            //create graphics
            g = Maze.CreateGraphics();
            //set grahpics in game logic
            gameLogic.G = g;

        }

        private void GameCollision(object source, Lives args)
        {
            Lives.Text = Convert.ToString(args.HowManyLives);

            if (args.HowManyLives == 0)
            {
                timer2.Stop();
                Console.WriteLine("Game over");
            }
        }

        private void start_Click(object sender, EventArgs e)
        {
            running = true;
            start.Enabled = false;
            //start timer and initial logic / render

            timer1.Start();
            gameLogic.BuildLogicalMaze();
            gameLogic.AddPlayer();
            gameLogic.AddGuard();

            gameLogic.OriginalRender();
            //start game loop
            timer2.Start();


        }



        private void timer1_Tick(object sender, EventArgs e)
        {
            //timer in seconds
            time++;
            timerOut.Text = Convert.ToString(time);
        }

        private void timer2_Tick(object sender, EventArgs e)
        {

            //update game logic

            gameLogic.UpdateGame(range,timer2);

            Lives.Text = Convert.ToString(gameLogic.Lives.HowManyLives);

            scoreBox.Text = Convert.ToString(gameLogic.Score);
            //render game
            gameLogic.Render();

            //testing
            if (count == 5)
            {
                timer2.Stop();
                count = 0;
            }
        }


        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Manual manual = new Manual();
            manual.Show();
        }

        private void aboutToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            MessageBox.Show("A game by Gordon B00720507 and Pearse B00693290");


        }

        private void Maze_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                gameLogic.OriginalRender();
            }
            catch
            {
               
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {

                if (running)
                {
                    Direction d;
                    if (e.KeyCode == Keys.W)
                    {
                        //move player up
                        d = Direction.UP;
                        gameLogic.Player.MovePlayer(d);

                    }
                    else if (e.KeyCode == Keys.S)
                    {
                        //move player down
                        d = Direction.DOWN;
                        gameLogic.Player.MovePlayer(d);
                    }
                    else if (e.KeyCode == Keys.A)
                    {
                        //move player left
                        d = Direction.LEFT;
                        gameLogic.Player.MovePlayer(d);
                    }
                    else if (e.KeyCode == Keys.D)
                    {
                        //move player right
                        d = Direction.RIGHT;
                        gameLogic.Player.MovePlayer(d);
                    }

                }

                if (e.KeyCode == Keys.P)
                {
                    if (running)
                    {
                        //pause
                        timer1.Stop();
                        timer2.Stop();
                        running = false;

                    }
                    else
                    {
                        running = true;
                        timer1.Start();
                        timer2.Start();
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Bad things are happening!");

            }


        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            range = trackBar1.Value;
            Console.WriteLine("range is set at {0}", range);
        }
    }

}
