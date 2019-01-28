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
        public int holdShipLength;
        public eDirection holdShipDir;

        //----------- KONFIGURACJA ------------------------
        public static bool DEBUG = false;                       //pokazywanie planszy komputera
        public static int BOARD_SIZE = 10;                      //rozmiar planszy
        public static int[] SHIP_NUMBER = new int[4] {4,3,2,1}; //liczba statków
        public static int COMPUTER_DELAY = 400;                 //opóźnienie ruchu komputera
        //-------------------------------------------------


        public Game(MainWindow window)
        {
            this.holdShipDir = eDirection.Horizontal;
            this.holdShipLength = 0;
            this.GameStatus = eState.Init;
            this.window = window;
            CalculateAllShipNumber();
            this.player1 = new Human(this, "Jacek");
            this.player2 = new Computer(this, "PC");
        }
        private void CalculateAllShipNumber()
        {
            int suma_statkow = 0;
            for (int i = 0; i< 4; i++) { suma_statkow += Game.SHIP_NUMBER[i]; }
            Game.ALL_SHIP_NUMBER = suma_statkow;
        }
        public void GameStart()     //Początek gry
        {
            this.GameStatus = eState.Started;

            window.Player1_name.Text = player1.name;
            window.Player2_name.Text = player2.name;
            window.CreateBoards(window.Player1_board);
            window.CreateBoards(window.Player2_board);

            window.Ship_setup.Visibility = System.Windows.Visibility.Visible;
            window.ChangeAutoButtonBackgroundToGreen();
            window.ChangeStartButtonBackgroundToGrey();
            window.Start_button.Content = "Oczekiwanie na \n   rozstawienie";
            window.DrawBoard(player1.board, 1);
            window.DrawHiddenBoard(player2.board, 2);
            window.UpdateShipNumber();
            this.GameStatus = eState.ShipSetup;
        }

        public void ShipSetupCompleted()    //Gotowość do rozpoczęcia
        {
            window.ChangeStartButtonBackgroundToGreen();
            window.Start_button.Content = "Rozpocznij";
        }

        public void beginPlay()     //rozpoczęcie rozgrywki
        {
            player2.SetShips();
            window.DrawBoard(player1.board, 1);
            if (Game.DEBUG == true) window.DrawBoard(player2.board, 2);
            else window.DrawHiddenBoard(player2.board, 2);
            window.UpdateShipNumber();
            GameStatus = eState.PlayerMove;
            window.ChangeStartButtonBackgroundToGrey();
            window.Start_button.Content = player1.name + " na ruchu";
            window.Ship_setup.Visibility = System.Windows.Visibility.Hidden;
        }

        public int MakeAttack(Player p, int x, int y) //wykonanie ataku
        {
            if (p == player1) return player2.board.Attack(x, y);
            if (p == player2) return player1.board.Attack(x, y);
            return -1;
        }

        public void GameOver(Player win)    //koniec gry
        {
            this.winner = win;

            window.ChangeStartButtonBackgroundToGreen();
            window.Start_button.Content = "Gra skończona!\n Wygrał ";
            window.Start_button.Content += winner.name;
            window.DrawBoard(player2.board, 2);
            return;
        }

        public bool CheckIfFinished()   //sprawdź, czy koniec gry
        {
            if (player1.board.CheckIfAllSinked()) return true;
            if (player2.board.CheckIfAllSinked()) return true;
            else return false;
        }

    }
}
