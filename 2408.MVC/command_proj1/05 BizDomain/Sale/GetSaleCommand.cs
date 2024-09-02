using System;
using static command_proj1.GetSaleDAC;

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
                throw new InexecutableCommandError("판매 Get 커맨드 입력 없음");
            }
            if (Input.Key == null || Input.Key.COM_CODE == null || Input.Key.IO_DATE == null) {
                throw new InexecutableCommandError("Get 대상 판매 COM_CODE, IO_DATE 필요");
            }
        }
        protected override void OnExecuting()
        {
        }
        protected override void ExecuteCore()
        {
            _pipeLine.Register<GetSaleDAC, Sale>(new GetSaleDAC())
                .Mapping(cmd => {
                    cmd.Input = new GetSaleDACRequestDTO()
                    {
                        COM_CODE = Input.Key.COM_CODE,
                        IO_DATE = Input.Key.IO_DATE,
                        IO_NO = Input.Key.IO_NO,
                    };
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
