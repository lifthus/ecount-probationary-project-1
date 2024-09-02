using command_proj1._04_Data.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace command_proj1
{
    public class GetSaleNumberingDACRequestDTO
    {
        public string COM_CODE;
        public string IO_DATE;
    }

    public class GetSaleNumberingDAC : Command<SaleNumbering>
    {
        public GetSaleNumberingDACRequestDTO Input { get; set; }

        protected override void Init() { }
        protected override void CanExecute()
        {
            if (Input == null) {
                throw new InexecutableCommandError("판매 채번 조회 입력 없음");
            }
            if (Input.COM_CODE == null || Input.IO_DATE == null) {
                throw new InexecutableCommandError("COM_CODE와 IO_DATE 필요");
            }
        }
        protected override void OnExecuting() { }
        protected override void ExecuteCore()
        {
            var sql =
                "SELECT * " +
                "FROM flow.sale_numbering_jhl " +
                "WHERE " +
                    "com_code = @com_code " +
                    "AND io_date = @io_date ";

            var parameters = new Dictionary<string, object>()
            {
                {"@com_code", Input.COM_CODE },
                {"@io_date",  Input.IO_DATE }
            };

            var dbManager = new DbManager();
            var saleNumberings = dbManager.Query<SaleNumbering>(sql, parameters, (reader, data) => {
                data.Key.COM_CODE = reader["com_code"].ToString();
                data.Key.IO_DATE = reader["io_date"].ToString();
                data.IO_NO = Int32.Parse(reader["io_no"].ToString());
            });
            if (saleNumberings.Count < 1) {
                Output = null;
                return;
            }
            Output = saleNumberings[0];
        }
        protected override void Executed() { }
    }
}
