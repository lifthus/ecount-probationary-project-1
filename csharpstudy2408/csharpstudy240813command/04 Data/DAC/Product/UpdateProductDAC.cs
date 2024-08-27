using csharpstudy240813command._04_Data;
using csharpstudy240813command._04_Data.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csharpstudy240813command
{
    public class UpdateProductDAC : Command<int>
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
            var sql = @"
                UPDATE flow.product_jhl
                SET prod_nm = @prod_nm, price = @price, write_dt = @write_dt
                WHERE com_code = @com_code AND prod_cd = @prod_cd
            ";

            var parameters = new Dictionary<string, object>() {
                {"@com_code", Input.Key.COM_CODE },
                {"@prod_cd", Input.Key.PROD_CD },
                {"@prod_nm", Input.PROD_NM },
                {"@price", Input.PRICE },
                {"@write_dt", Input.WRITE_DT },
            };

            var dbManager = new DbManager();
            Output = dbManager.Execute(sql, parameters);
        }

        protected override void Executed()
        {
        }
    }
}
