using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Statki_WPF
{
    public class Computer : Player
    {
        public Computer(Game g, String n) : base(g, n) { }
        public override void SetShips()
        {
            int k = 0;

            int position_x = -1;
            int position_y = -1;
            int dir = -1;
            Random rnd = new Random(Guid.NewGuid().GetHashCode());
            for (int j = 4; j >= 1; j--)
            {
                for (int i = 0; i < 5 - j; i++)
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


        public async override void MakeMove(int x=-1, int y=-1)
        {

            //  game.board1.DrawBoard();
            //  game.board2.DrawBoard();

            //     Game.DrawTitle();
            int result = -1;
            await Task.Delay(400);
            while (result == -1)
            {
                Random rnd = new Random(Guid.NewGuid().GetHashCode());
                x = rnd.Next(0, board.size);
                y = rnd.Next(0, board.size);
                result = game.MakeAttack(this, x, y);
            }
            if (result == 0) //trafiono puste pole
            {
                game.window.DrawBoard(game.player1.board, 1);
                game.window.Start_button.Content = "Jacek na ruchu";
                game.GameStatus = eState.PlayerMove;
                return;
            }
            if (result == 1) //trafiono okręt
            {
                game.window.DrawBoard(game.player1.board, 1);
                if (game.CheckIfFinished())
                {
                    game.GameStatus = eState.Finished;
                    game.GameOver(this);
                    return;
                };
                this.MakeMove();
            }

        }
    }
}