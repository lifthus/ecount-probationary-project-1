﻿using command_proj1._04_Data;
using command_proj1._04_Data.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace command_proj1
{
    public class InsertSaleDAC : Command<int>
    {
        public Sale Input { get; set; }

        protected override void Init() {

        }

        protected override void CanExecute() {
            if (Input == null) {
                throw new Exception("새 판매 정보 없음");
            }
            if (!Input.Key.IO_DATE.vIsYYYYSMMSDD()) {
                throw new Exception("전표일자 형식 오류");
            }
        }

        protected override void OnExecuting() {

        }

        protected override void ExecuteCore() {
            var sql = @"
                INSERT INTO flow.sale_jhl (com_code, io_date, io_no, prod_cd, prod_nm, unit_price, qty, remarks)
                VALUES (@com_code,  @io_date, @io_no, @prod_cd, @prod_nm, @unit_price, @qty, @remarks)
            ";

            var parameters = new Dictionary<string, object>() {
                {"@com_code", Input.Key.COM_CODE },
                {"@io_date", Input.Key.IO_DATE },
                {"@io_no", Input.Key.IO_NO },
                {"@prod_cd", Input.PROD_CD },
                {"@prod_nm", Input.PROD_NM },
                {"@unit_price", Input.UNIT_PRICE },
                {"@qty", Input.QTY },
                {"@remarks", Input.REMARKS },
            };

            var dbManager = new DbManager();
            Output = dbManager.Execute(sql, parameters);
        }

        protected override void Executed() {
        }
    }
}
