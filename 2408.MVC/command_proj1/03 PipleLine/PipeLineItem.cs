using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace command_proj1
{
    public class PipeLineItem<TCmd, TOut> : IPipeLineItem 
        where TCmd: Command<TOut>
        where TOut: new()
    {
        private TCmd _command;
        //어떤 프로퍼티들이 추가로 필요할 지 고민해보기.

        public PipeLineItem(TCmd command)
        {
            // 생성자 로직 구현해보기
            _command = command;
        }

        private List<Action<TCmd>> _mappers = new List<Action<TCmd>>();
        private List<Predicate<TCmd>> _filters = new List<Predicate<TCmd>>();
        private List<Action<CommandResult<TOut>>> _executeds = new List<Action<CommandResult<TOut>>>();

        public PipeLineItem<TCmd, TOut> Mapping(Action<TCmd> mapping)
        {
            // 등록된 Command 의 Request 값 연결하는 동작
            _mappers.Add(mapping);
            return this;
        }

        /// <summary>
        /// 커맨드 실행 여부 (파이프라인 뒤의 커맨드는 계속 실행)
        /// </summary>
        public PipeLineItem<TCmd, TOut> AddFilter(Predicate<TCmd> filter)
        {
            _filters.Add(filter);
            return this;
        }

        public PipeLineItem<TCmd, TOut> Executed(Action<CommandResult<TOut>> executed)
        {
            // Command 가 실행된 이후, 결과에 추가 작업하는 동작
            _executeds.Add(executed);
            return this;
        }

        private void OnExecute()
        {
            // 함수 실행 시 위 함수들을 어떻게 호출하면 좋을지 구현해보기
            _mappers.ForEach(mapping => mapping(_command));
            _filters.ForEach(filter =>
            {
                if (!filter(_command)) {
                    return;
                }
            });
            _command.Execute();
            if (_command.Result == null) {
                throw new Exception("커맨드 실행 실패");
            }
            _executeds.ForEach(executed => executed(_command.Result));
        }

        public void Execute()
        {
            OnExecute();
        }
    }
}
