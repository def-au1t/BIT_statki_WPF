using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Statki_WPF
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Game game;
        public MainWindow()
        {
            InitializeComponent();
            Game g = new Game(this);
            this.game = g;
        }

        public void rectangle_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) //naciśnięto przycisk
        {
            Rectangle send = (Rectangle)sender;
            Grid parent = (Grid)send.Parent;
            int x = Grid.GetRow(send);
            int y = Grid.GetColumn(send);
            if (game.GameStatus == eState.PlayerMove)
            {
                if (parent.Name == "Player1_board") return;
                else
                {
                    game.player1.MakeMove(x, y);
                }
            }
            if (game.GameStatus == eState.HoldingShip)
            {
                if (parent.Name == "Player2_board") return;
                else
                {
                    game.player1.SetShipManually(game.holdShipLength, x, y, game.holdShipDir);
                }
            }

        }

        public void rectangle_MouseEnter(object sender, RoutedEventArgs e) //TODO
        {
            Rectangle send = (Rectangle)sender;
            Grid parent = (Grid)send.Parent;
            int x = Grid.GetRow(send);
            int y = Grid.GetColumn(send);
            if (game.GameStatus == eState.PlayerMove)
            {
                if (parent.Name == "Player1_board") return;
                else
                {       //Zmiana koloru po najechaniu na planszę przeciwnika przed strzałem

                    SolidColorBrush brush = new SolidColorBrush();
                    brush.Color = Color.FromRgb(150, 200, 255);
                    send.Fill = brush;
                }
            }
            if (game.GameStatus == eState.HoldingShip)
            {
                if (parent.Name == "Player2_board") return;
                else
                {   //Zmiana koloru przed ustawieniem statku
                }
            }
        }
        public void rectangle_MouseLeave(object sender, RoutedEventArgs e)
        {
            Rectangle send = (Rectangle)sender;
            Grid parent = (Grid)send.Parent;
            int x = Grid.GetRow(send);
            int y = Grid.GetColumn(send);
            if (game.GameStatus == eState.PlayerMove)
            {
                if (parent.Name == "Player1_board") return;
                else
                {       //Zmiana koloru po najechaniu na planszę przeciwnika przed strzałem

                    if (Game.DEBUG == true) setFieldColor(2, x, y, game.player2.board.field[x, y].Status, false);
                    else setFieldColor(2, x, y, game.player2.board.field[x, y].Status, true);
                }
            }
            if (game.GameStatus == eState.HoldingShip)
            {
                if (parent.Name == "Player2_board") return;
                else
                {   //Zmiana koloru przed ustawieniem statku
                    if (Game.DEBUG == true) setFieldColor(1, x, y, game.player1.board.field[x, y].Status, false);
                    else setFieldColor(1, x, y, game.player1.board.field[x, y].Status, true);
                }
            }

        }
        public void setFieldColor(int boardNumber, int w, int k, eFieldStatus status, bool hidden) //zmiana koloru pola
        {
            SolidColorBrush brush = new SolidColorBrush();
            if (status == eFieldStatus.Empty) brush.Color = Color.FromRgb(100, 150, 255);
            if (status == eFieldStatus.Empty_Missed) brush.Color = Color.FromRgb(180, 180, 180);
            if (status == eFieldStatus.Ship)
            {
                if (hidden == true) { brush.Color = Color.FromRgb(100, 150, 255); }
                else { brush.Color = Color.FromRgb(0, 0, 0); }
            }
            if (status == eFieldStatus.Ship_Destoyed) brush.Color = Color.FromRgb(255, 0, 0);
            if (boardNumber == 1)
            {
                Rectangle field = (Rectangle)Player1_board.Children[w * Game.BOARD_SIZE + k];
                field.Fill = brush;
            }
            if (boardNumber == 2)
            {
                Rectangle field = (Rectangle)Player2_board.Children[w * Game.BOARD_SIZE + k];
                field.Fill = brush;
            }
        }

        public void CreateBoards(Grid board)                //utworzenie plansz
        {
            double width = board.ActualWidth / Game.BOARD_SIZE;
            double height = board.ActualHeight / Game.BOARD_SIZE;

            for (int i = 0; i < Game.BOARD_SIZE; i++)
            {
                board.ColumnDefinitions.Add(new ColumnDefinition()
                {
                    Width = new GridLength(width)
                });
                board.RowDefinitions.Add(new RowDefinition()
                {
                    Height = new GridLength(height)
                });
            }

            SolidColorBrush brush = new SolidColorBrush(Color.FromRgb(200, 200, 200));
            for (int i = 0; i < Game.BOARD_SIZE; i++)
            {
                for (int j = 0; j < Game.BOARD_SIZE; j++)
                {
                    var field = new Rectangle
                    {
                        Margin = new Thickness(1.0),
                        Fill = brush,
                    };
                    field.MouseLeftButtonDown += rectangle_MouseLeftButtonDown;
                    field.MouseEnter += rectangle_MouseEnter;
                    field.MouseLeave += rectangle_MouseLeave;
                    field.SetValue(Grid.RowProperty, i);
                    field.SetValue(Grid.ColumnProperty, j);
                    board.Children.Add(field);
                }
            }
        }

        public void UpdateShipNumber()
        {
            for (int j = 0; j < 4; j++)
            {
                SolidColorBrush brush = new SolidColorBrush();
                SolidColorBrush brush2 = new SolidColorBrush();
                TextBlock textbox = this.player1_ship_grid.Children.Cast<TextBlock>()
                    .First(i => Grid.GetRow(i) == j && Grid.GetColumn(i) == 1);
                TextBlock textbox2 = this.player2_ship_grid.Children.Cast<TextBlock>()
                    .First(i => Grid.GetRow(i) == j && Grid.GetColumn(i) == 1);
                if (game.player1.ShipNumber[j] == 0)
                {
                    brush.Color = Color.FromRgb(198, 0, 0);
                    textbox.Foreground = brush;
                    textbox.Text = "brak";
                }
                else
                {
                    brush.Color = Color.FromRgb(0, 198, 0);
                    textbox.Foreground = brush;
                    textbox.Text = game.player1.ShipNumber[j].ToString();
                }
                if (game.player2.ShipNumber[j] == 0)
                {
                    brush2.Color = Color.FromRgb(198, 0, 0);
                    textbox2.Foreground = brush2;
                    textbox2.Text = "brak";
                }
                else
                {
                    brush2.Color = Color.FromRgb(0, 198, 0);
                    textbox2.Foreground = brush2;
                    textbox2.Text = game.player2.ShipNumber[j].ToString();
                }
            }
        }

        public void DrawBoard(Board board, int boardNumber)
        {
            for (int i = 0; i < Game.BOARD_SIZE; i++)
            {
                for (int j = 0; j < Game.BOARD_SIZE; j++)
                {
                    setFieldColor(boardNumber, i, j, board.field[i, j].Status, false);
                }
            }
        }
        public void DrawHiddenBoard(Board board, int boardNumber)
        {
            for (int i = 0; i < Game.BOARD_SIZE; i++)
            {
                for (int j = 0; j < Game.BOARD_SIZE; j++)
                {
                    setFieldColor(boardNumber, i, j, board.field[i, j].Status, true);
                }
            }
        }

        public void SetupShipsAutomatically()
        {
            game.player1.SetShips();
        }

        private void MainButtonCLick(object sender, RoutedEventArgs e)      //główny klawisz
        {
            if (game.GameStatus == eState.Init)
            {
                game.GameStart();
            }
            else if (game.GameStatus == eState.ShipSetup || game.GameStatus == eState.HoldingShip)
            {
                if (game.player1.shipSetupCompleted == true)
                    game.beginPlay();

            }
        }
        public void ChangeStartButtonBackgroundToGrey() {
            this.Start_button.Background = new SolidColorBrush(Color.FromRgb(220,220,220));
        }
        public void ChangeStartButtonBackgroundToGreen()
        {
            this.Start_button.Background = new SolidColorBrush(Color.FromRgb(120, 255, 86));
        }
        public void ChangeAutoButtonBackgroundToGreen()
        {
            this.Set_auto.Background = new SolidColorBrush(Color.FromRgb(120, 255, 86));
        }
        public void ChangeAutoButtonBackgroundToGrey()
        {
            this.Set_auto.Background = new SolidColorBrush(Color.FromRgb(220, 220, 220));
        }

        private void Set1Ship_Click(object sender, RoutedEventArgs e)
        {
            game.GameStatus = eState.HoldingShip;
            game.holdShipLength = 1;
        }

        private void Set2Ship_Click(object sender, RoutedEventArgs e)
        {

            game.GameStatus = eState.HoldingShip;
            game.holdShipLength = 2;
        }

        private void Set3Ship_Click(object sender, RoutedEventArgs e)
        {

            game.GameStatus = eState.HoldingShip;
            game.holdShipLength = 3;
        }

        private void Set4Ship_Click(object sender, RoutedEventArgs e)
        {

            game.GameStatus = eState.HoldingShip;
            game.holdShipLength = 4;
        }

        private void Turn_Click(object sender, RoutedEventArgs e)
        {
            if (game.holdShipDir == eDirection.Horizontal) game.holdShipDir = eDirection.Vertical;
            else game.holdShipDir = eDirection.Horizontal;
        }

        private void Set_auto_Click(object sender, RoutedEventArgs e)
        {
            if (game.GameStatus == eState.ShipSetup || game.GameStatus == eState.HoldingShip)
            {
                SetupShipsAutomatically();
                DrawBoard(game.player1.board, 1);
                UpdateShipNumber();
                game.ShipSetupCompleted();
                UpdateAllShipsButton();
            }
        }

        private void Reset_ships_Click(object sender, RoutedEventArgs e)
        {
            if (game.GameStatus == eState.ShipSetup || game.GameStatus == eState.HoldingShip)
            {
                ChangeAutoButtonBackgroundToGreen();
                game.player1.resetShipSetup();
                DrawBoard(game.player1.board, 1);
                UpdateShipNumber();
                ChangeStartButtonBackgroundToGrey();
                UpdateAllShipsButton();
            }
        }

        public void UpdateAllShipsButton()
        {
            bool isAllUnset = true;

            for (int j = 0; j < 4; j++)
            {
                if (game.player1.ShipNumber[j] != 0) isAllUnset = false;
                if (game.player1.ShipNumber[j] == Game.SHIP_NUMBER[j])
                {
                    Button button = this.Ship_setup.Children.Cast<Button>()
                            .First(i => Grid.GetRow(i) == 3 - j && Grid.GetColumn(i) == 0);
                    button.Visibility = Visibility.Hidden;
                }
                else
                {
                    Button button = this.Ship_setup.Children.Cast<Button>()
                            .First(i => Grid.GetRow(i) == 3 - j && Grid.GetColumn(i) == 0);
                    button.Visibility = Visibility.Visible;
                }
            }
            if (isAllUnset == true)
            {
                ChangeAutoButtonBackgroundToGreen();
            }
            else ChangeAutoButtonBackgroundToGrey();
        }
    }
}
