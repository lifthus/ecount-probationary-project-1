using MyCollection;
using NuGet.Frameworks;
using System.Collections;
using System.Security.Cryptography;

namespace DataStructureTest
{
    public class MyListTest
    {
        MyList<int> intMAL = new MyList<int>();
        MyList<int> intMAL2 = new MyList<int>();
        MyList<string> strMAL = new MyList<string>();
        MyList<object> myMAL = new MyList<object>();

        [SetUp]
        public void Setup()
        {
            intMAL = new MyList<int>();
            intMAL2 = new MyList<int>();
            strMAL = new MyList<string>();
            myMAL = new MyList<object>();
        }

        [Test]
        public void TestCountAndCapacity()
        {
            Assert.AreEqual(intMAL.Count, 0);
            Assert.AreEqual(intMAL.Capacity, MyArrayList.DEFAULT_SIZE);
            intMAL.Add(1);
            intMAL.Add(2);
            Assert.AreEqual(intMAL.Count, 2);
            intMAL.RemoveAt(1);
            Assert.AreEqual(intMAL.Count, 1);
            intMAL.RemoveAt(0);
            Assert.AreEqual(intMAL.Count, 0);

            Assert.AreEqual(intMAL2.Count, 0);
            Assert.AreEqual(intMAL2.Capacity, 4);
            intMAL2.Add(1);
            intMAL2.Add(2);
            intMAL2.Add(3);
            Assert.AreEqual(intMAL2.Count, 3);
            Assert.AreEqual(intMAL2.Capacity, 4);

            Assert.Throws<ArgumentOutOfRangeException>(() => {
                intMAL2.Capacity = 3;
            });
            Assert.Throws<ArgumentOutOfRangeException>(() => {
                intMAL2.Capacity = 4;
            });

            intMAL2.Capacity = 10;
            Assert.AreEqual(intMAL2.Count, 3);
            Assert.AreEqual(intMAL2.Capacity, 10);
        }

        [Test]
        public void TestIndexer()
        {
            intMAL.Add(1);
            intMAL.Add(2);
            Assert.AreEqual(intMAL.Count, 2);
            Assert.Throws<IndexOutOfRangeException>(() => {
                var _ = intMAL[2];
            });
            Assert.Throws<IndexOutOfRangeException>(() => {
                intMAL[10] = 42;
            });
        }

        [Test]
        public void TestAdd()
        {
            intMAL.Add(42);
            Assert.AreEqual(intMAL[0], 42);
            intMAL.Add(24);
            Assert.AreEqual(intMAL[1], 24);

            Assert.AreEqual(intMAL.Count, 2);

            intMAL.Add(2);
            Assert.AreEqual(intMAL.Capacity, 4);
        }

        [Test]
        public void TestInsert()
        {
            intMAL.Add(1);
            intMAL.Add(2);
            intMAL.Add(4);
            intMAL.Add(5);

            intMAL.Insert(2, 3);

            Assert.AreEqual(intMAL.Count, 5);
            Assert.AreEqual(intMAL.Capacity, 8);

            for (int i = 0; i < 5; i++) {
                Assert.AreEqual(intMAL[i], i + 1);
            }
        }

        [Test]
        public void TestRemove()
        {
            strMAL.Add("A");
            strMAL.Add("B");
            strMAL.Add("C");

            Assert.IsTrue(strMAL.Remove("B"));
            Assert.AreEqual(strMAL.Count, 2);
            Assert.AreEqual(strMAL[0], "A");
            Assert.AreEqual(strMAL[1], "C");

            Assert.IsFalse(strMAL.Remove("B"));

            intMAL.Add(3);
            intMAL.Add(4);
            intMAL.Add(42);
            intMAL.Add(123);
            intMAL.Add(5);
            intMAL.Add(123);
            intMAL.Add(55);
            intMAL.Add(7);

            var firstSize = intMAL.Count;

            intMAL.Remove(n => n == 42);

            Assert.AreEqual(firstSize - 1, intMAL.Count);
            Assert.AreEqual(123, intMAL[2]);

            firstSize = intMAL.Count;

            intMAL.RemoveAll(n => n >= 10);

            Assert.AreEqual(firstSize - 3, intMAL.Count);
            Assert.AreEqual(3, intMAL[0]);
            Assert.AreEqual(4, intMAL[1]);
            Assert.AreEqual(5, intMAL[2]);
            Assert.AreEqual(7, intMAL[3]);

            var TCMAL = new MyList<TestClass>();

            TCMAL.Add(new TestClass(3));
            TCMAL.Add(new TestClass(42));
            TCMAL.Add(new TestClass(42));
            TCMAL.Add(new TestClass(5));
            TCMAL.Add(new TestClass(42));
            TCMAL.Add(new TestClass(9));

            firstSize = TCMAL.Count;

            TCMAL.RemoveAll(tc => tc.Test == 42);

            Assert.AreEqual(firstSize - 3, TCMAL.Count);
            Assert.AreEqual(3, TCMAL[0].Test);
            Assert.AreEqual(5, TCMAL[1].Test);
            Assert.AreEqual(9, TCMAL[2].Test);
        }

