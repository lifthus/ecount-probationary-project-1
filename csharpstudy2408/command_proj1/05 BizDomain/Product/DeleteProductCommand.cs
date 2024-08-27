using System;

namespace command_proj1
{
    public class DeleteProductCommandInput
    {
        private ProductKey _key;
        public ProductKey Key {
            get {
                if (_key == null) {
                    _key = new ProductKey();
                }
                return _key;
            }
            set {
                Key = value;
            }
        }
    }

    public class DeleteProductCommand : Command<Product>
    {
        public DeleteProductCommandInput Input { get; set; }

        private PipeLine _pipeLine = new PipeLine();

        protected override void Init()
        {
        }
        protected override void CanExecute()
        {
            if (Input == null) {
                throw new InexecutableCommandError("품목 삭제 커맨드 입력 없음");
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
            _pipeLine.Register<DeleteProductDAC, int>(new DeleteProductDAC())
                .AddFilter(cmd => {
                    if (targetProduct == null) {
                        return false;
                    }
                    return true;
                })
                .Mapping(cmd => {
                    cmd.Input = targetProduct;
                })
                .Executed(res => {
                    if (res.HasError()) {
                        throw new Exception(Errors[0].Message);
                    }
                    Output = targetProduct;
                });

            _pipeLine.Execute();
        }
        protected override void Executed()
        {
        }

    }
}
