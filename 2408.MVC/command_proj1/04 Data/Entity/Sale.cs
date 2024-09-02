using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace command_proj1
{
    public class SaleKey: IEntityKey
    {
        public string COM_CODE {  get; set; }
        public string IO_DATE {  get; set; }
        public int IO_NO { get; set; }

    }

    public class Sale : Entity<SaleKey>
    {
        public string PROD_CD { get; set; }
        public decimal QTY { get; set; }
        public decimal UNIT_PRICE { get; set; }
        public string REMARKS { get; set; }

        public override string ToString()
        {
            return $"{Key.COM_CODE} | {Key.IO_DATE} | {Key.IO_NO} | {PROD_CD} | {QTY} | {UNIT_PRICE} | {REMARKS}";
        }
    }
}