        [Test]
        public void TestRemoveAt()
        {
            strMAL.Add("A");
            strMAL.Add("B");
            strMAL.Add("C");

            strMAL.RemoveAt(1);
            Assert.AreEqual(strMAL.Count, 2);
            Assert.AreEqual(strMAL[0], "A");
            Assert.AreEqual(strMAL[1], "C");

            Assert.Throws<ArgumentOutOfRangeException>(() => {
                strMAL.RemoveAt(2);
            });
        }
        [Test]
        public void TestRemoveRange()
        {
            intMAL.Add(1);
            intMAL.Add(2);
            intMAL.Add(3);
            intMAL.Add(4);

            intMAL.RemoveRange(1, 2);
            Assert.AreEqual(intMAL.Count, 2);
            Assert.AreEqual(intMAL[0], 1);
            Assert.AreEqual(intMAL[1], 4);
        }

        [Test]
        public void TestCopyTo()
        {
            intMAL.Add(0);
            intMAL.Add(1);
            intMAL.Add(2);

            var intArr = new int[5];

            intMAL.CopyTo(intArr);

            for (int i = 0; i < 3; i++) {
                Assert.AreEqual(intArr[i], i);
            }

            intMAL.CopyTo(intArr, 2);
            Assert.AreEqual(intArr[0], 0);
            Assert.AreEqual(intArr[1], 1);
            Assert.AreEqual(intArr[2], 0);
            Assert.AreEqual(intArr[3], 1);
            Assert.AreEqual(intArr[4], 2);
            Assert.Throws<IndexOutOfRangeException>(() => {
                var _ = intArr[5];
            });

            // Fail test 

            intMAL2.Add(0);
            intMAL2.Add(2);

            var intArr2 = new int[1];

            Assert.Throws<ArgumentOutOfRangeException>(() => {
                intMAL2.CopyTo(intArr2);
            });

            intArr2 = new int[5];
            Assert.Throws<ArgumentOutOfRangeException>(() => {
                intMAL2.CopyTo(intArr2, 4);
            });
        }

        [Test]
        public void TestToArray()
        {
            myMAL.Add(1);
            myMAL.Add("A");

            var myArr = myMAL.ToArray();

            Assert.AreEqual((int)myArr[0], 1);
            Assert.AreEqual((string)myArr[1], "A");
            Assert.AreEqual(myArr.Length, 2);
        }

        [Test]
        public void TestSwap()
        {
            intMAL.Add(1);
            intMAL.Add(2);
            intMAL.Add(3);
            intMAL.Swap(0, 2);

            Assert.AreEqual(intMAL[0], 3);
            Assert.AreEqual(intMAL[2], 1);

            Assert.Throws<ArgumentOutOfRangeException>(() => {
                intMAL.Swap(0, 3);
            });
            Assert.Throws<ArgumentOutOfRangeException>(() => {
                intMAL.Swap(3, 4);
            });
        }

        [Test]
        public void TestClear()
        {
            intMAL.Add(1);
            intMAL.Add(2);
            intMAL.Clear();
            Assert.AreEqual(intMAL.Count, 0);
        }

