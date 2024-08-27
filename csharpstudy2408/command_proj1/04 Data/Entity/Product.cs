using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace command_proj1
{
    public class ProductKey: IEntityKey
    {
        public string COM_CODE {  get; set; }
        public string PROD_CD {  get; set; }

    }

    public class Product : Entity<ProductKey>
    {
        public string PROD_NM { get; set; }
        public int PRICE { get; set; }
        public DateTime WRITE_DT { get; set; }

        public override string ToString()
        {
            return $"{Key.COM_CODE} | {Key.PROD_CD} | {PROD_NM} | {PRICE} | {WRITE_DT}";
        }
    }
}
