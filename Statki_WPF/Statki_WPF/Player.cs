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
        public Ship[] ship = new Ship[Game.ALL_SHIP_NUMBER];
        public int[] ShipNumber = new int[4] { 0, 0, 0, 0 };

        public Player(Game g, String n)
        {

            this.game = g;
            this.board = new Board(Game.BOARD_SIZE);
            this.name = n;
            board.Init(this);
        }

        public abstract void SetShips();
        public abstract void MakeMove(int x, int y);

        public void SetShipsRandom()
        {
            for (int i = 0; i < 4; i++) {
                this.ShipNumber[i] = Game.SHIP_NUMBER[i];
            }
            int k = 0;

            int position_x = -1;
            int position_y = -1;
            int dir = -1;
            Random rnd = new Random(Guid.NewGuid().GetHashCode());
            for (int j = 4; j >= 1; j--)
            {
                for (int i = 0; i < Game.SHIP_NUMBER[j-1]; i++)
                {
                    do
                    {
                        position_x = rnd.Next(0, board.size);
                        position_y = rnd.Next(0, board.size);
                        dir = rnd.Next(0, 2);
                    } while (board.CanPutShip(position_x, position_y, j, (eDirection)dir) != true);

                    ship[k] = new Ship(j, position_x, position_y, (eDirection)dir, this.board);
                    board.PutShip(position_x, position_y, j, (eDirection)dir);

                    k++;
                }
            }
        }

    }
}
