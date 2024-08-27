﻿using csharpstudy240813command._04_Data.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csharpstudy240813command
{
    public class GetSaleDAC : Command<Sale?>
    {
        public GetSaleDACRequestDTO Input { get; set; }

        protected override void Init() { }
        protected override void CanExecute() { }
        protected override void OnExecuting() { }
        protected override void ExecuteCore()
        {
            var sql = @"
                SELECT *
                FROM flow.sale_jhl
                WHERE 
                    com_code = @com_code
                    AND io_date = @io_date
                    AND io_no = @io_no
                ORDER BY io_date, io_no
                LIMIT 1
            ";
            var parameters = new Dictionary<string, object>()
            {
                {"@com_code", Input.ComCode },
                {"@io_date",  Input.IODate },
                {"@io_no",  Input.IONo }
            };

            var dbManager = new DbManager();
            var sales = dbManager.Query<Sale>(sql, parameters, (reader, data) => {
                data.Key.COM_CODE = reader["com_code"].ToString();
                data.Key.IO_DATE = reader["io_date"].ToString();
                data.Key.IO_NO = Int32.Parse(reader["io_no"].ToString());
                data.PROD_CD = reader["prod_cd"].ToString();
                data.QTY = (int)reader["qty"];
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

    public class GetSaleDACRequestDTO
    {
        public readonly string ComCode;
        public readonly string IODate;
        public readonly int IONo;
        public GetSaleDACRequestDTO(string ComCode, string IODate, int IONo)
        {
            this.ComCode = ComCode;
            this.IODate = IODate;
            this.IONo = IONo;
        }
    }
}
