using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace command_proj1
{
    public static class ErrorExt
    {
        public static void vAddError<TOut>(this Command<TOut> cmd, Error err) where TOut : new()
        {
            cmd.Errors.Add(err);
        }
    }
}
