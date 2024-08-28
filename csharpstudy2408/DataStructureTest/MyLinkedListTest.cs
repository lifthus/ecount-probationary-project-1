using MyCollection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStructureTest
{
    internal class MyLinkedListTest
    {
        private class TestClass
        {
            public int T1 { get; set; }
            public string T2 { get; set; }

            public TestClass(int t1, string t2)
            {
                T1 = t1;
                T2 = t2;
            }
        }

        MyLinkedList<TestClass> tcMSLL;
        TestClass[] tcs;
        MyLinkedList<int> intMSLL;

        [SetUp]
        public void SetUp()
        {
            tcMSLL = new MyLinkedList<TestClass>();
            tcs = new TestClass[4];
            tcs[0] = new TestClass(0, "0");
            tcs[1] = new TestClass(1, "1");
            tcs[2] = new TestClass(2, "2");
            tcs[3] = new TestClass(3, "3");

            intMSLL = new MyLinkedList<int>();
        }

        [Test]
        public void TestAddFirst()
        {
            tcMSLL.AddFirst(tcs[2]);
            Assert.That(tcMSLL.Count, Is.EqualTo(1));
            tcMSLL.AddFirst(tcs[1]);
            Assert.That(tcMSLL.Count, Is.EqualTo(2));
            tcMSLL.AddFirst(tcs[0]);
            Assert.That(tcMSLL.Count, Is.EqualTo(3));

            Assert.That(tcMSLL.Count, Is.EqualTo(3));

            Assert.IsNull(tcMSLL.First.Prev);
            Assert.IsTrue(ReferenceEquals(tcMSLL.First.Data, tcs[0]));
            Assert.IsTrue(ReferenceEquals(tcMSLL.First.Next.Data, tcs[1]));

            var node2 = tcMSLL.First.Next;
            Assert.IsTrue(ReferenceEquals(tcMSLL.First, node2.Prev));
            Assert.IsTrue(ReferenceEquals(tcs[0], node2.Prev.Data));
            Assert.IsTrue(ReferenceEquals(node2.Data, tcs[1]));

            var node3 = node2.Next;
            Assert.IsTrue(ReferenceEquals(node2, node3.Prev));
            Assert.IsTrue(ReferenceEquals(tcs[1], node3.Prev.Data));
            Assert.IsTrue(ReferenceEquals(node3.Data, tcs[2]));
            Assert.IsNull(node3.Next);
        }

        [Test]
        public void TestAddLast()
        {
            tcMSLL.AddLast(tcs[0]);
            Assert.That(tcMSLL.Count, Is.EqualTo(1));
            tcMSLL.AddLast(tcs[1]);
            Assert.That(tcMSLL.Count, Is.EqualTo(2));
            tcMSLL.AddLast(tcs[2]);
            Assert.That(tcMSLL.Count, Is.EqualTo(3));

            Assert.IsNull(tcMSLL.First.Prev);
            Assert.IsTrue(ReferenceEquals(tcMSLL.First.Data, tcs[0]));
            Assert.IsTrue(ReferenceEquals(tcMSLL.First.Next.Data, tcs[1]));

            var node2 = tcMSLL.First.Next;
            Assert.IsTrue(ReferenceEquals(tcMSLL.First, node2.Prev));
            Assert.IsTrue(ReferenceEquals(tcs[0], node2.Prev.Data));
            Assert.IsTrue(ReferenceEquals(node2.Data, tcs[1]));

            var node3 = node2.Next;
            Assert.IsTrue(ReferenceEquals(node2, node3.Prev));
            Assert.IsTrue(ReferenceEquals(tcs[1], node3.Prev.Data));
            Assert.IsTrue(ReferenceEquals(node3.Data, tcs[2]));
            Assert.IsNull(node3.Next);
        }

        [Test]
        public void TestClear()
        {
            tcMSLL.AddLast(tcs[0]);
            tcMSLL.AddLast(tcs[1]);

            tcMSLL.Clear();

            Assert.IsNull(tcMSLL.First);
            Assert.IsNull(tcMSLL.Last);
            Assert.That(tcMSLL.Count, Is.EqualTo(0));
        }

        [Test]
        public void TestRemoveFirst()
        {
            tcMSLL.AddLast(tcs[0]);
            tcMSLL.AddLast(tcs[1]);
            tcMSLL.AddLast(tcs[2]);

            tcMSLL.RemoveFirst();

            Assert.IsNull(tcMSLL.First.Prev);
            Assert.IsTrue(ReferenceEquals(tcMSLL.First.Data, tcs[1]));
            Assert.IsTrue(ReferenceEquals(tcMSLL.First.Next.Data, tcs[2]));

            var secondNode = tcMSLL.First.Next;
            Assert.IsTrue(ReferenceEquals(secondNode.Prev.Data, tcs[1]));
            Assert.IsTrue(ReferenceEquals(secondNode.Data, tcs[2]));
            Assert.IsNull(secondNode.Next);

            Assert.That(tcMSLL.Count, Is.EqualTo(2));

            tcMSLL.RemoveFirst();
            Assert.That(tcMSLL.Count, Is.EqualTo(1));

        }

        [Test]
        public void TestRemoveLast()
        {
            tcMSLL.AddLast(tcs[0]);
            tcMSLL.AddLast(tcs[1]);
            tcMSLL.AddLast(tcs[2]);

            tcMSLL.RemoveLast();

            Assert.IsNull(tcMSLL.First.Prev);
            Assert.IsTrue(ReferenceEquals(tcMSLL.First.Data, tcs[0]));
            Assert.IsTrue(ReferenceEquals(tcMSLL.First.Next.Data, tcs[1]));

            var secondNode = tcMSLL.First.Next;
            Assert.IsTrue(ReferenceEquals(secondNode.Prev.Data, tcs[0]));
            Assert.IsTrue(ReferenceEquals(secondNode.Data, tcs[1]));
            Assert.IsNull(secondNode.Next);

            Assert.That(tcMSLL.Count, Is.EqualTo(2));

            tcMSLL.RemoveLast();
            Assert.That(tcMSLL.Count, Is.EqualTo(1));
        }

        [Test]
        public void TestRemove()
        {
            tcMSLL.AddLast(tcs[0]);
            tcMSLL.AddLast(tcs[1]);
            tcMSLL.AddLast(tcs[2]);

            tcMSLL.Remove(tcMSLL.First.Next);

            Assert.IsNull(tcMSLL.First.Prev);
            Assert.IsTrue(ReferenceEquals(tcMSLL.First.Data, tcs[0]));
            Assert.IsTrue(ReferenceEquals(tcMSLL.First.Next.Data, tcs[2]));

            var secondNode = tcMSLL.First.Next;
            Assert.IsTrue(ReferenceEquals(secondNode.Prev.Data, tcs[0]));
            Assert.IsTrue(ReferenceEquals(secondNode.Data, tcs[2]));
            Assert.IsNull(secondNode.Next);

            Assert.That(tcMSLL.Count, Is.EqualTo(2));

            tcMSLL.Remove(tcMSLL.First);
            Assert.That(tcMSLL.Count, Is.EqualTo(1));

            Assert.IsTrue(ReferenceEquals(tcMSLL.First, tcMSLL.Last));
            Assert.IsNull(tcMSLL.First.Prev);
            Assert.IsNull(tcMSLL.First.Next);
            Assert.IsNull(tcMSLL.Last.Prev);
            Assert.IsNull(tcMSLL.Last.Next);

            Assert.IsFalse(tcMSLL.Remove(tcs[3]));
            Assert.That(tcMSLL.Count, Is.EqualTo(1));

            tcMSLL.Clear();

            tcMSLL.AddLast(tcs[0]);
            tcMSLL.AddLast(tcs[1]);
            tcMSLL.AddLast(tcs[2]);

            Assert.IsTrue(tcMSLL.Remove(tcs[1]));
            Assert.That(tcMSLL.Count, Is.EqualTo(2));
            Assert.IsTrue(ReferenceEquals(tcMSLL.First.Data, tcs[0]));
            Assert.IsTrue(ReferenceEquals(tcMSLL.First.Next.Data, tcs[2]));
            Assert.IsTrue(ReferenceEquals(tcMSLL.Last.Prev.Data, tcs[0]));
            Assert.IsTrue(ReferenceEquals(tcMSLL.Last.Data, tcs[2]));
        }

        [Test]
        public void TestFind()
        {
            tcMSLL.AddLast(tcs[0]);
            tcMSLL.AddLast(tcs[1]);
            tcMSLL.AddLast(tcs[0]);
            tcMSLL.AddLast(tcs[1]);
            tcMSLL.AddLast(tcs[2]);

            var node = tcMSLL.Find(tcs[1]);

            Assert.IsNotNull(node);
            Assert.IsTrue(ReferenceEquals(node.Prev, tcMSLL.First));
            Assert.IsTrue(ReferenceEquals(node.Prev.Data, tcs[0]));
            Assert.IsTrue(ReferenceEquals(node.Next.Data, tcs[0]));
            Assert.IsTrue(ReferenceEquals(node.Prev.Data, node.Next.Data));
            Assert.IsFalse(ReferenceEquals(node.Prev, node.Next));
        }

        [Test]
        public void TestFindLast()
        {
            tcMSLL.AddLast(tcs[0]);
            tcMSLL.AddLast(tcs[1]);
            tcMSLL.AddLast(tcs[0]);
            tcMSLL.AddLast(tcs[1]);
            tcMSLL.AddLast(tcs[2]);

            var node = tcMSLL.FindLast(tcs[1]);

            Assert.IsNotNull(node);
            Assert.IsTrue(ReferenceEquals(node.Next, tcMSLL.Last));
            Assert.IsTrue(ReferenceEquals(node.Prev.Data, tcs[0]));
            Assert.IsTrue(ReferenceEquals(node.Next.Data, tcs[2]));
            Assert.IsNull(node.Next.Next);
        }

        [Test]
        public void TestToArray()
        {
            tcMSLL.AddLast(tcs[0]);
            tcMSLL.AddLast(tcs[1]);
            tcMSLL.AddLast(tcs[2]);

            var newArr = tcMSLL.ToArray();

            for (int i = 0; i < 3; i++) {
                Assert.That(newArr[i].T1, Is.EqualTo(tcs[i].T1));
                Assert.That(newArr[i].T2, Is.EqualTo(tcs[i].T2));
            }
        }

        [Test]
        public void TestContains()
        {
            tcMSLL.AddLast(tcs[0]);
            tcMSLL.AddLast(tcs[1]);
            tcMSLL.AddLast(tcs[2]);

            Assert.IsTrue(tcMSLL.Contains(tcs[0]));
            Assert.IsTrue(tcMSLL.Contains(tcs[1]));
            Assert.IsTrue(tcMSLL.Contains(tcs[2]));
            Assert.IsFalse(tcMSLL.Contains(tcs[3]));
        }

        [Test]
        public void TestEnumerable()
        {
            intMSLL.AddLast(1);
            intMSLL.AddLast(5);
            intMSLL.AddLast(3);
            intMSLL.AddLast(7);

            var answer = new int[10];
            var cnt = 1;
            foreach (var it in intMSLL) {
                answer[it] = cnt++;
            }

            Assert.That(answer[1], Is.EqualTo(1));
            Assert.That(answer[5], Is.EqualTo(2));
            Assert.That(answer[3], Is.EqualTo(3));
            Assert.That(answer[7], Is.EqualTo(4));
        }

        [Test]
        public void TestEqualityComparer()
        {
            tcMSLL = new MyLinkedList<TestClass>(new TestClassNoIntelligenceTrueEqualityComparer());
            tcMSLL.AddLast(tcs[0]);
            tcMSLL.AddLast(tcs[1]);
            tcMSLL.AddLast(tcs[2]);

            Assert.IsTrue(tcMSLL.Contains(tcs[3]));
            var node0 = tcMSLL.Find(tcs[3]);
            Assert.IsTrue(ReferenceEquals(node0.Data, tcs[0]));
        }

        private class TestClassNoIntelligenceTrueEqualityComparer : IEqualityComparer<TestClass>
        {
            public bool Equals(TestClass a, TestClass b)
            {
                return true;
            }

            public int GetHashCode(TestClass a)
            {
                return a.GetHashCode();
            }
        }

        [Test]
        public void TestContainsExt()
        {
            tcMSLL.AddLast(tcs[0]);
            tcMSLL.AddLast(tcs[1]);
            tcMSLL.AddLast(tcs[2]);

            Assert.IsTrue(tcMSLL.Contains(node => {
                return node.Data.T1 == 1;
            }));
            Assert.IsFalse(tcMSLL.Contains(node => {
                return node.Data.T1 == 42;
            }));
        }

        [Test]
        public void TestFindExt()
        {
            tcMSLL.AddLast(tcs[0]);
            tcMSLL.AddLast(tcs[1]);
            tcMSLL.AddLast(tcs[1]);
            tcMSLL.AddLast(tcs[2]);

            Assert.IsNull(tcMSLL.Find(node => {
                return node.Data.T1 == 42;
            }));

            var node1 = tcMSLL.Find(node => {
                return node.Data.T1 == 1;
            });

            Assert.IsTrue(ReferenceEquals(node1.Data, tcs[1]));
            Assert.IsTrue(ReferenceEquals(node1.Prev.Data, tcs[0]));
        }

        [Test]
        public void TestFindLastExt()
        {
            tcMSLL.AddLast(tcs[0]);
            tcMSLL.AddLast(tcs[1]);
            tcMSLL.AddLast(tcs[1]);
            tcMSLL.AddLast(tcs[2]);

            Assert.IsNull(tcMSLL.Find(node => {
                return node.Data.T1 == 42;
            }));

            var node1_2 = tcMSLL.FindLast(node => {
                return node.Data.T1 == 1;
            });

            Assert.IsTrue(ReferenceEquals(node1_2.Data, tcs[1]));
            Assert.IsTrue(ReferenceEquals(node1_2.Next.Data, tcs[2]));

        }

        [Test]
        public void TestRemoveExt()
        {
            tcMSLL.AddLast(tcs[0]);
            tcMSLL.AddLast(tcs[1]);
            tcMSLL.AddLast(tcs[0]);
            tcMSLL.AddLast(tcs[1]);
            tcMSLL.AddLast(tcs[2]);

            tcMSLL.Remove(node => {
                return node.Data.T1 == 1;
            });

            Assert.That(tcMSLL.Count, Is.EqualTo(4));
            Assert.IsTrue(ReferenceEquals(tcMSLL.First.Data, tcMSLL.First.Next.Data));
            Assert.IsTrue(ReferenceEquals(tcMSLL.Last.Data, tcs[2]));
        }
    }
}
