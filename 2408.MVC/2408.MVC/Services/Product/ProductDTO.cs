using command_proj1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _2408.MVC.Services
{
    public class ProductKeyDTO
    {
        public readonly string COM_CODE;
        public readonly string PROD_CD;
        public ProductKeyDTO(ProductKey prdKey)
        {
            this.COM_CODE = prdKey.COM_CODE;
            this.PROD_CD = prdKey.PROD_CD;
        }
    }

    public class ProductDTO
    {
        public readonly ProductKeyDTO key;
        public readonly string PROD_NM;
        public readonly int PRICE;
        public readonly string WRITE_DT;
        
        public ProductDTO(Product prd)
        {
            this.key = new ProductKeyDTO(prd.Key);
            this.PROD_NM = prd.PROD_NM;
            this.PRICE = prd.PRICE;
            this.WRITE_DT = prd.WRITE_DT.ToString();
        }
    }
}