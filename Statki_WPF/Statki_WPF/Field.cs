using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Statki_WPF
{
    public class Field
    {
        public Field(int pos_x = -1, int pos_y = -1, eFieldStatus status = eFieldStatus.Empty)
        {
            this.Position_x = pos_x;
            this.Position_y = pos_y;
            this.Status = status;
        }

        public int Position_x { get; set; }
        public int Position_y { get; set; }
        public eFieldStatus Status { get; set; }


    }
}
