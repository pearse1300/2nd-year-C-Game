using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeGame
{
    public class Lives:EventArgs
    {
        private int lives = 3;
        public int HowManyLives { get { return lives; } set { lives = value; } }
    }
}
