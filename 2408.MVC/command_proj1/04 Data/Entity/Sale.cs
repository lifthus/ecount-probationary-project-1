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
        public string PROD_NM { get; set; } // 판매 시점의 품목명 또한 캡쳐하는 것이 타당하기 때문에 판매 시점에 품목명을 저장한다.
        public decimal QTY { get; set; }
        public decimal UNIT_PRICE { get; set; }
        public string REMARKS { get; set; }

        public override string ToString()
        {
            return $"{Key.COM_CODE} | {Key.IO_DATE} | {Key.IO_NO} | {PROD_CD} | {QTY} | {UNIT_PRICE} | {REMARKS}";
        }
    }
}
