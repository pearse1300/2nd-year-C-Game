using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeGame
{
    public abstract class Character
    {
        protected Cell[,] mapCells;
        protected Graphics g;
        protected int x, y, previousX, previousY;
        public abstract void Render();

        protected abstract void moveUp();
        protected abstract void moveDown();
        protected abstract void moveLeft();
        protected abstract void moveRight();

    }
}
