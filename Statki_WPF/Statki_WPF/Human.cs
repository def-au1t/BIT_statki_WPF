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


        public override void SetShips()
        {
            SetShipsRandom();
            shipSetupCompleted = true;
        }
        public override void MakeMove(int x, int y) //Wykonanie ataku
        {
            int result = game.MakeAttack(this, x, y);
            if (result == 0)    //trafiono w puste pole
            {
                if (Game.DEBUG == true) game.window.DrawBoard(game.player2.board, 2);
                else game.window.DrawHiddenBoard(game.player2.board, 2);
                game.window.UpdateShipNumber();
                game.window.Start_button.Content = game.player2.name+" na ruchu";
                game.GameStatus = eState.ComputerMove;
                game.player2.MakeMove(-1,-1);
            }

            if (result == 1)    //trafiono w okręt
            {
                if (Game.DEBUG == true) game.window.DrawBoard(game.player2.board, 2);
                else game.window.DrawHiddenBoard(game.player2.board, 2);
                game.window.UpdateShipNumber();
                if (game.CheckIfFinished())
                {
                    game.GameStatus = eState.Finished;
                    game.GameOver(this);
                    return;
                };
                return;
            }
            return;
        }
    }
}
