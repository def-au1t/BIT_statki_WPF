using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Statki_WPF
{

    public class Game
    {

        public MainWindow window;
        public Player player1;
        public Player player2;
        public bool finished;
        public Player winner;
        public static int SHIP_NUMBER = 10;
        public static int BOARD_SIZE = 10;
        public eState GameStatus;

        public Game(MainWindow window)
        {
            this.GameStatus = eState.Init;
            this.window = window;
            this.player1 = new Human(this, "Jacek");
            this.player2 = new Computer(this, "PC");

         //   player1.SetShips();
         //   System.Threading.Thread.Sleep(1000);
         //   player2.SetShips();
            // player1.board.DrawBoard();
            //  player2.board.DrawBoard();
            this.finished = false;
         //   this.GameInProgress();
        }

        public void GameStart()
        {
            this.GameStatus = eState.Started;

            window.CreateBoards(window.Player1_board);
            window.CreateBoards(window.Player2_board);
            window.Start_button.Content = "Ustaw statki";
            window.DrawBoard(player1.board, 1);
            window.DrawHiddenBoard(player2.board, 2);
        }


        public int MakeAttack(Player p, int x, int y)
        {
            if (p == player1) return player2.board.Attack(x, y);
            if (p == player2) return player1.board.Attack(x, y);
            return -1;
        }

        public void GameOver(Player win)
        {
            this.winner = win;

            //Game.DrawTitle();

            window.Start_button.Content = "Gra skończona. Wygrał ";
            window.Start_button.Content += winner.name;
            return;
        }

        public bool CheckIfFinished()
        {
            if (player1.board.CheckIfAllSinked()) return true;
            if (player2.board.CheckIfAllSinked()) return true;
            else return false;
        }

    }
}
