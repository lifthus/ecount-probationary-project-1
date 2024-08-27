using command_proj1._04_Data;
using command_proj1._04_Data.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace command_proj1
{
    public class DeleteProductDAC : Command<int>
    {
        public Product Input { get; set; }

        protected override void Init() { }
        
        protected override void CanExecute() {
            if (Input == null) {
                throw new Exception("삭제 대상 품목 정보 필요");
            }
        }

        protected override void OnExecuting() { }

        protected override void ExecuteCore()
        {
            var sql = @"
                DELETE FROM flow.product_jhl
                WHERE com_code = @com_code AND prod_cd = @prod_cd
            ";

            var parameters = new Dictionary<string, object>() {
                {"@com_code", Input.Key.COM_CODE},
                {"@prod_cd", Input.Key.PROD_CD },
            };

            var dbManager = new DbManager();
            Output = dbManager.Execute(sql, parameters);
        }

        protected override void Executed()
        {
        }
    }
}
