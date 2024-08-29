using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;

namespace MyCollection
{
    public class MyDictionary<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue>>
    {
        public const int DEFAULT_BUCKET_SIZE = 3;

        private MyLinkedList<KeyValuePair<TKey, TValue>>[] _bucket;
        private IEqualityComparer<TKey> _equalityComparer;
        private int _count;

        private class KeyEqComparer : IEqualityComparer<KeyValuePair<TKey, TValue>>
        {
            private IEqualityComparer<TKey> _equalityComparer;

            public KeyEqComparer(IEqualityComparer<TKey> equalityComparer)
            {
                _equalityComparer = equalityComparer;
            }

            public bool Equals(KeyValuePair<TKey, TValue> a, KeyValuePair<TKey, TValue> b)
            {
                return _equalityComparer.Equals(a.Key, b.Key);
            }

            public int GetHashCode(KeyValuePair<TKey, TValue> a)
            {
                return a.GetHashCode() & 0x7fffffff;
            }
        }

        public MyDictionary(IEqualityComparer<TKey> equalityComparer = null)
            : this(DEFAULT_BUCKET_SIZE, equalityComparer)
        {
        }

        public MyDictionary(int capacity, IEqualityComparer<TKey> equalityComparer = null)
        {
            int size = HashHelpers.GetPrime(capacity);
            this._bucket = new MyLinkedList<KeyValuePair<TKey, TValue>>[size];
            this._equalityComparer = equalityComparer ?? EqualityComparer<TKey>.Default;
        }


        // PROPERTIES
        //_________________________________________________________________________________________

        public int Count {
            get { return _count; }
        }

        public TValue this[TKey key] {
            get { return GetValue(key, true); }
            set { SetValue(key, value, false); }
        }


        // METHODS
        //_________________________________________________________________________________________

        internal int GetBucketIndex(TKey key, int bucketSize)
        {
            int hash = key.GetHashCode() & 0x7fffffff; // TODO:EqualityComparer를 이용하여 item을 해싱한 해쉬코드와 버킷(배열)의 크기를 이용하여 해당 인덱스를 구한다.
            return hash % bucketSize;
        }

        // 같은 어셈블리(DLL 또는 EXE) 안에 있는 다른 클래스에서만 공개 메소드로 사용 할 수 있도록 접근 제어자를 internal로 정의한다.
        // HashMap 구현시 해당 메소드를 호출하여 사용 할 예정
        internal MyLinkedList<KeyValuePair<TKey, TValue>> FindBucketList(TKey key)
        {
            int index = GetBucketIndex(key, _bucket.Length);
            return _bucket[index];
        }

        internal LinkedNode<KeyValuePair<TKey, TValue>> FindEntry(TKey key)
        {
            var list = FindBucketList(key);
            if (list != null) {
                return list.Find((n) => _equalityComparer.Equals(n.Data.Key, key));
            }
            return null;
        }

        // 같은 어셈블리(DLL 또는 EXE) 안에 있는 다른 클래스에서만 공개 메소드로 사용 할 수 있도록 접근 제어자를 internal로 정의한다.
        // HashMap 구현시 해당 메소드를 호출하여 사용 할 예정
        internal TValue GetValue(TKey key, bool raiseError)
        {
            var node = FindEntry(key);
            if (node == null) {
                if (raiseError) {
                    throw new ArgumentException("The key doesn't exist in the Dictionary.", key.ToString());
                }
                return default(TValue);
            }
            return node.Data.Value;
        }

