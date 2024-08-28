using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;

namespace MyCollection
{
    public class MyStack<T> : IEnumerable<T>
    {
        private MySLinkedList<T> _list;

        public MyStack()
        {
            _list = new MySLinkedList<T>();
        }

        public int Count {
            get { return _list.Count; }
        }

        public void Push(T item)
        {
            _list.AddFirst(item);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        // TODO:...

        public T Peek()
        {
            if (Count == 0) {
                throw new InvalidOperationException();
            }
            return _list.First.Data;
        }

        public T Pop()
        {
            if (Count == 0) {
                throw new InvalidOperationException();
            }
            return _list.RemoveFirst();
        }

        public T[] ToArray()
        {
            return _list.ToArray();
        }

        public void Clear()
        {
            _list.Clear();
        }
    }

}
