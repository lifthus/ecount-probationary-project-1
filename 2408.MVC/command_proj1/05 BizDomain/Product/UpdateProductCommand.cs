using System;

namespace command_proj1
{
    public class UpdateProductCommandInput
    {
        public ProductKey Key { get; set; }
        public string PROD_NM { get; set; }
        public decimal PRICE { get; set; }
    }

    public class UpdateProductCommand : Command<Product>
    {
        public UpdateProductCommandInput Input { get; set; }

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
            Product targetProduct = null;
            _pipeLine.Register<GetProductDAC, Product>(new GetProductDAC())
               .Mapping(cmd => {
                   cmd.Input = new GetProductDACRequestDTO(Input.Key.COM_CODE, Input.Key.PROD_CD);
               })
               .Executed(res => {
                   if (res.Output == null || res.Errors.Count > 0) {
                       throw new InexecutableCommandError($"품목 등록 여부 검증 실패");
                   }
                   targetProduct = res.Output;
               });
            _pipeLine.Register<UpdateProductDAC, int>(new UpdateProductDAC())
                .AddFilter(cmd => {
                    if (targetProduct == null) {
                        return false;
                    }
                    return true;
                })
                .Mapping(cmd => {
                    targetProduct.PROD_NM = Input.PROD_NM;
                    targetProduct.PRICE = Input.PRICE;
                    cmd.Input = targetProduct;
                })
                .Executed(res => {
                    if (res.HasError()) {
                        throw new Exception(Errors[0].Message);
                    }
                });
            _pipeLine.Register<GetProductDAC, Product>(new GetProductDAC())
                .Mapping(cmd => {
                    cmd.Input = new GetProductDACRequestDTO(Input.Key.COM_CODE, Input.Key.PROD_CD);
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
