using command_proj1._04_Data.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace command_proj1
{
    public class SelectSaleDACRequestDTO
    {
        /// <summary>
        /// EQUAL
        /// </summary>
        public string COM_CODE { get; set; }
        /// <summary>
        /// IN
        /// </summary>
        public string[] PROD_CD_list { get; set; }
        /// <summary>
        /// LIKE
        /// </summary>
        public string REMARKS { get; set; }
        /// <summary>
        /// BETWEEN
        /// </summary>
        public string IO_DATE_start { get; set; }
        /// <summary>
        /// BETWEEN
        /// </summary>
        public string IO_DATE_end { get; set; }
        /// <summary>
        /// -1 0 date DESC no ASC / 1 date ASC no ASC
        /// </summary>
        public int IO_DATE_NO_ord { get; set; }
        /// <summary>
        /// -1 DESC / 0 NONE / 1 ASC
        /// </summary>
        public int PROD_CD_ord { get; set; }

        public int pageSize;
        public int pageNo;
    }

    public class SelectSaleDACResponseDTO
    {
        public List<Sale> list;
        public int totalCount;
        public int pageSize;
        public int pageNo;
    }

    public class SelectSaleDAC : Command<SelectSaleDACResponseDTO>
    {
        public SelectSaleDACRequestDTO Input { get; set; }

        protected override void Init() { }
        protected override void CanExecute()
        {
            if (Input == null) {
                throw new Exception("판매 SELECT 조건 명세 필요");
            }
        }
        protected override void OnExecuting() { }
        protected override void ExecuteCore()
        {
            var countQuery = new StringBuilder("SELECT COUNT(*) ");
            var entityQuery = new StringBuilder("SELECT * ");

            // WHERE 절은 두 쿼리 다 붙이기
            var filterSQL = new StringBuilder(
                "FROM flow.sale_jhl " +
                $"WHERE com_code = @com_code AND prod_cd = ANY(@prod_cd_list) AND remarks LIKE @remarks AND " +
                "io_date BETWEEN @io_date_start AND @io_date_end "
                );
            countQuery.Append(filterSQL);
            entityQuery.Append(filterSQL);

            var parameters = new Dictionary<string, object>()
            {
                {"@com_code", Input.COM_CODE },
                {"@prod_cd_list", Input.PROD_CD_list },
                {"@remarks", $"%{Input.REMARKS}%"},
                {"@io_date_start", Input.IO_DATE_start },
                {"@io_date_end", Input.IO_DATE_end }
            };

            var dbManager = new DbManager();

            // 카운트 쿼리 먼저 실행
            var totalCount = dbManager.Scalar(countQuery.ToString(), parameters, reader => {
                return Int32.Parse(reader["count"].ToString());
            });

            // 정렬
            entityQuery.AppendLine("ORDER BY ");
            if (Input.PROD_CD_ord < 0) {
                entityQuery.AppendLine("prod_cd DESC, ");
            } else if (0 < Input.PROD_CD_ord) {
                entityQuery.AppendLine("prod_cd ASC, ");
            }
            if (Input.IO_DATE_NO_ord > 0) {
                entityQuery.AppendLine("io_date ASC, ");
            } else {
                entityQuery.AppendLine("io_date DESC, ");
            }
            entityQuery.AppendLine("io_no ASC ");

            entityQuery.AppendLine("LIMIT @pageSize OFFSET @offset");

            parameters.Add("@pageSize", Input.pageSize);
            parameters.Add("@offset", Input.pageSize * (Input.pageNo - 1));

            // 쿼리
            var saleList = dbManager.Query<Sale>(entityQuery.ToString(), parameters, (reader, data) =>
            {
                data.Key.COM_CODE = reader["com_code"].ToString();
                data.Key.IO_DATE = reader["io_date"].ToString();
                data.Key.IO_NO = (int)reader["io_no"];
                data.PROD_CD = reader["prod_cd"].ToString();
                data.PROD_NM = reader["prod_nm"].ToString();
                data.UNIT_PRICE = (decimal)reader["unit_price"];
                data.QTY = (decimal)reader["qty"];
                data.REMARKS = reader["remarks"].ToString();
            });

            Output = new SelectSaleDACResponseDTO();
            Output.list = saleList;
            Output.totalCount = totalCount;
            Output.pageSize = Input.pageSize;
            Output.pageNo = Input.pageNo;
        }
        protected override void Executed() { }
    }
}