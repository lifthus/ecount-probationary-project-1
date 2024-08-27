using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace command_proj1
{
    public class Entity<TKey> : IEntity<TKey>
        where TKey : IEntityKey, new()
    {
        private TKey _key;
        public TKey Key
        {
            get
            {
                if (_key == null) {
                    _key = new TKey();
                }
                return _key;
            }
            set
            {
                _key = value;
            }
        }
    }
}
