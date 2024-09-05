using command_proj1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _2408.MVC.Services
{
    public class SaleKeyDTO
    {
        public readonly string COM_CODE;
        public readonly string IO_DATE;
        public readonly int IO_NO;
        public SaleKeyDTO(SaleKey saleKey)
        {
            this.COM_CODE = saleKey.COM_CODE;
            this.IO_DATE = saleKey.IO_DATE;
            this.IO_NO = saleKey.IO_NO;
        }
    }

    public class SaleDTO
    {
        public readonly SaleKeyDTO Key;
        public readonly string PROD_CD;
        public readonly string PROD_NM;
        public readonly string QTY;
        public readonly string UNIT_PRICE;
        public readonly string REMARKS;
        
        public SaleDTO(Sale sale)
        {
            this.Key = new SaleKeyDTO(sale.Key);
            PROD_CD = sale.PROD_CD;
            PROD_NM = sale.PROD_NM;
            QTY = sale.QTY.ToString();
            UNIT_PRICE = sale.UNIT_PRICE.ToString();
            REMARKS = sale.REMARKS;
        }
    }

    public class SaleSelectDTO
    {
        public int totalCount;
        public int pageSize;
        public int pageNo;
        public List<SaleDTO> sales;
    }
}