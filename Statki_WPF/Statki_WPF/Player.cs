using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Statki_WPF
{
    public abstract class Player
    {
        public String name;
        public Game game;
        public Board board;
        public Ship[] ship = new Ship[Game.SHIP_NUMBER];

        public Player(Game g, String n)
        {

            this.game = g;
            this.board = new Board(Game.BOARD_SIZE);
            this.name = n;
            board.Init(this);
        }

        public abstract void SetShips();
        public abstract void MakeMove(int x, int y);

    }
}
