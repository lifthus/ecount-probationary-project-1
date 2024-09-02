using command_proj1._04_Data.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace command_proj1
{
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

    public class GetProductDAC : Command<Product>
    {
        public GetProductDACRequestDTO Input { get; set; }

        protected override void Init() { }
        protected override void CanExecute() {
            if (Input == null) {
                throw new InexecutableCommandError("품목 조회 명세 필요");
            }
        }
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
                data.PRICE = (decimal)reader["price"];
                data.PROD_NM = reader["prod_nm"].ToString();
                data.ACTIVE = (bool)reader["active"];
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
}
