using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace command_proj1
{
    public class CommandResult<TOut>
    {
        public readonly TOut Output;
        public readonly List<Error> Errors;

        public CommandResult(TOut output, List<Error> errors) 
        {
            this.Output = output;
            this.Errors = errors;
        }
        
        public bool HasError ()
        {
            return Errors.Count > 0;
        }
    }
}
