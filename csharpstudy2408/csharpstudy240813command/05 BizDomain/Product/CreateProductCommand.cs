using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csharpstudy240813command
{
    public class CreateProductCommandInput
    {
        public string COM_CODE = "";
        public string PROD_CD = "";
        public string PROD_NM = "";
        public int PRICE = 0;
    }

    public class CreateProductCommand : Command<int>
    {
        public CreateProductCommandInput Input { get; set; }

        private PipeLine _pipeLine = new PipeLine();

        protected override void Init()
        {
        }
        protected override void CanExecute()
        {
            if (Input == null) {
                throw new InexecutableCommandError("품목 생성 커맨드 입력 없음");
            }
        }
        protected override void OnExecuting()
        {
        }
        protected override void ExecuteCore()
        {
            _pipeLine.Register<GetProductDAC, Product?>(new GetProductDAC())
               .Mapping(cmd => {
                   cmd.Input = new GetProductDACRequestDTO(Input.COM_CODE, Input.PROD_CD);
               })
               .Executed(res => {
                   if (res.Output != null || res.Errors.Count() > 0) {
                       throw new InexecutableCommandError("품목 등록 여부 검증 실패");
                   }
               });
            _pipeLine.Register<InsertProductDAC, int>(new InsertProductDAC())
                .Mapping(cmd => {
                    var newProd = new Product();
                    newProd.Key.COM_CODE = Input.COM_CODE;
                    newProd.Key.PROD_CD = Input.PROD_CD;
                    newProd.PROD_NM = Input.PROD_NM;
                    newProd.PRICE = Input.PRICE;
                    cmd.Input = newProd;
                })
                .Executed(res => {
                    if (res.Errors.Count() > 0) {
                        throw new Exception(Errors[0].Message);
                    }
                    Output = res.Output;
                });
            _pipeLine.Execute();
        }
        protected override void Executed()
        {
        }

    }
}
