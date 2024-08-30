using command_proj1._04_Data;
using command_proj1._04_Data.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace command_proj1
{
    public class UpdateSaleDAC : Command<int>
    {
        public Sale Input {  get; set; }

        protected override void Init()
        {

        }

        protected override void CanExecute()
        {
            if (Input == null) {
                throw new Exception("업데이트 대상 판매 명세 필요");
            }
        }

        protected override void OnExecuting()
        {

        }

        protected override void ExecuteCore()
        {
            var sql = @"
                UPDATE flow.sale_jhl
                SET prod_cd = @prod_cd, qty = @qty, remarks = @remarks
                WHERE com_code = @com_code AND io_date = @io_date AND io_no = @io_no
            ";

            var parameters = new Dictionary<string, object>() {
                {"@com_code", Input.Key.COM_CODE },
                {"@io_date", Input.Key.IO_DATE},
                {"@io_no", Input.Key.IO_NO },
                {"@prod_cd", Input.PROD_CD },
                {"@qty", Input.QTY },
                {"@remarks", Input.REMARKS }
            };

            var dbManager = new DbManager();
            Output = dbManager.Execute(sql, parameters);
        }

        protected override void Executed()
        {
        }
    }
}