        [Test]
        public void TestContains()
        {
            intMAL.Add(1);
            intMAL.Add(2);
            intMAL.Add(3);
            Assert.IsTrue(intMAL.Contains(2));
            Assert.IsFalse(intMAL.Contains(42));

            Assert.IsTrue(intMAL.Contains(n => n == 2));
            Assert.IsFalse(intMAL.Contains(n => n == 42));
        }

        [Test]
        public void TestIndexOf() {
            intMAL.Add(1);
            intMAL.Add(2);
            intMAL.Add(3);
            intMAL.Add(4);
            intMAL.Add(5);

            Assert.AreEqual(intMAL.IndexOf(6), -1);
            Assert.AreEqual(intMAL.IndexOf(5), 4);

            Assert.AreEqual(intMAL.IndexOf(1, 1), -1);
            Assert.AreEqual(intMAL.IndexOf(5, 4), 4);

            Assert.AreEqual(intMAL.IndexOf(5, 2, 2), -1);
            Assert.AreEqual(intMAL.IndexOf(5, 2, 3), 4);

            Assert.Throws<ArgumentOutOfRangeException>(() => {
                intMAL.IndexOf(5, 3, 3);
            });
        }

        [Test]
        public void LastIndexOf()
        {
            intMAL.Add(1);

            intMAL.Add(2);
            intMAL.Add(3);
            intMAL.Add(2);
            intMAL.Add(3);
            intMAL.Add(4);

            Assert.AreEqual(intMAL.LastIndexOf(6), -1);
            Assert.AreEqual(intMAL.LastIndexOf(2), 3);
            Assert.AreEqual(intMAL.LastIndexOf(3), 4);

            Assert.AreEqual(intMAL.LastIndexOf(2, 2), 1);
            Assert.AreEqual(intMAL.LastIndexOf(2, 0), -1);

            Assert.AreEqual(intMAL.LastIndexOf(3, 1, 2), -1);
            Assert.AreEqual(intMAL.LastIndexOf(3, 2, 2), 2);
            Assert.AreEqual(intMAL.LastIndexOf(1, 2, 2), -1);

            Assert.Throws<ArgumentOutOfRangeException>(() => {
                intMAL.IndexOf(2, 1, 10);
            });
        }

        [Test]
        public void TestEnumerable()
        {
            intMAL.Add(1);
            intMAL.Add(5);
            intMAL.Add(3);
            intMAL.Add(7);

            var answer = new int[10];
            var cnt = 1;
            foreach (var it in intMAL) {
                answer[it] = cnt++;
            }

            Assert.AreEqual(1, answer[1]);
            Assert.AreEqual(2, answer[5]);
            Assert.AreEqual(3, answer[3]);
            Assert.AreEqual(4, answer[7]);
        }

        [Test]
        public void TestEqualityComparer()
        {
            intMAL = new MyList<int>(new NoIntelligenceTrueComparer());
            intMAL.Add(42);
            intMAL.Add(43);
            intMAL.Add(44);
            Assert.AreEqual(0, intMAL.IndexOf(99));
        }

        private class NoIntelligenceTrueComparer : IEqualityComparer<int>
        {
            public bool Equals(int x, int y)
            {
                return true;
            }
            public int GetHashCode(int x)
            {
                return x.GetHashCode();
            }
        }

        [Test]
        public void TestBinarySearch()
        {
            intMAL.Add(42);
            intMAL.Add(45);
            intMAL.Add(47);
            intMAL.Add(55);
            intMAL.Add(67);
            intMAL.Add(99);
            Assert.IsTrue( 0 > intMAL.BinarySearch(424242));
            Assert.AreEqual(3, intMAL.BinarySearch(55));

            intMAL2.Add(99);
            intMAL2.Add(67);
            intMAL2.Add(55);
            intMAL2.Add(47);
            intMAL2.Add(45);
            intMAL2.Add(42);
            Assert.IsTrue(0 > intMAL2.BinarySearch(100, new IntDescComparer()));
            Assert.AreEqual(2, intMAL2.BinarySearch(55, new IntDescComparer()));
        }

        private class IntDescComparer : IComparer<int>
        {
            public int Compare(int a, int b)
            {
                return b.CompareTo(a);
            }
        }

