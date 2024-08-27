using csharpstudy240813command._04_Data.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csharpstudy240813command
{
    public class GetProductDAC : Command<Product?>
    {
        public GetProductDACRequestDTO Input { get; set; }

        protected override void Init() { }
        protected override void CanExecute() { }
        protected override void OnExecuting() { }
        protected override void ExecuteCore()
        {
            var sql = @"
                SELECT *
                FROM flow.product_jhl
                WHERE com_code = @com_code AND prod_cd = @prod_cd
                ORDER BY write_dt
                LIMIT 1
            ";
            var parameters = new Dictionary<string, object>()
            {
                {"@com_code", Input.ComCode },
                {"@prod_cd",  Input.ProdCd }
            };

            var dbManager = new DbManager();
            var products = dbManager.Query<Product>(sql, parameters, (reader, data) => {
                data.Key.COM_CODE = reader["com_code"].ToString();
                data.Key.PROD_CD = reader["prod_cd"].ToString();
                data.PRICE = (int)reader["price"];
                data.PROD_NM = reader["prod_nm"].ToString();
                data.WRITE_DT = (DateTime)reader["write_dt"];
            });
            if (products.Count() < 1) {
                Output = null;
                return;
            }
            Output = products[0];
        }
        protected override void Executed() { }
    }

    public class GetProductDACRequestDTO
    {
        public readonly string ComCode;
        public readonly string ProdCd;
        public GetProductDACRequestDTO(string ComCode, string ProdCd)
        {
            this.ComCode = ComCode;
            this.ProdCd = ProdCd;
        }
    }
}
