using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Statki_WPF
{

    public class Human : Player
    {
        public Human(Game g, String n) : base(g, n) { }
        /*     public override void SetShips()
             {
                 int k = 0;

                 int position_x = -1;
                 int position_y = -1;
                 int dir = -1;
                 for (int dl = 4; dl >= 1; dl--)
                 {
                     for (int i = 0; i < 5-dl; i++)
                     {
                         do
                         {
                             System.Console.WriteLine("Podaj parametry " + (5 - dl) + " " + dl + "-masztowca: ");
                             position_x = ReadNumber();
                             position_y = ReadNumber();
                             dir = ReadNumber();

                         } while (board.CanPutShip(position_x, position_y, dl, (eDirection)dir) != true);
                         ship[k] = new Ship(dl, position_x, position_y, (eDirection)dir, this.board);
                         board.PutShip(position_x, position_y, dl, (eDirection)dir);
                         ship[k].isSet = true;
                         k++;
                         Console.Clear();
                         this.board.DrawBoard();
                     }
                 }
             } 
             */

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
        public override void MakeMove(int x, int y)
        {
            int result = game.MakeAttack(this, x, y);
            if (result == 0)
            {
                game.window.DrawHiddenBoard(game.player2.board, 2);
                game.window.Start_button.Content = "Komputer na ruchu";
                game.GameStatus = eState.ComputerMove;
                game.player2.MakeMove(-1,-1);
            }

            if (result == 1)
            {
                game.window.DrawHiddenBoard(game.player2.board, 2);
                if (game.CheckIfFinished())
                {
                    game.finished = true;
                    game.GameStatus = eState.Finished;
                    game.GameOver(this);
                    return;
                };
                return;
            }
            return;

        }
    /*    public int ReadNumber()
        {
          //  string l = Console.ReadLine();

            int line = 0;

            while (!int.TryParse(l, out line))
            {
                l = Console.ReadLine();
            }
            return line;
            ;
        } */
    }
}
