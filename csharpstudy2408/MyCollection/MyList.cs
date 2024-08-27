using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace MyCollection
{
    public class MyList<T> : IEnumerable<T>
    {
        public const int DEFAULT_SIZE = 4; // 기본 사이즈

        private T[] _array;   // 할당된 배열을 가리키는 참조변수
        private int _size;         // 현재 저장된 원소 개수

        private IEqualityComparer<T> _equalityComparer;

        // 생성자
        public MyList(IEqualityComparer<T> equalityComparer = null)
            : this(DEFAULT_SIZE, equalityComparer)
        {
        }

        public MyList(int capacity, IEqualityComparer<T> equalityComparer = null)
        {   
            this._size = 0;
            this._array = new T[capacity];
            this._equalityComparer = equalityComparer ?? EqualityComparer<T>.Default;
        }

        public int Count {
            get { return _size; }
        }

        public int Capacity {
            get { return _array.Length; }
            set {
                if (value <= Capacity)
                    throw new ArgumentOutOfRangeException();

                var newArray = new T[value];
                this.CopyTo(newArray);
                _array = newArray;
            }
        }

        // 외부에서 배열 요소에 접근을 위한 인덱서 프로퍼티
        public T this[int index] {
            get {
                if (index >= _size)
                    throw new IndexOutOfRangeException();
                return _array[index];
            }
            set {
                if (index >= _size)
                    throw new IndexOutOfRangeException();
                _array[index] = value;
            }
        }

        private void EnsureCapacity()
        {
            int capacity = _array.Length;
            if (_size >= capacity) {
                this.Capacity = capacity == 0 ? 4 : capacity * 2;
            }
        }

        // 배열의 마지막에 원소 추가
        public void Add(T element)
        {
            // 배열 공간 체크, 부족할 시 resize
            EnsureCapacity();

            // 원소 추가
            _array[_size] = element;
            _size++;
        }

        // 해당 위치에 원소 추가
        public void Insert(int index, T element)
        {
            // 배열 공간 체크, 부족할 시 resize
            EnsureCapacity();

            // 추가되려고 하는 위치부터 한칸씩 뒤로 데이터 이동
            for (int i = _size; i > index; i--) { // TODO
                _array[i] = _array[i - 1];
            }

            // 원소 추가
            _array[index] = element;
            _size++;
        }

        public bool Remove(T item)
        {
            var index = IndexOf(item);
            if (index == -1) {
                return false;
            }
            RemoveAt(index);
            return true;
        }

        // 해당 위치의 원소 삭제
        public void RemoveAt(int index)
        {
            // TODO...
            RemoveRange(index, 1);
        }

        public void RemoveRange(int index, int count)
        {
            if (index < 0 || index + count > _size) {
                throw new ArgumentOutOfRangeException();
            }

            _size -= count;
            // 삭제하려는 위치부터 한칸씩 앞으로 데이터 이동
            for (int i = index; i < _size; i++) { // TODO
                _array[i] = _array[i + count]; // TODO
            }
        }

        public void CopyTo(Array array)
        {
            CopyTo(array, 0);
        }

        public void CopyTo(Array array, int arrayIndex)
        {
            if (array.Length - arrayIndex < _size) {
                throw new ArgumentOutOfRangeException();
            }

            for (int i = 0; i < _size; i++) { // TODO
                // TODO: ...
                array.SetValue(_array[i], arrayIndex + i);
            }
        }

        public T[] ToArray()
        {
            var newArray = new T[_size];
            this.CopyTo(newArray, 0);
            // Array.Copy(_array, 0, newArray, 0, _size);
            return newArray;
        }

        public void Swap(int i, int j)
        {
            if (i < 0 || j < 0 || i >= _size || j >= _size) {
                throw new ArgumentOutOfRangeException();
            }
            T tmpObj = _array[i];
            _array[i] = _array[j];
            _array[j] = tmpObj;
        }

        public void Clear()
        {
            // Array.Clear(_array, 0, _size);
            // 로 클리어해줄 수도 안 해줄 수도 있다. (이 자료구조에서 어차피 구 데이터에 직접적으로 접근 불가능해서 의미는 없을 것)
            _size = 0;
        }

        public bool Contains(T item)
        {
            return IndexOf(item) != -1;
        }

        public int IndexOf(T item)
        {
            return IndexOf(item, 0);
        }
        public int IndexOf(T item, int index)
        {
            return IndexOf(item, index, _size - index);
        }
        public int IndexOf(T item, int index, int count)
        {
            var maxRange = index + count - 1;
            if (maxRange >= _size) {
                throw new ArgumentOutOfRangeException();
            }
            for (int i = index; i <= maxRange; i++) {
                if (_equalityComparer.Equals(_array[i], item)) {
                    return i;
                }
            }
            return -1;
        }

        public int LastIndexOf(T item)
        {
            return LastIndexOf(item, _size-1);
        }
        public int LastIndexOf(T item, int index)
        {
            return LastIndexOf(item, index, index+1);
        }
        public int LastIndexOf(T item, int index, int count)
        {
            var minRange = index - count;
            if (minRange < -1) {
                throw new ArgumentOutOfRangeException();
            }
            for (int i = index; i > minRange; i--) {
                if (_equalityComparer.Equals(_array[i], item)) {
                    return i;
                }
            }
            return -1;
        }

        /* ========================= ENUMERABLE ========================= */
        public IEnumerator<T> GetEnumerator()
        {
            return new MyListEnumerator(this);
        }

        // IEnumerable<T> 인터페이스는 IEnumerable에서 상속되었으므로 IEnumerable 인터페이스에 대한 구현도 해줘야 한다.
        // 파라메터가 동일한 중복된 이름의 GetEnumerator() 메소드가 2개 있을 수 없으므로 IEnumerable 인터페이스의 메소드임을 명시해준다.
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        // 내부에서만 사용가능하게 private 중첩 객체로 구현함.
        // 호출하는 쪽은 IEnumerator<T> 인터페이스를 사용하기 때문에 MyListEnumerator<T>를 밖으로 노출 할 이유가 없다.
        private class MyListEnumerator : IEnumerator<T>
        {
            private MyList<T> _list;
            private T _current;
            private int _index;

            public MyListEnumerator(MyList<T> list)
            {
                this._list = list;
                this._index = 0;
                this._current = default(T);
            }

            // TODO... IEnumerator, IDisposable 에서 상속되므로 해당 인터페이스에 정의된 내용을 모두 구현 해줘야 한다.
            object IEnumerator.Current {
                get { return _current; }
            }

            public T Current {
                get { return _current; }
            }

            public bool MoveNext()
            {
                if (_index < _list.Count) {
                    _current = _list[_index++];
                    return true;
                }
                return false;
            }

            public void Reset()
            {
                _index = 0;
                _current = default(T);
            }

            public void Dispose()
            {
                // nothing to do.
            }
        }

    }
}
