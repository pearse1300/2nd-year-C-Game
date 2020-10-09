using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeGame
{
   public class Guard: Character
    {
        protected int startX, startY,frame=0;
        protected Image[] sprite;
        protected Graphics g;
        protected Game game;
        protected bool upPath, downPath, leftPath, rightPath;
        protected Direction d;
   


        public Guard(int x, int y, Game game)
        {
            this.x = x;
            this.y = y;
            startX = x;
            startY = y;
            this.game = game;
            mapCells = game.MapCells;
            g = game.G;
            d = Direction.UP;
            game.Collision += Game_Collision;
            sprite = new Image[5];
            sprite[0] = Image.FromFile("..\\..\\resources\\guard.png");
            sprite[1] = Image.FromFile("..\\..\\resources\\guardELeft.png");
            sprite[2] = Image.FromFile("..\\..\\resources\\guardELeftF.png");
            sprite[3] = Image.FromFile("..\\..\\resources\\guardERight.png");
            sprite[4] = Image.FromFile("..\\..\\resources\\guardERightF.png");

        }

        protected void Game_Collision(object source, Lives args)
        {
            //reset guard position
            x = startX;
            y = startY;
        }

        public int X { get { return x; }set { x = value; } }
        public int Y { get { return y;} set { y = value; } }
        public Direction D{ get { return d; } }

        public override void Render()
        {
            //draw guard
            //g.FillRectangle(Brushes.Red, x * 20, y * 20, 20, 20);
            if(frame == 0)
            {
                g.DrawImage(sprite[0], x * 20, y * 20);
            }
            else if (0<frame && frame <= 3)
            {
                g.DrawImage(sprite[1], x * 20, y * 20);
            }
            else if (3 < frame && frame <= 6)
            {
                g.DrawImage(sprite[2], x * 20, y * 20);
            }
            else if (6 < frame && frame <= 9)
            {
                g.DrawImage(sprite[1], x * 20, y * 20);
            }
            else if (9 < frame && frame <= 12)
            {
                g.DrawImage(sprite[0], x * 20, y * 20);
            }
            else if (12 < frame && frame <= 15)
            {
                g.DrawImage(sprite[3], x * 20, y * 20);
            }
            else if (15 < frame && frame <= 18)
            {
                g.DrawImage(sprite[4], x * 20, y * 20);
            }
            else if (18 < frame && frame <= 21)
            {
                g.DrawImage(sprite[3], x * 20, y * 20);
            }
            else if (21 < frame && frame <= 24)
            {
                g.DrawImage(sprite[0], x * 20, y * 20);
            }

            frame++;
            if(frame == 24)
            {
                frame = 0;
            }

        }
        public void MoveGuard()
        {   // 1: reset path bools
            upPath = false;
            downPath = false;
            leftPath = false;
            rightPath = false;
            //2: check for valid paths
            upPath = CheckUp();
            downPath = CheckDown();
            leftPath = CheckLeft();
            rightPath = CheckRight();

            //3: change direction in accordance with the valid paths with respect to current direction
            ChooseDirection();


            //4: move
            if (d == Direction.UP)
            {
                this.moveUp();
            }
            else if (d == Direction.DOWN)
            {
                this.moveDown();
            }
            else if (d == Direction.LEFT)
            {
                this.moveLeft();
            }
            else if (d == Direction.RIGHT)
            {
                this.moveRight();
            }

        }
        //overload
        public void MoveGuard(Direction d)
        {   
            if (d == Direction.UP)
            {
                this.moveUp();
            }
            else if (d == Direction.DOWN)
            {
                this.moveDown();
            }
            else if (d == Direction.LEFT)
            {
                this.moveLeft();
            }
            else if (d == Direction.RIGHT)
            {
                this.moveRight();
            }

        }
        // down the rabbit hole?
        private void ChooseDirection()
        {
            if (d == Direction.UP)
            {
                UpDirectionPicker();
            }
            else if (d == Direction.DOWN)
            {
                DownDirectionPicker();
            }
            else if (d == Direction.LEFT)
            {
                LeftDirectionPicker();
            }
            else if (d == Direction.RIGHT)
            {
                RightDirectionPicker();
            }

        }

        private void RightDirectionPicker()
        {
            //ALL DIRECTIONS REALATIVE
            int direction;
            if (upPath && downPath && leftPath && rightPath)
            {
                //left & right intersection
                //left and right fork
                Random rnd = new Random();
                direction = rnd.Next(1, 4);

                if (direction == 1)
                {
                    d = Direction.RIGHT;
                }
                else if (direction == 2)
                {
                    d = Direction.DOWN;
                }
                else if (direction == 3)
                {
                    d = Direction.UP;
                }
            }
            else if (upPath && !downPath && leftPath && rightPath)
            {
                //left intersection
                Random rnd = new Random();
                direction = rnd.Next(1, 3);

                if (direction == 1)
                {
                    d = Direction.RIGHT;
                }
                else if (direction == 2)
                {
                    d = Direction.UP;
                }

            }
            else if (!upPath && downPath && leftPath && rightPath)
            {
                //right intersection
                Random rnd = new Random();
                direction = rnd.Next(1, 3);

                if (direction == 1)
                {
                    d = Direction.RIGHT;
                }
                else if (direction == 2)
                {
                    d = Direction.DOWN;
                }

            }
            else if (!upPath && !downPath && leftPath && rightPath)
            {
                //straight path
                d = Direction.RIGHT;
            }
            else if (upPath && !downPath && leftPath && !rightPath)
            {
                //left corner
                d = Direction.UP;
            }
            else if (!upPath && downPath && leftPath && !rightPath)
            {
                //right corner
                d = Direction.DOWN;
            }
            else if (upPath && downPath && leftPath && !rightPath)
            {
                //left and right fork
                Random rnd = new Random();
                direction = rnd.Next(1, 3);

                if (direction == 1)
                {
                    d = Direction.UP;
                }
                else if (direction == 2)
                {
                    d = Direction.DOWN;
                }
            }
            else if (!upPath && !downPath && leftPath && !rightPath)
            {
                //Deadend
                d = Direction.LEFT;
            }

        }

        private void LeftDirectionPicker()
        {
            //ALL DIRECTIONS REALATIVE
            int direction;
            if (upPath && downPath && leftPath && rightPath)
            {
                //left & right intersection
                //left and right fork
                Random rnd = new Random();
                direction = rnd.Next(1, 4);

                if (direction == 1)
                {
                    d = Direction.LEFT;
                }
                else if (direction == 2)
                {
                    d = Direction.DOWN;
                }
                else if (direction == 3)
                {
                    d = Direction.UP;
                }
            }
            else if (!upPath && downPath && leftPath && rightPath)
            {
                //left intersection
                Random rnd = new Random();
                direction = rnd.Next(1, 3);

                if (direction == 1)
                {
                    d = Direction.LEFT;
                }
                else if (direction == 2)
                {
                    d = Direction.DOWN;
                }

            }
            else if (upPath && !downPath && leftPath && rightPath)
            {
                //right intersection
                Random rnd = new Random();
                direction = rnd.Next(1, 3);

                if (direction == 1)
                {
                    d = Direction.LEFT;
                }
                else if (direction == 2)
                {
                    d = Direction.UP;
                }

            }
            else if (!upPath && !downPath && leftPath && rightPath)
            {
                //straight path
                d = Direction.LEFT;
            }
            else if (!upPath && downPath && !leftPath && rightPath)
            {
                //left corner
                d = Direction.DOWN;
            }
            else if (upPath && !downPath && !leftPath && rightPath)
            {
                //right corner
                d = Direction.UP;
            }
            else if (upPath && downPath && !leftPath && rightPath)
            {
                //left and right fork
                Random rnd = new Random();
                direction = rnd.Next(1, 3);

                if (direction == 1)
                {
                    d = Direction.UP;
                }
                else if (direction == 2)
                {
                    d = Direction.DOWN;

                }
            }
            else if (!upPath && !downPath && !leftPath && rightPath)
            {
                //Deadend
                d = Direction.RIGHT;
            }


        
    }

        private void DownDirectionPicker()
        {
            //ALL DIRECTIONS REALATIVE
            int direction;
            if (upPath && downPath && leftPath && rightPath)
            {
                //left & right intersection
                //left and right fork
                Random rnd = new Random();
                direction = rnd.Next(1, 4);

                if (direction == 1)
                {
                    d = Direction.LEFT;
                }
                else if (direction == 2)
                {
                    d = Direction.RIGHT;
                }
                else if (direction == 3)
                {
                    d = Direction.DOWN;
                }
            }
            else if (upPath && downPath && !leftPath && rightPath)
            {
                //left intersection
                Random rnd = new Random();
                direction = rnd.Next(1, 3);

                if (direction == 1)
                {
                    d = Direction.RIGHT;
                }
                else if (direction == 2)
                {
                    d = Direction.DOWN;
                }

            }
            else if (upPath && downPath && leftPath && !rightPath)
            {
                //right intersection
                Random rnd = new Random();
                direction = rnd.Next(1, 3);

                if (direction == 1)
                {
                    d = Direction.LEFT;
                }
                else if (direction == 2)
                {
                    d = Direction.DOWN;
                }

            }
            else if (upPath && downPath && !leftPath && !rightPath)
            {
                //straight path
                d = Direction.DOWN;
            }
            else if (upPath && !downPath && !leftPath && rightPath)
            {
                //left corner
                d = Direction.RIGHT;
            }
            else if (upPath && !downPath && leftPath && !rightPath)
            {
                //right corner
                d = Direction.LEFT;
            }
            else if (upPath && !downPath && leftPath && rightPath)
            {
                //left and right fork
                Random rnd = new Random();
                direction = rnd.Next(1, 3);

                if (direction == 1)
                {
                    d = Direction.LEFT;

                }
                else if (direction == 2)
                {
                    d = Direction.RIGHT;

                }
            }
            else if (upPath && !downPath && !leftPath && !rightPath)
            {
                //Deadend
                d = Direction.UP;
            }

    }

        private void UpDirectionPicker()
        {
            //ALL DIRECTIONS REALATIVE to current direction
            int direction;
            if (upPath && downPath && leftPath && rightPath)
            {
                //left & right intersection
                //left and right fork
                Random rnd = new Random();
                direction = rnd.Next(1, 4);

                if (direction == 1)
                {
                    d = Direction.LEFT;
                }
                else if (direction == 2)
                {
                    d = Direction.RIGHT;
                }
                else if (direction == 3)
                {
                    d = Direction.UP;
                }
            }
            else if (upPath && downPath && leftPath && !rightPath)
            {
                //left intersection
                Random rnd = new Random();
                direction = rnd.Next(1, 3);

                if (direction == 1)
                {
                    d = Direction.LEFT;
                }
                else if (direction == 2)
                {
                    d = Direction.UP;
                }

            }
            else if (upPath && downPath && !leftPath && rightPath)
            {
                //right intersection
                Random rnd = new Random();
                direction = rnd.Next(1, 3);

                if (direction == 1)
                {
                    d = Direction.RIGHT;
                }
                else if (direction == 2)
                {
                    d = Direction.UP;
                }

            }
            else if (upPath && downPath && !leftPath && !rightPath)
            {
                //straight path
                d = Direction.UP;
            }
            else if (!upPath && downPath && leftPath && !rightPath)
            {
                //left corner
                d = Direction.LEFT;
            }
            else if (!upPath && downPath && !leftPath && rightPath)
            {
                //right corner
                d = Direction.RIGHT;
            }
            else if (!upPath && downPath && leftPath && rightPath)
            {
                //left and right fork
                Random rnd = new Random();
                direction = rnd.Next(1, 3);

                if (direction == 1)
                {
                    d = Direction.LEFT;

                }
                else if (direction == 2)
                {
                    d = Direction.RIGHT;

                }
            }
            else if (!upPath && downPath && !leftPath && !rightPath)
            {
                //Deadend
                d = Direction.DOWN;
            }


        }

        protected override void moveUp()
        {
            //check next cell for wall
            if (mapCells[y - 1, x].CellType == 'w')
            {

            }
            else
            {
                previousY = y;
                y--;
                //render previous position 
                mapCells[previousY, x].DrawBackground(g);
            }



        }
        protected override void moveDown()
        {
            //check next cell
            if (mapCells[y + 1, x].CellType == 'w')
            {

            }
            else
            {
                previousY = y;
                y++;
                //render previous position 
                mapCells[previousY, x].DrawBackground(g);

            }

        }
        protected override void moveLeft()
        {
            //check next cell
            if (mapCells[y, x - 1].CellType == 'w')
            {

            }
            else
            {
                previousX = x;
                x--;
                //render previous position 
                mapCells[y, previousX].DrawBackground(g);
            }


        }
        protected override void moveRight()
        {
            //check next cell
            if (mapCells[y, x + 1].CellType == 'w')
            {

            }
            else
            {
                previousX = x;
                x++;
                //render previous position 
                mapCells[y, previousX].DrawBackground(g);
            }

        }

        protected bool CheckUp()
        {
            if (mapCells[y - 1, x].CellType != 'w')
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        protected bool CheckDown()
        {
            if (mapCells[y + 1, x].CellType != 'w')
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        protected bool CheckLeft()
        {
            if (mapCells[y, x - 1].CellType != 'w')
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        protected bool CheckRight()
        {
            if (mapCells[y, x + 1].CellType != 'w')
            {
                return true;
            }
            else
            {
                return false;
            }
        }




    }
}
