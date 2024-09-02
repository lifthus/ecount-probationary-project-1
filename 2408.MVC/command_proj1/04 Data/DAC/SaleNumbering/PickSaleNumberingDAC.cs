using command_proj1._04_Data.Db;
using System;
using System.Collections.Generic;

namespace command_proj1
{
    /// <summary>
    /// 채번 로직은 일반화 가능한 비즈니스 로직이 아니라 특수한 목적성을 띄기 때문에
    /// 굳이 여러 커맨드를 나누지 않고 핵심 로직을 해당 커맨드에 모아둔다.
    /// </summary>
    public class PickSaleNumberingDAC : Command<SaleNumbering>
    {
        public GetSaleNumberingDACRequestDTO Input { get; set; }

        protected override void Init() { }
        protected override void CanExecute() {
            if (Input == null)
            {
                throw new InexecutableCommandError("판매 채번 입력 없음");
            }
            if (Input.COM_CODE == null || Input.IO_DATE == null)
            {
                throw new InexecutableCommandError("COM_CODE와 IO_DATE 필요");
            }
            if (!Input.IO_DATE.vIsYYYYSMMSDD()) {
                throw new InexecutableCommandError("전표일자 형식 불일치");
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

            // 우선 해당 채번 레코드 쿼리
            var saleNumberings = dbManager.Query<SaleNumbering>(sql, parameters, (reader, data) => {
                data.Key.COM_CODE = reader["com_code"].ToString();
                data.Key.IO_DATE = reader["io_date"].ToString();
                data.IO_NO = Int32.Parse(reader["io_no"].ToString());
            });
            
            // 채번 레코드 없으면 새로 생성 후 바로 리턴
            if (saleNumberings.Count < 1) {
                var insertSQL =
                    "INSERT INTO flow.sale_numbering_jhl " +
                    "(com_code, io_date) VALUES " +
                    "(@com_code, @io_date)";
                var insertedRows = dbManager.Execute(insertSQL, parameters);
                if (insertedRows != 1) {
                    throw new Exception("새 채번 레코드 생성 실패");
                }
                var newSN = new SaleNumbering();
                newSN.Key.COM_CODE = Input.COM_CODE;
                newSN.Key.IO_DATE = Input.IO_DATE;
                newSN.IO_NO = 1;
                Output = newSN;
                return;
            }

            // 채번 레코드 있으면 채번 증가
            SaleNumbering targetNumbering = saleNumberings[0];
            targetNumbering.IO_NO += 1;

            // 증가된 값으로 업데이트
            var updateSQL =
                "UPDATE flow.sale_numbering_jhl " +
                "SET io_no = @io_no " +
                "WHERE com_code = @com_code AND io_date = @io_date";
            parameters.Add("@io_no", targetNumbering.IO_NO);
            var result = dbManager.Execute(updateSQL, parameters);

            if (result == 1) {
                Output = targetNumbering;
                return;
            }
            throw new Exception("채번 실패");
        }
        protected override void Executed() { }
    }
}
