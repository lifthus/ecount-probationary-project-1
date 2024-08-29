using command_proj1._04_Data.Db;
using Npgsql.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace command_proj1
{
    public class SelectProductDACRequestDTO
    {
        /// <summary>
        /// EQUAL
        /// </summary>
        public string COM_CODE;

        /// <summary>
        /// LIKE
        /// </summary>
        public string PROD_CD;
        /// <summary>
        /// LIKE
        /// </summary>
        public string PROD_NM;

        /// <summary>
        /// -1 DESC
        /// 0 NONE
        /// 1 ASC
        /// </summary>
        public int prodNmOrd; // -1 DESC 1 ASC
    }

    public class SelectProductDAC : Command<List<Product>>
    {
        public SelectProductDACRequestDTO Input { get; set; }

        protected override void Init() { }
        protected override void CanExecute()
        {
            if (Input == null) {
                throw new InexecutableCommandError("품목 조회 필터 정보 필요");
            }
            if (Input.COM_CODE == null) {
                throw new InexecutableCommandError("회사 코드 필요");
            }
        }
        protected override void OnExecuting()
        {
            if (Input.PROD_CD == null) {
                Input.PROD_CD = "";
            }
            if (Input.PROD_NM == null) {
                Input.PROD_NM = "";
            }
        }
        protected override void ExecuteCore()
        {
            var sqlBuilder = new StringBuilder()
                .AppendLine("SELECT *")
                .AppendLine("FROM flow.product_jhl")
                .AppendLine("WHERE com_code = @com_code AND prod_cd LIKE @prod_cd AND prod_nm LIKE @prod_nm")
                .AppendLine("ORDER BY write_dt");

            var sql = sqlBuilder.ToString();

            var parameters = new Dictionary<string, object>()
            {
                {"@com_code", $"{Input.COM_CODE}" },
                {"@prod_cd",  $"%{Input.PROD_CD}%" },
                {"@prod_nm",  $"%{Input.PROD_NM}%" }
            };

            var dbManager = new DbManager();
            var prdList = dbManager.Query<Product>(sql, parameters, (reader, data) => {
                data.Key.COM_CODE = reader["com_code"].ToString();
                data.Key.PROD_CD = reader["prod_cd"].ToString();
                data.PRICE = (int)reader["price"];
                data.PROD_NM = reader["prod_nm"].ToString();
                data.WRITE_DT = (DateTime)reader["write_dt"];
            });

            if (Input.prodNmOrd > 0) {
                prdList.OrderBy(prd => prd.PROD_NM);
            } else if (Input.prodNmOrd < 0) {
                prdList.OrderByDescending(prd => prd.PROD_NM);
            }

            Output = prdList;
        }
        protected override void Executed() { }
    }
}