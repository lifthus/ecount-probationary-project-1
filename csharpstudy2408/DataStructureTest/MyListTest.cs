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
        }
        [Test]
        public void TestFailCopyTo()
        {
            intMAL.Add(0);
            intMAL.Add(2);

            var intArr = new int[1];

            Assert.Throws<ArgumentOutOfRangeException>(() => {
                intMAL.CopyTo(intArr);
            });

            intArr = new int[5];
            Assert.Throws<ArgumentOutOfRangeException>(() => {
                intMAL.CopyTo(intArr, 4);
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
    }
}