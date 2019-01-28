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
        public Ship[] ship;
        public int[] ShipNumber;
        public bool shipSetupCompleted;
        public Player(Game g, String n)
        {
            this.shipSetupCompleted = false;
            this.ShipNumber = new int[4] { 0, 0, 0, 0 };
            this.ship = new Ship[Game.ALL_SHIP_NUMBER];
            this.game = g;
            this.board = new Board(Game.BOARD_SIZE);
            this.name = n;
            board.Init(this);
        }

        public abstract void SetShips();
        public abstract void MakeMove(int x, int y);
        protected void SetShipsRandom()
        {
            int k = 0;
            board.Init(this);
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
            for(int l=0; l<4; l++)
            {
                this.ShipNumber[l] = Game.SHIP_NUMBER[l];
            }
            shipSetupCompleted = true;
        }

        public void SetShipManually(int length, int x, int y, eDirection d)     //ręczne dodanie statku
        {
            int nr_statku = 0;
            int position_x = x;
            int position_y = y;
            eDirection dir = d;

            if (length == 0) return;
            if (this.ShipNumber[length - 1] >= Game.SHIP_NUMBER[length - 1]) return;
            if (board.CanPutShip(position_x, position_y, length, (eDirection)dir) == false) return;

            for (int i = 0; i < 4; i++)
            {
                nr_statku += this.ShipNumber[i];
            }

            ship[nr_statku] = new Ship(length, position_x, position_y, (eDirection)dir, this.board);
            board.PutShip(position_x, position_y, length, (eDirection)dir);
            this.ShipNumber[length - 1]++;

            game.window.UpdateAllShipsButton();
            game.window.DrawBoard(game.player1.board, 1);
            game.window.UpdateShipNumber();

            if(this.ShipNumber[length-1] == Game.SHIP_NUMBER[length - 1])
            {
                game.holdShipLength = 0;
            }

            bool allSet = true;
            for(int j=0; j<4; j++)
            {
                if (this.ShipNumber[j] != Game.SHIP_NUMBER[j]) allSet = false;
            }
            if (allSet == true)
            {
                shipSetupCompleted = true;
                game.ShipSetupCompleted();
            }
            return;
        }

        public void resetShipSetup() //resetowanie ustawienia statków gracza
        {
            for (int i = 0; i < 4; i++) {
                this.ShipNumber[i] = 0;
            }
            board.Init(this);
            shipSetupCompleted = false;
            game.window.ChangeStartButtonBackgroundToGreen();
            game.window.Start_button.Content = "Oczekiwanie na \n   rozstawienie";
        }
    }
}
