using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Statki_WPF
{

    public class Ship
    {
        public Ship(int length, int position_x, int position_y, eDirection dir, Board b)
        {
            this.energy = length;
            this.length = length;
            this.position_x = position_x;
            this.position_y = position_y;
            this.direction = dir;
            this.board = b;
            isSet = true;
            isSinked = false;
        }
        public int length { get; set; }
        public int energy { get; set; }
        public int position_x { get; set; }
        public int position_y { get; set; }
        public bool isSet { get; set; }
        public bool isSinked { get; set; }
        public eDirection direction { get; set; }
        public Board board { get; set; }
        public bool IsOnField(int x, int y)
        {
            if (direction == eDirection.Horizontal && x == this.position_x)
            {
                for (int i = 0; i < this.length; i++)
                {
                    if (this.position_y + i == y) return true;
                }
            }
            if (direction == eDirection.Vertical && y == this.position_y)
            {
                for (int i = 0; i < this.length; i++)
                {
                    if (this.position_x + i == x) return true;
                }
            }
            return false;
        }
        public void Sink()
        {
            this.isSinked = true;
            return;
        }
    }
}