        // 같은 어셈블리(DLL 또는 EXE) 안에 있는 다른 클래스에서만 공개 메소드로 사용 할 수 있도록 접근 제어자를 internal로 정의한다.
        // HashMap 구현시 해당 메소드를 호출하여 사용 할 예정
        internal bool SetValue(TKey key, TValue value, bool raiseError)
        {
            // 현재 데이터 개수가 해시 버킷 개수의 125% 가 넘으면 리사이징한다.
            if (_count >= _bucket.Length * HashHelpers.RESIZE_FACTOR) {
                Resize(_bucket.Length * HashHelpers.PRIME_FACTOR);
            }

            int index = GetBucketIndex(key, _bucket.Length);
            var list = _bucket[index];

            if (list == null) {
                // TODO: 해당 버킷에 이미 만들어진 연결리스트가 없다면 새로 만들고 버킷에 할당한다.
                _bucket[index] = new MyLinkedList<KeyValuePair<TKey, TValue>>();
                list = _bucket[index];
            } else {
                var node = FindEntry(key); // TODO: EqualityComparer를 이용하여 list에 key와 같은 중복된 항목이 있는지 찾는다.
                if (node != null) { // 중복된 값이 있는 경우
                    if (raiseError) {
                        throw new ArgumentException("An element with the same key already exists in the Dictionary.", key.ToString());
                    }

                    // 기존에 저장되어 있던 값을 새로 설정되는 값으로 변경한다.
                    node.Data = new KeyValuePair<TKey, TValue>(key, value);
                    return false;
                }
            }

            // TODO: 연결리스트의 마지막에 해당 항목을 추가하고 카운트값을 하나 늘린다.
            list.AddLast(new KeyValuePair<TKey, TValue>(key, value));
            _count++;
            return true;
        }

        private void Resize(int capacity)
        {
            // 새로운 크기로 배열 새로 할당
            var newSize = HashHelpers.GetPrime(capacity);
            var newBucket = new MyLinkedList<KeyValuePair<TKey, TValue>>[newSize];

            // 기존 버킷배열에 저장되어 있는 연결리스트 항목을 순회한다.(루프)
            for (int i = 0; i < _bucket.Length; i++) { // TODO...
                var list = _bucket[i]; // TODO...
                if (list != null) {
                    foreach (var item in list) {
                        // TODO: 현재 항목을 바뀐 배열 크기로 재해싱하여 버킷의 인덱스를 구한다.
                        // 해당 버킷에 이미 만들어진 연결리스트가 없다면 새로 만들고 버킷에 할당한다.
                        // 연결리스트에 현재 항목을 추가한다.
                        var newIndex = GetBucketIndex(item.Key, newSize);
                        if (newBucket[newIndex] == null) {
                            newBucket[newIndex] = new MyLinkedList<KeyValuePair<TKey, TValue>>();
                        }
                        newBucket[newIndex].AddLast(item);
                    }
                }
            }
        }

        public void Add(TKey key, TValue value)
        {
            // SetValue 호출하는 방식으로 재활용
            SetValue(key, value, false);
        }

        public bool Remove(TKey key)
        {
            var list = FindBucketList(key);
            if (list != null) {
                // TODO: 연결리스트에서 해당 항목을 찾은 후 있다면
                // 해당 노드를 연결리스트에서 삭제하고 카운트값을 하나 줄인 후 true 리턴.
                var node = FindEntry(key);
                if (node == null) {
                    return false;
                }
                list.Remove(node);
                _count--;
                return true;
            }

            return false;
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            var node = FindEntry(key); // TODO: 찾고자 하는 키가 저장된 LinkedNode를 찾는다
            if (node != null) {
                value = node.Data.Value;
                return true;
            }

            value = default(TValue);
            return false;
        }

        public bool Contains(TKey key)
        {
            var node = FindEntry(key);
            if (node != null) {
                return true;
            }
            return false;
        }

        public void Clear()
        {
            _bucket = new MyLinkedList<KeyValuePair<TKey, TValue>>[DEFAULT_BUCKET_SIZE];
            _count = 0;
        }

        // IEnumerable 인터페이스 구현
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return new MyDictEnumerator(this);
        }

        public IEnumerable<TKey> Keys {
            get { return new MyDictKeyCollection(this); }
        }

        public IEnumerable<TValue> Values {
            get { return new MyDictValueCollection(this); }
        }

        // NESTED Helper Class
        //_________________________________________________________________________________________

        // IEnumerator<T>를 구현한 추상 클래스
        private abstract class MyDictEnumeratorBase<TCurrent> : IEnumerator<TCurrent>
        {
            protected MyDictionary<TKey, TValue> _dict;
            protected IEnumerator<KeyValuePair<TKey, TValue>> _iterator;
            protected int _index;

            public MyDictEnumeratorBase(MyDictionary<TKey, TValue> dict)
            {
                this._dict = dict;
                this._index = 0;
                this._iterator = FindNextEnumerator();
            }

