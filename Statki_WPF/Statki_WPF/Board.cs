using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Statki_WPF
{
    public class Board
    {
        public Board(int size)
        {
            this.size = size;
        }
        public int size { get; set; }
        public Field[,] field;
        public Player player;
        public void Init(Player p)
        {
            this.player = p;
            field = new Field[size, size];
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    field[i, j] = new Field();
                    field[i, j].Position_x = i;
                    field[i, j].Position_y = j;
                    field[i, j].Status = eFieldStatus.Empty;
                }
            }
        }

        public void DrawBoard()
        {
            System.Console.Write("\t");
            for (int i = 0; i < size; i++)
            {
                System.Console.Write(i + "\t");
            }
            System.Console.WriteLine();
            for (int i = 0; i < size; i++)
            {
                System.Console.Write(i + "\t");
                for (int j = 0; j < size; j++)
                {
                    if (field[i, j].Status == eFieldStatus.Empty) System.Console.Write("[ ]" + "\t");
                    if (field[i, j].Status == eFieldStatus.Empty_Missed) System.Console.Write("[*]" + "\t");
                    if (field[i, j].Status == eFieldStatus.Ship_Destoyed) System.Console.Write("[X]" + "\t");
                    if (field[i, j].Status == eFieldStatus.Ship) System.Console.Write("[#]" + "\t");
                }
                System.Console.WriteLine();
            }
            System.Console.WriteLine();
            System.Console.WriteLine();
        }
        public void DrawBoardHidden()
        {

            System.Console.Write("\t");
            for (int i = 0; i < size; i++)
            {
                System.Console.Write(i + "\t");
            }
            System.Console.WriteLine();
            for (int i = 0; i < size; i++)
            {
                System.Console.Write(i + "\t");
                for (int j = 0; j < size; j++)
                {
                    if (field[i, j].Status == eFieldStatus.Empty) System.Console.Write("[ ]" + "\t");
                    if (field[i, j].Status == eFieldStatus.Empty_Missed) System.Console.Write("[*]" + "\t");
                    if (field[i, j].Status == eFieldStatus.Ship_Destoyed) System.Console.Write("[X]" + "\t");
                    if (field[i, j].Status == eFieldStatus.Ship) System.Console.Write("[ ]" + "\t");
                }
                System.Console.WriteLine();
            }
        }

        public bool CanPutShip(int x, int y, int len, eDirection dir)
        {
            if (dir == eDirection.Horizontal)
            {
                if (x < 0 || x >= this.size || y < 0 || y + len >= this.size) return false;
                for (int i = x - 1; i <= x + 1; i++)
                {
                    for (int j = y - 1; j <= y + len; j++)
                    {
                        if (i >= 0 && i < this.size && j >= 0 && j < this.size)
                        {
                            if (this.field[i, j].Status != eFieldStatus.Empty) return false;
                        }
                    }
                }
            }
            else
            {
                if (x < 0 || x + len >= this.size || y < 0 || y >= this.size) return false;
                for (int i = x - 1; i <= x + len; i++)
                {
                    for (int j = y - 1; j <= y + 1; j++)
                    {
                        if (i >= 0 && i < this.size && j >= 0 && j < this.size)
                        {
                            if (this.field[i, j].Status != eFieldStatus.Empty) return false;
                        }
                    }
                }
            }
            return true;
        }

        public void PutShip(int x, int y, int length, eDirection dir)
        {
            if (dir == eDirection.Horizontal)
            {
                for (int i = 0; i < length; i++)
                {
                    field[x, y + i].Status = eFieldStatus.Ship;
                }
            }
            else
            {
                for (int i = 0; i < length; i++)
                {
                    field[x + i, y].Status = eFieldStatus.Ship;
                }
            }
        }

        public int Attack(int x, int y)
        {
            if (x < 0 || x >= this.size || y < 0 || y >= this.size) return -1;
            if (field[x, y].Status == eFieldStatus.Empty)
            {
                field[x, y].Status = eFieldStatus.Empty_Missed;
                return 0;
            }
            if (field[x, y].Status == eFieldStatus.Ship)
            {
                for (int i = 0; i < Game.SHIP_NUMBER; i++)
                {
                    bool flag = false;
                    if (this.player.ship[i].IsOnField(x, y) == true)
                    {
                        this.player.ship[i].energy -= 1;
                        if (this.player.ship[i].energy == 0)
                        {
                            this.player.ship[i].Sink();
                            SurroundShip(this.player.ship[i].position_x, this.player.ship[i].position_y, this.player.ship[i].direction, this.player.ship[i].length);
                        }
                        flag = true;
                    }
                    if (flag == true) break;
                }
                field[x, y].Status = eFieldStatus.Ship_Destoyed;
                return 1;
            }
            else
            {
                return -1;
            }



        }

        public void SurroundShip(int x, int y, eDirection dir, int len)
        {
            if (dir == eDirection.Horizontal)
            {
                for (int i = x - 1; i <= x + 1; i++)
                {
                    for (int j = y - 1; j <= y + len; j++)
                    {
                        if (i >= 0 && i < this.size && j >= 0 && j < this.size)
                        {
                            if (this.field[i, j].Status == eFieldStatus.Empty) this.field[i, j].Status = eFieldStatus.Empty_Missed;
                        }
                    }
                }
            }
            else
            {
                for (int i = x - 1; i <= x + len; i++)
                {
                    for (int j = y - 1; j <= y + 1; j++)
                    {
                        if (i >= 0 && i < this.size && j >= 0 && j < this.size)
                        {
                            if (this.field[i, j].Status == eFieldStatus.Empty) this.field[i, j].Status = eFieldStatus.Empty_Missed;
                        }
                    }
                }
            }
        }

        public bool CheckIfAllSinked()
        {
            for (int i = 0; i < this.size; i++)
            {
                for (int j = 0; j < this.size; j++)
                {
                    if (field[i, j].Status == eFieldStatus.Ship) return false;
                }
            }
            return true;
        }
    }
}
