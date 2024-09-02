using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace command_proj1
{
    public class SaleNumberingKey: IEntityKey
    {
        public string COM_CODE { get; set; }
        public string IO_DATE { get; set; }
    }
    public class SaleNumbering: Entity<SaleNumberingKey>
    {
        public int IO_NO { get; set; }
    }
}