            protected IEnumerator<KeyValuePair<TKey, TValue>> FindNextEnumerator()
            {
                // TODO: 현재 인덱스가 딕셔너리의 버킷배열의 크기보다 작을때까지 반복한다.
                // 버킷배열에 할당된 연결리스트를 가져온 후 현재 인덱스를 하나 증가시킨다.
                // 연결리스트가 존재하고 리스트에 추가되어 있는 항목의 갯수가 0보다 크다면
                // 연결리스트의 GetEnumerator() 결과를 리턴한다.
                while (_index < _dict._bucket.Length) {
                    var list = _dict._bucket[_index++];
                    if (list != null && list.Count > 0) {
                        return list.GetEnumerator();
                    }
                }

                return null;
            }

            // IDispose
            //_________________________________________________________________________________________
            public void Dispose()
            {
            }

            // IEnumerator
            //_________________________________________________________________________________________
            object IEnumerator.Current {
                get { return this.Current; }
            }

            // IEnumerator<T>
            //_________________________________________________________________________________________

            public abstract TCurrent Current { get; }

            public bool MoveNext()
            {
                // _iterator가 null이 아니고 _iterator의 MoveNext() 결과값이 false 일때까지
                // FindNextEnumerator를 호출하여 다음 버킷에 있는 연결리스트를 찾는다.
                while (_iterator != null && !_iterator.MoveNext()) {
                    //
                    _iterator = FindNextEnumerator();
                }

                return _iterator != null;
            }

            // TODO:...
            public void Reset()
            {
                _index = 0;
                _iterator = FindNextEnumerator();
            }
        }

        private class MyDictEnumerator : MyDictEnumeratorBase<KeyValuePair<TKey, TValue>>
        {
            public override KeyValuePair<TKey, TValue> Current {
                get {
                    return _iterator.Current;
                }
            }

            public MyDictEnumerator(MyDictionary<TKey, TValue> dict) : base(dict)
            {
                this._dict = dict;
                this._index = 0;
                this._iterator = FindNextEnumerator();
            }
        }

        // IEnumerable<T>를 구현한 추상 클래스
        private abstract class MyDictCollectionBase<TCurrent> : IEnumerable<TCurrent>
        {
            protected MyDictionary<TKey, TValue> _dict;

            protected MyDictCollectionBase(MyDictionary<TKey, TValue> dict)
            {
                this._dict = dict;
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
            }

            public abstract IEnumerator<TCurrent> GetEnumerator();

        }

        // 키 열거를 위한 객체를 MyDictCollectionBase 에서 상속하는 구조로 코드를 작성하시오
        private class MyDictKeyCollection : MyDictCollectionBase<TKey>
        {
            public MyDictKeyCollection(MyDictionary<TKey, TValue> dict)
                : base(dict)
            {
            }

            public override IEnumerator<TKey> GetEnumerator()
            {
                // TODO:
                return new MyDictKeyEnumerator(_dict);
            }

            private class MyDictKeyEnumerator : MyDictEnumeratorBase<TKey>
            {
                public MyDictKeyEnumerator(MyDictionary<TKey, TValue> dict)
                    : base(dict)
                {
                }

                // TODO: Current
                public override TKey Current {
                    get {
                        return _iterator.Current.Key;
                    }
                }
            }
        }

        // 값 열거를 위한 객체를 MyDictCollectionBase 에서 상속하는 구조로 코드를 작성하시오
        private class MyDictValueCollection : MyDictCollectionBase<TValue>
        {
            // TODO: MyDictKeyCollection 참고하여 구현
            public MyDictValueCollection(MyDictionary<TKey, TValue> dict) : base(dict)
            {
            }

            public override IEnumerator<TValue> GetEnumerator()
            {
                return new MyDictValueEnumerator(_dict);
            }

            private class MyDictValueEnumerator : MyDictEnumeratorBase<TValue>
            {
                public MyDictValueEnumerator(MyDictionary<TKey, TValue> dict) : base(dict)
                {

                }

                public override TValue Current {
                    get {
                        return _iterator.Current.Value;
                    }
                }
            }
        }
    }

}