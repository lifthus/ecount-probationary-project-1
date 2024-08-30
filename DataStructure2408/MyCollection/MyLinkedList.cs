using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCollection
{
    public class LinkedNode<T>
    {
        public T Data;
        public LinkedNode<T> Prev;
        public LinkedNode<T> Next;

        public LinkedNode(T data)
        {
            this.Data = data;
        }

        public LinkedNode(T data, LinkedNode<T> prev, LinkedNode<T> next)
        {
            this.Data = data;
            this.Prev = prev;
            this.Next = next;
        }
    }

    public class MyLinkedList<T> : IEnumerable<T>
    {
        private LinkedNode<T> _head;
        private LinkedNode<T> _tail;
        private IEqualityComparer<T> _equalityComparer;
        private int _size; // 현재 저장된 원소 개수

        public MyLinkedList(IEqualityComparer<T> equalityComparer = null)
        {
            this._equalityComparer = equalityComparer ?? EqualityComparer<T>.Default;
        }

        // PROPERTIES
        //_________________________________________________________________________________________

        public int Count {
            get { return _size; }
        }

        public LinkedNode<T> First {
            get { return _head; }
        }

        public LinkedNode<T> Last {
            get { return _tail; }
        }

        // METHODS
        //_________________________________________________________________________________________

        public void AddFirst(T data)
        {
            LinkedNode<T> newNode = new LinkedNode<T>(data);
            if (_head != null) {
                // TODO... AddLast 로직 반대로
                _head.Prev = newNode;
                newNode.Next = _head;
            } else {
                _tail = newNode;
            }

            _head = newNode;
            _size++;
        }

        public void AddLast(T data)
        {
            LinkedNode<T> newNode = new LinkedNode<T>(data);
            if (_tail != null) {
                _tail.Next = newNode;
                newNode.Prev = _tail;
            } else {
                // TODO... AddFirst 로직 반대로
                _head = newNode;
            }

            // TODO... AddFirst 로직 반대로
            _tail = newNode;
            _size++;
        }

        public void Clear()
        {
            _head = null;
            _tail = null;
            _size = 0;
        }

        public T RemoveFirst()
        {
            T result = default(T);

            if (_head != null) {
                result = _head.Data;
                Remove(_head);
            }

            return result;
        }

        public T RemoveLast()
        {
            T result = default(T);

            if (_tail != null) {
                //TODO: tail이 가지고 있는 값을 result에 설정한 후 tail 노드를 삭제.
                result = _tail.Data;
                Remove(_tail);
            }

            return result;
        }

        public bool Remove(T data)
        {
            var currNode = _head;
            while (currNode != null) {
                if (_equalityComparer.Equals(currNode.Data, data)) {
                    Remove(currNode);
                    return true;
                }
                currNode = currNode.Next;
            }
            return false;
        }

        public void Remove(LinkedNode<T> node)
        {
            if (node == _head) {
                _head = node.Next;
                if (_head != null) {
                    _head.Prev = null;
                }
            } else {
                // TODO... tail 로직의 반대로
                node.Prev.Next = node.Next;
            }

            if (node == _tail) {
                // TODO... head 로직의 반대로
                _tail = node.Prev;
                if (_tail != null) {
                    _tail.Next = null;
                }
            } else {
                node.Next.Prev = node.Prev;
            }

            this._size--;
        }

        public LinkedNode<T> Find(T data)
        {
            for (var currNode = _head; currNode != null; currNode = currNode.Next ) {
                //TODO: comparer로 비교하여 같다면 현재 노드를 리턴
                if (_equalityComparer.Equals(data, currNode.Data)) {
                    return currNode;
                }
            }
            return null;
        }

        public LinkedNode<T> FindLast(T data)
        {
            for (var currNode = _tail; currNode != null; currNode = currNode.Prev ) {
                //TODO: comparer로 비교하여 같다면 현재 노드를 리턴
                if (_equalityComparer.Equals(data, currNode.Data)) {
                    return currNode;
                }
            }
            return null;
        }

        public T[] ToArray()
        {
            T[] arr = new T[_size];
            var curNode = _head;
            for (int i = 0; i < _size; i++, curNode = curNode.Next) {
                arr[i] = curNode.Data;
            }
            return arr;
        }

        public bool Contains(T data)
        {
            var curNode = _head;
            while (curNode != null) {
                if (_equalityComparer.Equals(curNode.Data, data)) {
                    return true;
                }
                curNode = curNode.Next;
            }
            return false;
        }

        // TODO...
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<T> GetEnumerator ()
        {
            return new MyLinkedListEnumerator(this);
        }

        private class MyLinkedListEnumerator : IEnumerator<T>
        {
            private MyLinkedList<T> _list;
            private LinkedNode<T> _node;
            private T _current;

            public MyLinkedListEnumerator(MyLinkedList<T> list)
            {
                _list = list;
                _node = list.First;
                _current = default(T);
            }

            object IEnumerator.Current {
                get {
                    return Current;
                }
            }
            public T Current {
                get {
                    return _current;
                }
            }

            public bool MoveNext()
            {
                if (_node != null) {
                    _current = _node.Data;
                    _node = _node.Next;
                    return true;
                }
                return false;
            }

            public void Reset()
            {
                _current = default(T);
                _node = _list.First;
            }

            public void Dispose()
            {

            }
        }
    }

    public static class MyLinkedListExtension
    {
        public static bool Contains<T>(this MyLinkedList<T> list, Predicate<LinkedNode<T>> match)
        {
            var curNode = list.First;
            while (curNode != null) {
                if (match(curNode)) {
                    return true;
                }
                curNode = curNode.Next;
            }
            return false;
        }

        public static LinkedNode<T> Find<T>(this MyLinkedList<T> list, Predicate<LinkedNode<T>> match)
        {
            var curNode = list.First;
            while (curNode != null) {
                if (match(curNode)) {
                    return curNode;
                }
                curNode = curNode.Next;
            }
            return null;
        }

        public static LinkedNode<T> FindLast<T>(this MyLinkedList<T> list, Predicate<LinkedNode<T>> match)
        {
            var curNode = list.Last;
            while (curNode != null) {
                if (match(curNode)) {
                    return curNode;
                }
                curNode = curNode.Prev;
            }
            return null;
        }

        public static bool Remove<T>(this MyLinkedList<T> list, Predicate<LinkedNode<T>> match)
        {
            var curNode = list.First;
            while (curNode != null) {
                if (match(curNode)) {
                    list.Remove(curNode);
                    return true;
                }
                curNode = curNode.Next;
            }
            return false;

        }
    }
}
