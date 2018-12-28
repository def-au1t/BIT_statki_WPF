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
        public Player winner;
        public static int ALL_SHIP_NUMBER = -1;
        public eState GameStatus;

        //----------- KONFIGURACJA ------------------------
        public static bool DEBUG = false;                       //pokazywanie planszy komputera
        public static int BOARD_SIZE = 10;                      //rozmiar planszy
        public static int[] SHIP_NUMBER = new int[4] {4,3,2,1}; //liczba statków
        public static int COMPUTER_DELAY = 600;                 //opóźnienie ruchu komputera
        //-------------------------------------------------


        public Game(MainWindow window)
        {
            this.GameStatus = eState.Init;
            this.window = window;
            CalculateAllShipNumber();
            this.player1 = new Human(this, "Jacek");
            this.player2 = new Computer(this, "PC");
         //   player1.SetShips();
         //   System.Threading.Thread.Sleep(1000);
         //   player2.SetShips();
            // player1.board.DrawBoard();
            //  player2.board.DrawBoard();
         //   this.GameInProgress();
        }
        private void CalculateAllShipNumber()
        {
            int suma_statkow = 0;
            for (int i = 0; i< 4; i++) { suma_statkow += Game.SHIP_NUMBER[i]; }
            Game.ALL_SHIP_NUMBER = suma_statkow;
        }
        public void GameStart()
        {
            this.GameStatus = eState.Started;

            window.Player1_name.Text = player1.name;
            window.Player2_name.Text = player2.name;
            window.CreateBoards(window.Player1_board);
            window.CreateBoards(window.Player2_board);

            window.ChangeStartButtonBackgroundToGrey();
            window.Start_button.Content = "Ustaw statki";
            window.DrawBoard(player1.board, 1);
            window.DrawHiddenBoard(player2.board, 2);
            window.UpdateShipNumber();
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
            window.ChangeStartButtonBackgroundToGreen();
            window.Start_button.Content = "Gra skończona.\n Wygrał ";
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
