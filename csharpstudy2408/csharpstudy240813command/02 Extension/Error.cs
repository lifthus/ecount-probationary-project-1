using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csharpstudy240813command
{
    public static class ErrorExt
    {
        public static void vAddError<TOut>(this Command<TOut> cmd, Error err)
        {
            cmd.Errors.Add(err);
        }
    }
}
