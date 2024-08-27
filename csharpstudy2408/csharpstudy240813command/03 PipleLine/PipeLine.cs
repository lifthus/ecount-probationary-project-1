using csharpstudy240813command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csharpstudy240813command
{
    public class PipeLine: IPipeLineItem
    {
        // 자료구조를 고민해보세요.
        private readonly Queue<IPipeLineItem> Items = new Queue<IPipeLineItem>();

        public PipeLineItem<TCmd, TOut> Register<TCmd, TOut>(TCmd command)
            where TCmd : Command<TOut>
        {
            // Command 를 PipeLine 에 등록하는 동작
            var newPipeLineItem = new PipeLineItem<TCmd, TOut>(command);
            Items.Enqueue(newPipeLineItem);
            return newPipeLineItem;
        }

        public void Execute()
        {
            while (Items.Count > 0)
            {
                    var item = Items.Dequeue();
                    item.Execute();
            }
        }
    }
}
