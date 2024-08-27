using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace command_proj1
{
    public interface IEntityKey
    {

    }

    public interface IEntity<TKey>
        where TKey : IEntityKey
    {
        TKey Key { get; }
    }
}
