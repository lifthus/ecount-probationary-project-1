using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace command_proj1
{
    public class SelectSaleCommand : Command<SelectSaleDACResponseDTO>
    {
        public SelectSaleDACRequestDTO Input { get; set; }

        private PipeLine _pipeLine = new PipeLine();

        protected override void Init()
        {
        }
        protected override void CanExecute()
        {
            if (Input == null)
            {
                throw new InexecutableCommandError("품목 조회 커맨드 입력 없음");
            }
        }
        protected override void OnExecuting()
        {
        }
        protected override void ExecuteCore()
        {
            _pipeLine.Register<SelectSaleDAC, SelectSaleDACResponseDTO>(new SelectSaleDAC())
               .Mapping(cmd => {
                   cmd.Input = Input;
               })
               .Executed(res => {
                   if (res.HasError())
                   {
                       throw new InexecutableCommandError($"품목 쿼리 실패: {res.Errors[0].Message}");
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
