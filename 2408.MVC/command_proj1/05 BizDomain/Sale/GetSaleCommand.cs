using System;

namespace command_proj1
{
    public class GetSaleCommandInput
    {
        private SaleKey _key;
        public SaleKey Key {
            get {
                if (_key == null) {
                    _key = new SaleKey();
                }
                return _key;
            }
            set {
                Key = value;
            }
        }
    }

    public class GetSaleCommand : Command<Sale>
    {
        public GetSaleCommandInput Input { get; set; }

        private PipeLine _pipeLine = new PipeLine();

        protected override void Init()
        {
        }
        protected override void CanExecute()
        {
            if (Input == null) {
                throw new InexecutableCommandError("품목 Get 커맨드 입력 없음");
            }
            if (Input.Key == null || Input.Key.COM_CODE == null || Input.Key.IO_DATE == null) {
                throw new InexecutableCommandError("Get 대상 품목 COM_CODE, PROD_CD 필요");
            }
        }
        protected override void OnExecuting()
        {
        }
        protected override void ExecuteCore()
        {
            _pipeLine.Register<GetSaleDAC, Sale>(new GetSaleDAC())
                .Mapping(cmd => {
                    cmd.Input = new GetSaleDACRequestDTO(Input.Key.COM_CODE, Input.Key.IO_DATE, Input.Key.IO_NO);
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
