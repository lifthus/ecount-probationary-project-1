using MyCollection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStructureTest
{
    public class MySLinkedListTest
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

        MySLinkedList<TestClass> tcMSLL;
        TestClass[] tcs;
        MySLinkedList<int> intMSLL;

        [SetUp]
        public void SetUp()
        {
            tcMSLL = new MySLinkedList<TestClass>();
            tcs = new TestClass[3];
            tcs[0] = new TestClass(0, "0");
            tcs[1] = new TestClass(1, "1");
            tcs[2] = new TestClass(2, "2");

            intMSLL = new MySLinkedList<int>();
        }

        [Test]
        public void TestAddFirst()
        {
            tcMSLL.AddFirst(tcs[2]);
            tcMSLL.AddFirst(tcs[1]);
            tcMSLL.AddFirst(tcs[0]);

            Assert.AreEqual(3, tcMSLL.Count);

            var firstNode = tcMSLL.First;
            var lastNode = tcMSLL.Last;

            Assert.AreEqual(tcs[0], firstNode.Data);
            Assert.AreEqual(tcs[2], lastNode.Data);

            var curNode = firstNode;
            for (int i = 0; i < 3; i++) {
                var data = curNode.Data;
                Assert.AreEqual(tcs[i].T1, data.T1);
                Assert.AreEqual(tcs[i].T2, data.T2);
                curNode = curNode.Next;
            }
        }

        [Test]
        public void TestAddLast()
        {
            tcMSLL.AddLast(tcs[0]);
            tcMSLL.AddLast(tcs[1]);
            tcMSLL.AddLast(tcs[2]);

            Assert.AreEqual(3, tcMSLL.Count);

            var firstNode = tcMSLL.First;
            var lastNode = tcMSLL.Last;

            Assert.AreEqual(tcs[0], firstNode.Data);
            Assert.AreEqual(tcs[2], lastNode.Data);

            var curNode = firstNode;
            for (int i = 0; i < 3; i++) {
                var data = curNode.Data;
                Assert.AreEqual(tcs[i].T1, data.T1);
                Assert.AreEqual(tcs[i].T2, data.T2);
                curNode = curNode.Next;
            }
        }

        [Test]
        public void TestRemoveFirst()
        {
            var removedNode = tcMSLL.RemoveFirst();

            Assert.IsNull(removedNode);
            Assert.AreEqual(0, tcMSLL.Count);

            tcMSLL.AddLast(tcs[0]);
            tcMSLL.AddLast(tcs[1]);
            tcMSLL.AddLast(tcs[2]);

            removedNode = tcMSLL.RemoveFirst();
            Assert.AreEqual(2, tcMSLL.Count);
            Assert.AreEqual(tcs[0], removedNode);
        }

        [Test]
        public void TestToArray()
        {
            tcMSLL.AddLast(tcs[0]);
            tcMSLL.AddLast(tcs[1]);
            tcMSLL.AddLast(tcs[2]);

            var newArr = tcMSLL.ToArray();

            for (int i = 0; i < 3; i++) {
                Assert.AreEqual(tcs[i].T1, newArr[i].T1);
                Assert.AreEqual(tcs[i].T2, newArr[i].T2);
            }
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
    }
}
