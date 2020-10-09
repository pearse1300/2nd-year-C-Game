/* Cell.cs
 * 
 * This program is used to construct the game board for COM377 group project.
 * You are not allowed to make any changes to this class.
 * 
 */


using System.Drawing;
using System.Drawing.Drawing2D;

namespace MazeGame
{
    public class Cell
    {
        const int CellSize = 20; //cell dimension in pixels
        private int x, y;
        private char type;
        private bool isVisible = true;

        //Constructor
        public Cell(int x, int y, char type)
        {
            this.type = type;
            this.x = x;
            this.y = y;
        }

        //Set cell as visited (no pill inside it)
        public bool IsVisible
        {
            get
            {
                return isVisible;
            }

            set
            {
                isVisible = value;
            }
        }

        public char CellType
        {
            get { return type; }
            set { type = value; }
        }

        //draw the cell 
        public virtual void DrawBackground(Graphics g)
        {

            switch (type)
            {
                case 'w'://wall
                    g.FillRectangle(Brushes.Black, x * CellSize, y * CellSize, CellSize, CellSize);
                    break;
                case 'p'://path
                    g.FillRectangle(Brushes.White, x * CellSize, y * CellSize, CellSize, CellSize);
                    break;
                case 's'://starting position
                    g.FillRectangle(Brushes.Blue, x * CellSize, y * CellSize, CellSize, CellSize);
                    g.DrawString("S", new Font("Arial", 12, FontStyle.Bold), new SolidBrush(Color.Red), new Point(x * CellSize, y * CellSize));
                    break;
                case 'f'://finishing line
                    g.FillRectangle(Brushes.Blue, x * CellSize, y * CellSize, CellSize, CellSize);
                    g.DrawString("F", new Font("Arial", 12, FontStyle.Bold), new SolidBrush(Color.Red), new Point(x * CellSize, y * CellSize));
                    break;
                case 'g'://guard position
                    g.FillRectangle(Brushes.Blue, x * CellSize, y * CellSize, CellSize, CellSize);
                    g.DrawString("G", new Font("Arial", 12, FontStyle.Bold), new SolidBrush(Color.Red), new Point(x * CellSize, y * CellSize));
                    break;
                default:
                    break;
            }
        }
    }

}

