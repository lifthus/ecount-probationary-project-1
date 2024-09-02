using System;

namespace command_proj1
{
    public class CreateSaleCommandInput
    {
        public SaleKey Key { get; set; }
        public string PROD_NM { get; set; }
        public decimal PRICE { get; set; }
    }

    public class CreateSaleCommand : Command<Sale>
    {
        public CreateSaleCommandInput Input { get; set; }

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
            _pipeLine.Register<GetSaleDAC, Sale>(new GetSaleDAC())
               .Mapping(cmd => {
                   // cmd.Input = new GetSaleDACRequestDTO(Input.Key.COM_CODE, Input.Key.IO_DATE, Input.Key.IO_NO);
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
            _pipeLine.Register<InsertSaleDAC, int>(new InsertSaleDAC())
                .Mapping(cmd => {
                    var newProd = new Sale();
                    cmd.Input = newProd;
                })
                .Executed(res => {
                    if (res.HasError())
                    {
                        throw res.Errors[0];
                    }
                });
            // 생성 후 엔티티 조회하는 부분 까지 트랜잭션으로 묶였으면 좋겠다.
            _pipeLine.Register<GetSaleDAC, Sale>(new GetSaleDAC())
                .Mapping(cmd => {
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
