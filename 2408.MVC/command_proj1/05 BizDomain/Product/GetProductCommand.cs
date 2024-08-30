using System;

namespace command_proj1
{
    public class GetProductCommandInput
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

    public class GetProductCommand : Command<Product>
    {
        public GetProductCommandInput Input { get; set; }

        private PipeLine _pipeLine = new PipeLine();

        protected override void Init()
        {
        }
        protected override void CanExecute()
        {
            if (Input == null) {
                throw new InexecutableCommandError("품목 Get 커맨드 입력 없음");
            }
            if (Input.Key == null || Input.Key.COM_CODE == null || Input.Key.PROD_CD == null) {
                throw new InexecutableCommandError("Get 대상 품목 COM_CODE, PROD_CD 필요");
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
