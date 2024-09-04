using command_proj1._04_Data;
using command_proj1._04_Data.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace command_proj1
{
    public class InsertProductDAC : Command<int>
    {
        public Product Input {  get; set; }

        protected override void Init()
        {

        }

        protected override void CanExecute()
        {

        }

        protected override void OnExecuting()
        {

        }

        protected override void ExecuteCore()
        {
            // 딸라 템플릿 리터럴 변수, 골뱅이 개행 가능 , @쓰면 파라미터로 사용 가능
            var sql =
                "INSERT INTO flow.product_jhl (com_code, prod_cd, prod_nm, price, active, write_dt) " +
                "VALUES (@com_code, @prod_cd, @prod_nm, @price, @active, @write_dt)";

            var parameters = new Dictionary<string, object>() {
                {"@com_code", Input.Key.COM_CODE },
                {"@prod_cd", Input.Key.PROD_CD },
                {"@prod_nm", Input.PROD_NM },
                {"@price", Input.PRICE },
                {"@active", Input.ACTIVE },
                {"@write_dt", DateTime.Now },
            };

            var dbManager = new DbManager();
            Output = dbManager.Execute(sql, parameters);
        }

        protected override void Executed()
        {
        }
    }
}
