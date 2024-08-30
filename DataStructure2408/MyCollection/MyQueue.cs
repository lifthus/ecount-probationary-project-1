using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCollection
{
    public class MyQueue<T> : IEnumerable<T>
    {
        private MyLinkedList<T> _list;

        public MyQueue()
        {
            _list = new MyLinkedList<T>();
        }

        public int Count {
            get { return _list.Count; }
        }

        public void Enqueue(T item)
        {
            _list.AddLast(item);
        }

        // TODO:...

        public T Dequeue()
        {
            if (Count == 0) {
                throw new InvalidOperationException();
            }
            return _list.RemoveFirst();
        }

        public T Peek()
        {
            if (Count == 0) {
                throw new InvalidOperationException();
            }
            return _list.First.Data;
        }

        public T[] ToArray()
        {
            return _list.ToArray();
        }

        public void Clear()
        {
            _list.Clear();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _list.GetEnumerator();
        }
    }

}
