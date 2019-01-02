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
        public Player owner;
        public void Init(Player p)  //Utworzenie planszy
        {
            this.owner = p;
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
        
        public bool CanPutShip(int x, int y, int len, eDirection dir)   //sprawdzenie możliwości ustawienia statku
        {
            if (dir == eDirection.Horizontal)
            {
                if (x < 0 || x >= this.size || y < 0 || y + len > this.size) return false;
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
                if (x < 0 || x + len > this.size || y < 0 || y >= this.size) return false;
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

        public void PutShip(int x, int y, int length, eDirection dir)     //ustawienie statku na dane pole
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

        public int Attack(int x, int y)     //wykonanie ataku na dane pole
        {
            if (x < 0 || x >= this.size || y < 0 || y >= this.size) return -1;
            if (field[x, y].Status == eFieldStatus.Empty)
            {
                field[x, y].Status = eFieldStatus.Empty_Missed;
                return 0;
            }
            if (field[x, y].Status == eFieldStatus.Ship)
            {
                for (int i = 0; i < Game.ALL_SHIP_NUMBER; i++)
                {
                    bool flag = false;
                    if (this.owner.ship[i].IsOnField(x, y) == true)
                    {
                        this.owner.ship[i].energy -= 1;
                        if (this.owner.ship[i].energy == 0)
                        {
                            this.owner.ship[i].Sink();
                            SurroundShip(this.owner.ship[i].position_x, this.owner.ship[i].position_y, this.owner.ship[i].direction, this.owner.ship[i].length);
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

        private void SurroundShip(int x, int y, eDirection dir, int len)   //otoczenie statku nieaktywnymi polami po zatopieniu
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

        public bool CheckIfAllSinked()  //sprawdzenie czy nie ma statków na planszy
        {
           if(this.owner.ShipNumber[0] + this.owner.ShipNumber[1] + this.owner.ShipNumber[2] + this.owner.ShipNumber[3] == 0)
                return true;
            return false;
        }

        public bool CanAttackNear(int x, int y) //czy można zaatakować "obok" danego pola
        {
            int fields = 4;
            if (x + 1 >= Game.BOARD_SIZE || field[x + 1, y].Status == eFieldStatus.Empty_Missed || field[x + 1, y].Status == eFieldStatus.Ship_Destoyed) fields--;
            if (y + 1 >= Game.BOARD_SIZE || field[x, y + 1].Status == eFieldStatus.Empty_Missed || field[x, y + 1].Status == eFieldStatus.Ship_Destoyed) fields--;
            if (x - 1 < 0 || field[x - 1, y].Status == eFieldStatus.Empty_Missed || field[x - 1, y].Status == eFieldStatus.Ship_Destoyed) fields--;
            if (y - 1 < 0 || field[x, y - 1].Status == eFieldStatus.Empty_Missed || field[x, y - 1].Status == eFieldStatus.Ship_Destoyed) fields--;
            if (fields > 0) return true;
            else return false;
        }
    }
}
