using System;

namespace command_proj1
{
    public class UpdateSaleCommandInput
    {
        public SaleKey Key { get; set; }
        public string PROD_NM { get; set; }
        public decimal PRICE { get; set; }
    }

    public class UpdateSaleCommand : Command<Sale>
    {
        public UpdateSaleCommandInput Input { get; set; }

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
            Sale targetSale = null;
            _pipeLine.Register<GetSaleDAC, Sale>(new GetSaleDAC())
               .Mapping(cmd => {
                  
               })
               .Executed(res => {
                   if (res.Output == null || res.Errors.Count > 0) {
                       throw new InexecutableCommandError($"품목 등록 여부 검증 실패");
                   }
                   targetSale = res.Output;
               });
            _pipeLine.Register<UpdateSaleDAC, int>(new UpdateSaleDAC())
                .AddFilter(cmd => {
                    if (targetSale == null) {
                        return false;
                    }
                    return true;
                })
                .Mapping(cmd => {
                    
                    cmd.Input = targetSale;
                })
                .Executed(res => {
                    if (res.HasError()) {
                        throw new Exception(Errors[0].Message);
                    }
                });
            _pipeLine.Register<GetSaleDAC, Sale>(new GetSaleDAC())
                .Mapping(cmd => {
                   
                })
                .Executed(res => {
                    if (res.Errors.Count > 0) {
                        throw Errors[0];
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
