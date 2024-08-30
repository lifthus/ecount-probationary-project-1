using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCollection
{
    public class MyHashMap<TValue> : IEnumerable<KeyValuePair<string, MyList<TValue>>>
    {
        public const int DEFAULT_BUCKET_SIZE = 3;

        private MyDictionary<string, MyList<TValue>> _dict;
        private MyList<string> _keyList;


        public MyHashMap(IEqualityComparer<string> equalityComparer = null)
            : this(DEFAULT_BUCKET_SIZE, equalityComparer)
        {
        }

        public MyHashMap(int capacity, IEqualityComparer<string> equalityComparer = null)
        {
            var comparer = equalityComparer ?? StringComparer.OrdinalIgnoreCase;    // 대소문자 구분없이 비교하는 비교자를 기본으로 사용한다.
            this._dict = new MyDictionary<string, MyList<TValue>>(capacity, comparer);
            this._keyList = new MyList<string>(capacity, comparer);    // 추가되는 키를 순서대로 저장 할 용도의 리스트 객체
        }

        public int Count {
            get { return _keyList.Count; }
        }

        public TValue this[int index] {
            get {
                return GetValue(_keyList[index]); //키목록 리스트에서 키를 찾아서 GetValue 호출;
            }
            set {
                SetValue(_keyList[index], value); // 키목록 리스트에서 키를 찾아서 SetValue 호출;
            }
        }

        public TValue this[string key] {
            get { return GetValue(key); }
            set { SetValue(key, value); }
        }

        public IEnumerable<string> Keys {
            get { return new MyMapKeyCollection(this); }
        }

        public TValue[] GetAllValues()
        {
            MyList<TValue> values = new MyList<TValue>();

            foreach (string key in this._keyList) {
                // TODO            
                foreach (TValue v in _dict[key]) {
                    values.Add(v);
                }
            }
            return values.ToArray();
        }

        public TValue[] GetValues(string key)
        {
            var list = _dict.GetValue(key, false);
            if (list == null) {
                return Array.Empty<TValue>();
            }
            return list.ToArray();
        }

        protected TValue GetValue(string key)
        {
            var list = _dict.GetValue(key, false);
            if (list == null) {
                return default(TValue);
            }

            return list[0]; // 첫번째 요소를 리턴한다.
        }

        protected void SetValue(string key, TValue value)
        {
            var list = _dict.GetValue(key, false); // TODO
            if (list == null) {
                // 리스트 새로 생성
                // 딕셔너리의 해당 key에 새로 생성한 리스트 설정
                // 키 목록 리스트에 key 추가
                list = new MyList<TValue>();
                _dict.Add(key, list);
                _keyList.Add(key);
            }

            // 리스트에 값 추가
            list.Add(value);
        }

        public void Add(string key, TValue value)
        {
            SetValue(key, value);
        }

        public bool Remove(string key)
        {
            // 딕셔너리에서 삭제가 성공하면 키 목록 리스트에서도 삭제 후 true 리턴
            if (_dict.Remove(key)) {
                return _keyList.Remove(key);
            }

            return false;
        }

        public void Clear()
        {
            _dict = new MyDictionary<string, MyList<TValue>>(DEFAULT_BUCKET_SIZE);
            _keyList = new MyList<string>();
        }

        public bool Contains(string key)
        {
            var list = _dict.GetValue(key, false);
            if (list != null && list.Count > 0) {
                return true;
            }

            return false;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<KeyValuePair<string, MyList<TValue>>> GetEnumerator()
        {
            // 값이 추가된 순서대로 순회되지 않는다.
            //return _dict.GetEnumerator();

            // 값이 추가된 순서대로 순회하려면 키 목를을 가지고 있는 _keyList를 Iteration해야 함.
            // TODO:
            return new MyMapEnumerator(this);
        }

        private abstract class MyMapEnumeratorBase<TCurrent> : IEnumerator<TCurrent>
        {
            protected MyHashMap<TValue> _hashMap;
            protected int _keyIndex;
            protected KeyValuePair<string, MyList<TValue>> _current;

            public MyMapEnumeratorBase(MyHashMap<TValue> hashMap)
            {
                _hashMap = hashMap;
                _keyIndex = 0;
            }

            public void Dispose() { }

            object IEnumerator.Current {
                get {
                    return Current;
                }
            }

            public abstract TCurrent Current { get; }

            public bool MoveNext()
            {
                if (_keyIndex < _hashMap._keyList.Count) {
                    var currentKey = _hashMap._keyList[_keyIndex++];
                    _current = new KeyValuePair<string, MyList<TValue>>(currentKey, _hashMap._dict[currentKey]);
                    return true;
                }
                return false;
            }

            public void Reset()
            {
                _keyIndex = 0;
                _current = default(KeyValuePair<string, MyList<TValue>>);
            }
        }

        private class MyMapEnumerator : MyMapEnumeratorBase<KeyValuePair<string, MyList<TValue>>>
        {
            public MyMapEnumerator(MyHashMap<TValue> hashMap) : base(hashMap) { }

            public override KeyValuePair<string, MyList<TValue>> Current {
                get {
                    return _current;
                }
            }
        }

        private abstract class MyMapCollectionBase<TCurrent> : IEnumerable<TCurrent>
        {
            protected MyHashMap<TValue> _hashMap;

            public MyMapCollectionBase(MyHashMap<TValue> hashMap)
            {
                _hashMap = hashMap;
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            public abstract IEnumerator<TCurrent> GetEnumerator();
        }

        private class MyMapKeyCollection : MyMapCollectionBase<string>
        {
            public MyMapKeyCollection(MyHashMap<TValue> hashMap) : base(hashMap) { }

            public override IEnumerator<string> GetEnumerator()
            {
                return _hashMap._keyList.GetEnumerator();
            }
        }
    }

}
