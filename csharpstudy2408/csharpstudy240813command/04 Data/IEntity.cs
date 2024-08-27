﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csharpstudy240813command
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
