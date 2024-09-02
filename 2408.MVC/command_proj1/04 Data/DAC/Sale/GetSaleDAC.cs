using command_proj1._04_Data.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace command_proj1
{
    public class GetSaleDACRequestDTO
    {
        public string COM_CODE;
        public string IO_DATE;
        public int IO_NO;
    }

    public class GetSaleDAC : Command<Sale>
    {
        public GetSaleDACRequestDTO Input { get; set; }

        protected override void Init() { }
        protected override void CanExecute() {
            if (Input == null)
            {
                throw new InexecutableCommandError("판매 조회 입력 없음");
            }
            if (Input.COM_CODE == null || Input.IO_DATE == null)
            {
                throw new InexecutableCommandError("COM_CODE와 IO_DATE 필요");
            }
        }
        protected override void OnExecuting() { }
        protected override void ExecuteCore()
        {
            var sql =
                "SELECT * " +
                "FROM flow.sale_jhl " +
                "WHERE " +
                    "com_code = @com_code " +
                    "AND io_date = @io_date " +
                    "AND io_no = @io_no " +
                "ORDER BY io_date, io_no " +
                "LIMIT 1";
            
            var parameters = new Dictionary<string, object>()
            {
                {"@com_code", Input.COM_CODE },
                {"@io_date",  Input.IO_DATE },
                {"@io_no",  Input.IO_NO }
            };

            var dbManager = new DbManager();
            var sales = dbManager.Query<Sale>(sql, parameters, (reader, data) => {
                data.Key.COM_CODE = reader["com_code"].ToString();
                data.Key.IO_DATE = reader["io_date"].ToString();
                data.Key.IO_NO = Int32.Parse(reader["io_no"].ToString());
                data.PROD_CD = reader["prod_cd"].ToString();
                data.PROD_NM = reader["prod_nm"].ToString();
                data.QTY = (decimal)reader["qty"];
                data.UNIT_PRICE = (decimal)reader["unit_price"];
                data.REMARKS = reader["remarks"].ToString();
            });
            if (sales.Count() < 1) {
                Output = null;
                return;
            }
            Output = sales[0];
        }
        protected override void Executed() { }
    }
}
