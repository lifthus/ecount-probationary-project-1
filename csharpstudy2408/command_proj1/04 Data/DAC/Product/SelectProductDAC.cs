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
        public class EqualFilter {
            public string COM_CODE;
        }

        public class LikeFilter
        {
            public string PROD_CD;
            public string PROD_NM;
        }

        public class OrderFilter
        {
            public int PROD_NM;
        }

        public EqualFilter Equal;
        public LikeFilter Like;
        public OrderFilter OrderBy;
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
            if (Input.Equal == null || Input.Equal.COM_CODE == null) {
                throw new InexecutableCommandError("회사 코드 필요");
            }
        }
        protected override void OnExecuting() {
            // LIKE 필터 설정
            if (Input.Like == null) {
                Input.Like = new SelectProductDACRequestDTO.LikeFilter();
            }
            if (Input.Like.PROD_CD == null) {
                Input.Like.PROD_CD = "";
            }
            if (Input.Like.PROD_NM == null) {
                Input.Like.PROD_NM = "";
            }

            // ORDER 필터 설정
            if (Input.OrderBy == null) {
                Input.OrderBy = new SelectProductDACRequestDTO.OrderFilter();
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
                {"@com_code", $"{Input.Equal.COM_CODE}" },
                {"@prod_cd",  $"%{Input.Like.PROD_CD}%" },
                {"@prod_nm",  $"%{Input.Like.PROD_NM}%" }
            };

            var dbManager = new DbManager();
            var prdList = dbManager.Query<Product>(sql, parameters, (reader, data) =>
            {
                data.Key.COM_CODE = reader["com_code"].ToString();
                data.Key.PROD_CD = reader["prod_cd"].ToString();
                data.PRICE = (int)reader["price"];
                data.PROD_NM = reader["prod_nm"].ToString();
                data.WRITE_DT = (DateTime)reader["write_dt"];
            });

            if (Input.OrderBy.PROD_NM != 0) {
                prdList.OrderBy((a, b) => {
                    if (Input.OrderBy.PROD_NM > 0) {
                        return a.PROD_NM.CompareTo(b.PROD_NM);
                    }
                    return b.PROD_NM.CompareTo(a.PROD_NM);
                });
            }

            Output = prdList;
        }
        protected override void Executed() { }
    }
}
