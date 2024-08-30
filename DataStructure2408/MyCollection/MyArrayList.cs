using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace MyCollection
{
    public class MyArrayList
    {
        public const int DEFAULT_SIZE = 4; // 기본 사이즈

        private object[] _array;   // 할당된 배열을 가리키는 참조변수
        private int _size;         // 현재 저장된 원소 개수

        // 생성자
        public MyArrayList()
            : this(DEFAULT_SIZE)
        {
        }

        public MyArrayList(int capacity)
        {
            this._size = 0;
            this._array = new object[capacity];
        }

        public int Count {
            get { return _size; }
        }

        public int Capacity {
            get { return _array.Length; }
            set {
                if (value <= Capacity)
                    throw new ArgumentOutOfRangeException();

                var newArray = new object[value];
                this.CopyTo(newArray);
                _array = newArray;
            }
        }

        // 외부에서 배열 요소에 접근을 위한 인덱서 프로퍼티
        public object this[int index] {
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
        public void Add(object element)
        {
            // 배열 공간 체크, 부족할 시 resize
            EnsureCapacity();

            // 원소 추가
            _array[_size] = element;
            _size++;
        }

        // 해당 위치에 원소 추가
        public void Insert(int index, object element)
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

        public bool Remove(object item)
        {
            for (int i = 0; i < _size; i++) {
                if (_array[i].Equals(item)) {
                    this.RemoveAt(i);
                    return true;
                }
            }
            return false;
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

        public object[] ToArray()
        {
            var newArray = new object[_size];
            this.CopyTo(newArray, 0);
            // Array.Copy(_array, 0, newArray, 0, _size);
            return newArray;
        }

        public void Swap(int i, int j)
        {
            if (i < 0 || j < 0 || i >= _size || j >= _size) {
                throw new ArgumentOutOfRangeException();
            }
            object tmpObj = _array[i];
            _array[i] = _array[j];
            _array[j] = tmpObj;
        }

        public void Clear()
        {
            // Array.Clear(_array, 0, _size);
            // 로 클리어해줄 수도 안 해줄 수도 있다. (이 자료구조에서 어차피 구 데이터에 직접적으로 접근 불가능해서 의미는 없을 것)
            _size = 0;
        }

        public bool Contains(object item)
        {
            for (int i = 0; i < Count; i++) {
                if (_array[i].Equals(item)) {
                    return true;
                }
            }
            return false;
        }

        public int IndexOf(object item)
        {
            return IndexOf(item, 0);
        }
        public int IndexOf(object item, int index)
        {
            return IndexOf(item, index, _size - index);
        }
        public int IndexOf(object item, int index, int count)
        {
            var maxRange = index + count - 1;
            if (maxRange >= _size) {
                throw new ArgumentOutOfRangeException();
            }
            for (int i = index; i <= maxRange; i++) {
                if (_array[i].Equals(item)) {
                    return i;
                }
            }
            return -1;
        }

        public int LastIndexOf(object item)
        {
            return LastIndexOf(item, _size-1);
        }
        public int LastIndexOf(object item, int index)
        {
            return LastIndexOf(item, index, index+1);
        }
        public int LastIndexOf(object item, int index, int count)
        {
            var minRange = index - count;
            if (minRange < -1) {
                throw new ArgumentOutOfRangeException();
            }
            for (int i = index; i > minRange; i--) {
                if (_array[i].Equals(item)) {
                    return i;
                }
            }
            return -1;
        }
    }
}