        [Test]
        public void TestSort()
        {
            intMAL.Add(5);
            intMAL.Add(3);
            intMAL.Add(7);
            intMAL.Add(2);
            intMAL.Add(-5);
            intMAL.Add(42);
            intMAL.Sort();
            for (int i = 1; i < intMAL.Count; i++) {
                Assert.IsTrue(intMAL[i] >= intMAL[i - 1]);
            }

            intMAL2.Add(5);
            intMAL2.Add(3);
            intMAL2.Add(7);
            intMAL2.Add(2);
            intMAL2.Add(-5);
            intMAL2.Add(42);
            intMAL2.Sort(new IntDescComparer());
            for (int i = 1; i < intMAL2.Count; i++) {
                Assert.IsTrue(intMAL2[i] <= intMAL2[i - 1]);
            }
        }

        [Test]
        public void TestFind()
        {
            intMAL.Add(5);
            intMAL.Add(3);
            intMAL.Add(42);
            intMAL.Add(7);
            intMAL.Add(9);

            Assert.AreEqual(42, intMAL.Find((n) => n == 42));
            Assert.AreEqual(default(int), intMAL.Find((n) => n == 424242));

            var TCMAL = new MyList<TestClass>();
            TCMAL.Add(new TestClass(41));
            TCMAL.Add(new TestClass(42));
            TCMAL.Add(new TestClass(43));

            Assert.IsNull(TCMAL.Find(tc => tc.Test == 42424242));
        }

        [Test]
        public void TestFindIndex()
        {
            intMAL.Add(5);
            intMAL.Add(3);
            intMAL.Add(42);
            intMAL.Add(7);
            intMAL.Add(42);
            intMAL.Add(7);
            intMAL.Add(9);

            Assert.AreEqual(-1, intMAL.FindIndex(n => n == 4242));
            Assert.AreEqual(2, intMAL.FindIndex(n => n == 42));

            Assert.AreEqual(-1, intMAL.FindIndex(5, n => n == 42));
            Assert.AreEqual(4, intMAL.FindIndex(3, n => n == 42));

            Assert.AreEqual(-1, intMAL.FindIndex(1, 1, n => n == 42));
            Assert.AreEqual(4, intMAL.FindIndex(3, 3, n => n == 42));
        }

        [Test]
        public void TestFindLast()
        {
            intMAL.Add(5);
            intMAL.Add(3);
            intMAL.Add(42);
            intMAL.Add(7);
            intMAL.Add(9);

            Assert.AreEqual(7, intMAL.FindLast(n => n == 7));
            Assert.AreEqual(default(int), intMAL.FindLast(n => n == 4242));
        }

        [Test]
        public void TestFindLastIndex()
        {
            intMAL.Add(5);
            intMAL.Add(3);
            intMAL.Add(42);
            intMAL.Add(7);
            intMAL.Add(42);
            intMAL.Add(7);
            intMAL.Add(9);

            Assert.AreEqual(-1, intMAL.FindLastIndex(n => n == 4242));
            Assert.AreEqual(4, intMAL.FindLastIndex(n => n == 42));

            Assert.AreEqual(-1, intMAL.FindLastIndex(5, n => n == 42));
            Assert.AreEqual(4, intMAL.FindLastIndex(1, n => n == 42));

            Assert.AreEqual(-1, intMAL.FindLastIndex(1, 1, n => n == 42));
            Assert.AreEqual(2, intMAL.FindLastIndex(2, 2, n => n == 42));

        }

        [Test]
        public void TestForEach()
        {
            intMAL.Add(5);
            intMAL.Add(3);
            intMAL.Add(42);
            intMAL.Add(7);
            intMAL.Add(42);
            intMAL.Add(7);
            intMAL.Add(9);

            var ForEachCnt = 0;
            intMAL.ForEach(n => {
                ForEachCnt += n;
            });

            var foreachCnt = 0;
            foreach (var n in intMAL) {
                foreachCnt += n;
            }

            Assert.AreEqual(foreachCnt, ForEachCnt);
        }
    }

    public class TestClass
    {
        public int Test { get; set; }

        public TestClass (int test)
        {
            Test = test;
        }
    }
}