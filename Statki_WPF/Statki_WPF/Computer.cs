using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Statki_WPF
{
    public class Computer : Player
    {
        public Stack<Field> ListOfAttackedAndEmptyNear;
        public Computer(Game g, String n) : base(g, n) {
            ListOfAttackedAndEmptyNear = new Stack<Field>();
        }
        public override void SetShips()
        {
            SetShipsRandom();
            shipSetupCompleted = true;
        }

        private void MakeMoveRandom()
        {
            int x, y;
            do
            {
                Random rnd = new Random(Guid.NewGuid().GetHashCode());
                x = rnd.Next(0, board.size);
                y = rnd.Next(0, board.size);

            } while (MakeMoveOnField(x, y) == false);

        }

        public void MakeMoveBetter()
        {
            Field f;
            if(ListOfAttackedAndEmptyNear.Count == 0)
            {
                MakeMoveRandom();
                return;
            }
            else
            {
                bool flag = false;
                do                                  //ściąganie ze stosu elementów obok których jest już zajęte
                {
                    flag = false;
                    f = ListOfAttackedAndEmptyNear.Peek();
                    if (game.player1.board.CanAttackNear(f.Position_x, f.Position_y) == false)
                    {
                        ListOfAttackedAndEmptyNear.Pop();
                        flag = true;
                        if (ListOfAttackedAndEmptyNear.Count == 0) flag = false;
                    }
                } while (flag);
                if (ListOfAttackedAndEmptyNear.Count == 0)
                {
                    MakeMoveRandom();
                    return;
                }
            }
            bool flag2;
            do
            {
                Random rnd = new Random(Guid.NewGuid().GetHashCode());
                int rand = rnd.Next(0, 4);
                flag2 = false;
                switch (rand)
                {
                    case 0:
                        if (MakeMoveOnField(f.Position_x + 1, f.Position_y) == false) flag2 = true;
                        break;
                    case 1:
                        if (MakeMoveOnField(f.Position_x - 1, f.Position_y) == false) flag2 = true;
                        break;
                    case 2:
                        if (MakeMoveOnField(f.Position_x, f.Position_y + 1) == false) flag2 = true;
                        break;
                    case 3:
                        if (MakeMoveOnField(f.Position_x, f.Position_y - 1) == false) flag2 = true;
                        break;
                }
            } while (flag2 == true);
            return;
        }

        public bool MakeMoveOnField(int x, int y)
        {

            int result = game.MakeAttack(this, x, y);
            if (result != 0 && result != 1) return false;
            if (result == 0)
            {
                game.window.DrawBoard(game.player1.board, 1);
                game.window.UpdateShipNumber();
                game.window.Start_button.Content = game.player1.name+" na ruchu";
                game.GameStatus = eState.PlayerMove;
                return true;
            }

            if (result == 1)
            {
                game.window.DrawBoard(game.player1.board, 1);
                game.window.UpdateShipNumber();
                this.ListOfAttackedAndEmptyNear.Push(game.player1.board.field[x, y]); 
                if (game.CheckIfFinished())
                {
                    game.GameStatus = eState.Finished;
                    game.GameOver(this);
                    return true;
                };
                this.MakeMove();
                return true;
            }
            return true;

        }

        public async override void MakeMove(int x=-1, int y=-1)
        {
            await Task.Delay(Game.COMPUTER_DELAY);
            MakeMoveBetter();
        }
    }
}