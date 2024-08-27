using command_proj1._04_Data;
using command_proj1._04_Data.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace command_proj1
{
    public class DeleteSaleDACRequestDTO
    {
        public string ComCode;
        public string IODate;
        public int IONo;
        public DeleteSaleDACRequestDTO(string ComCode, string IODate, int IONo)
        {
            this.ComCode = ComCode;
            this.IODate = IODate;
            this.IONo = IONo;
        }
    }

    public class DeleteSaleDAC : Command<int>
    {
        public DeleteSaleDACRequestDTO Input {  get; set; }

        protected override void Init()
        {

        }

        protected override void CanExecute()
        {
            if (Input == null) {
                throw new Exception("삭제 대상 명세 필요");
            }
        }

        protected override void OnExecuting()
        {

        }

        protected override void ExecuteCore()
        {
            var sql = @"
                DELETE FROM flow.sale_jhl
                WHERE com_code = @com_code AND io_date = @io_date AND io_no = @io_no
            ";

            var parameters = new Dictionary<string, object>() {
                {"@com_code", Input.ComCode },
                {"@io_date", Input.IODate },
                {"@io_no", Input.IONo}
            };

            var dbManager = new DbManager();
            Output = dbManager.Execute(sql, parameters);
        }

        protected override void Executed()
        {
        }
    }
}
