using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csharpstudy240813command
{
    public class InexecutableCommandError : Error
    {
        public InexecutableCommandError(string message) : base("INEXECUTABLE", message) { }
    }
}
