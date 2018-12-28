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
            if (game.GameStatus == eState.PlayerMove)
            {
                if (parent.Name == "Player1_board") return;
                else
                {
                    game.player1.MakeMove(Grid.GetRow(send), Grid.GetColumn(send));
                }

            }
        }
        public void setFieldColor(int boardNumber, int w, int k, eFieldStatus status, bool hidden) //zmiana koloru pola
        {
            SolidColorBrush brush = new SolidColorBrush();
            if (status == eFieldStatus.Empty) brush.Color = Color.FromRgb(12, 12, 12);
            if (status == eFieldStatus.Empty_Missed) brush.Color = Color.FromRgb(122, 122, 122);
            if (status == eFieldStatus.Ship)
            {
                if (hidden == true) { brush.Color = Color.FromRgb(12, 12, 12); }
                else { brush.Color = Color.FromRgb(0, 255, 0); }
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

        private void MainButtonCLick(object sender, RoutedEventArgs e)      //główny klawisz
        {
            if (game.GameStatus == eState.Init)
            {
                game.GameStart();
            }
            else if (game.GameStatus == eState.Started)
            {
                game.player1.SetShips();
                game.player2.SetShips();
                DrawBoard(game.player1.board, 1);
                if(Game.DEBUG==true)DrawBoard(game.player2.board, 2);
                else DrawHiddenBoard(game.player2.board, 2);
                UpdateShipNumber();
                game.GameStatus = eState.PlayerMove;
                this.Start_button.Content = game.player1.name + " na ruchu";
            }
        }
        public void ChangeStartButtonBackgroundToGrey() {
            this.Start_button.Background = new SolidColorBrush(Color.FromRgb(220,220,220));
        }
        public void ChangeStartButtonBackgroundToGreen()
        {
            this.Start_button.Background = new SolidColorBrush(Color.FromRgb(120, 255, 86));
        }

    }
}
