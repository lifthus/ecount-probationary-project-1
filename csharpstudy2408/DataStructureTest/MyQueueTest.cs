using MyCollection;
using NUnit.Framework.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStructureTest
{
    internal class MyQueueTest
    {
        MyQueue<int> intMQ;

        [SetUp]
        public void SetUp()
        {
            intMQ = new MyQueue<int>();
        }

        [Test]
        public void TestEnqueue()
        {
            intMQ.Enqueue(1);
            Assert.That(intMQ.Count, Is.EqualTo(1));
            intMQ.Enqueue(2);
            Assert.That(intMQ.Count, Is.EqualTo(2));
            intMQ.Enqueue(3);
            Assert.That(intMQ.Count, Is.EqualTo(3));


        }

        [Test]
        public void TestDequeue()
        {
            intMQ.Enqueue(1);
            intMQ.Enqueue(2);
            intMQ.Enqueue(3);

            Assert.That(intMQ.Dequeue(), Is.EqualTo(1));
            Assert.That(intMQ.Count, Is.EqualTo(2));
            Assert.That(intMQ.Dequeue(), Is.EqualTo(2));
            Assert.That(intMQ.Count, Is.EqualTo(1));
            Assert.That(intMQ.Dequeue(), Is.EqualTo(3));
            Assert.That(intMQ.Count, Is.EqualTo(0));

            Assert.Throws<InvalidOperationException>(() => intMQ.Dequeue());
        }

        [Test]
        public void TestEnumerable()
        {
            int[] ints = new int[3] { 0, 1, 2 };
            foreach (var n in ints) {
                intMQ.Enqueue(n);
            }
            var cnt = 0;
            foreach (var n in intMQ) {
                Assert.That(n, Is.EqualTo(cnt++));
            }
        }

        [Test]
        public void TestPeek()
        {
            intMQ.Enqueue(1);
            intMQ.Enqueue(2);
            intMQ.Enqueue(3);

            Assert.That(intMQ.Peek(), Is.EqualTo(1));
        }

        [Test]
        public void TestToArray()
        {
            var arr1 = intMQ.ToArray();
            Assert.That(arr1.Length, Is.EqualTo(0));

            intMQ.Enqueue(0);
            intMQ.Enqueue(1);
            intMQ.Enqueue(2);

            var arr2 = intMQ.ToArray();
            for (int i = 0; i < 3; i++) {
                Assert.That(arr2[i], Is.EqualTo(i));
            }
        }

        [Test]
        public void TestClear()
        {
            intMQ.Enqueue(0);
            intMQ.Enqueue(1);
            intMQ.Enqueue(2);

            intMQ.Clear();

            Assert.That(intMQ.Count, Is.EqualTo(0));
            Assert.Throws<InvalidOperationException>(() => intMQ.Dequeue());
            Assert.Throws<InvalidOperationException>(() => intMQ.Peek());
        }
    }
}
