using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace csharpstudy240813command
{
    public class ProductResponseDTO
    {
        public string COM_CODE { get; set; }
        public string PROD_CD { get; set; }
        public string PROD_NM { get; set; }
        public int PRICE { get; set; }
        public DateTime WRITE_DT { get; set; }
        public static ProductResponseDTO From(Product prd)
        {
            return new ProductResponseDTO() {
                COM_CODE = prd.Key.COM_CODE,
                PROD_CD = prd.Key.PROD_CD,
                PROD_NM = prd.PROD_NM,
                PRICE = prd.PRICE,
                WRITE_DT = prd.WRITE_DT,
            };
        }
    }
}
