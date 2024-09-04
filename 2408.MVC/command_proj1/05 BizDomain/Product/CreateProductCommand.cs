using System;

namespace command_proj1
{
    public class CreateProductCommandInput
    {
        public ProductKey Key { get; set; }
        public string PROD_NM { get; set; }
        public decimal PRICE { get; set; }
        public bool ACTIVE { get; set; }
    }

    public class CreateProductCommand : Command<Product>
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
            // 판매 조회 등에서 구분자로 사용할 문자를 포함하고 있으면 생성 불가
            if (Input.Key.PROD_CD.Contains(",")) {
                throw new InexecutableCommandError("품목 코드에 구분자를 포함할 수 없음");
            }
        }
        protected override void OnExecuting()
        {
        }
        protected override void ExecuteCore()
        {
            _pipeLine.Register<GetProductDAC, Product>(new GetProductDAC())
               .Mapping(cmd => {
                   cmd.Input = new GetProductDACRequestDTO(Input.Key.COM_CODE, Input.Key.PROD_CD);
               })
               .Executed(res => {
                   if (res.Output != null)
                   {
                       throw new InexecutableCommandError($"품목 중복 키 존재");
                   }
                   if (res.HasError()) {
                       throw new InexecutableCommandError($"품목 등록 여부 검증 실패: {res.Errors[0].Message}");
                   }
               });
            _pipeLine.Register<InsertProductDAC, int>(new InsertProductDAC())
                .Mapping(cmd => {
                    var newProd = new Product();
                    newProd.Key.COM_CODE = Input.Key.COM_CODE;
                    newProd.Key.PROD_CD = Input.Key.PROD_CD;
                    newProd.PROD_NM = Input.PROD_NM;
                    newProd.PRICE = Input.PRICE;
                    newProd.ACTIVE = Input.ACTIVE;
                    cmd.Input = newProd;
                })
                .Executed(res => {
                    if (res.HasError())
                    {
                        throw res.Errors[0];
                    }
                });
            // 생성 후 엔티티 조회하는 부분 까지 트랜잭션으로 묶였으면 좋겠다.
            _pipeLine.Register<GetProductDAC, Product>(new GetProductDAC())
                .Mapping(cmd => {
                    cmd.Input = new GetProductDACRequestDTO(Input.Key.COM_CODE, Input.Key.PROD_CD);
                })
                .Executed(res => {
                    if (res.HasError()) {
                        throw res.Errors[0];
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
