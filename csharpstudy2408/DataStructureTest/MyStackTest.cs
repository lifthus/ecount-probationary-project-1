using MyCollection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;

namespace DataStructureTest
{
    internal class MyStackTest
    {
        MyStack<int> intMSt;

        [SetUp]
        public void SetUp()
        {
            intMSt = new MyStack<int>();
        }

        [Test]
        public void TestPush()
        {
            Assert.That(intMSt.Count, Is.EqualTo(0));
            intMSt.Push(1);
            Assert.That(intMSt.Count, Is.EqualTo(1));
            intMSt.Push(2);
            Assert.That(intMSt.Count, Is.EqualTo(2));
            intMSt.Push(3);
            Assert.That(intMSt.Count, Is.EqualTo(3));
        }

        [Test]
        public void TestEnumerable()
        {
            int[] ints = new int[3] { 2, 1, 0 };
            foreach (var n in ints) {
                intMSt.Push(n);
            }
            var cnt = 0;
            foreach (var n in intMSt) {
                Assert.That(n, Is.EqualTo(cnt++));
            }
        }

        [Test]
        public void TestPeek()
        {
            Assert.Throws<InvalidOperationException>(() => intMSt.Peek());

            intMSt.Push(1);
            intMSt.Push(2);
            intMSt.Push(3);

            Assert.That(intMSt.Peek(), Is.EqualTo(3));
            Assert.That(intMSt.Count, Is.EqualTo(3));
        }

        [Test]
        public void TestPop()
        {
            Assert.Throws<InvalidOperationException>(() => intMSt.Pop());

            intMSt.Push(1);
            intMSt.Push(2);
            intMSt.Push(3);

            Assert.That(intMSt.Pop(), Is.EqualTo(3));
            Assert.That(intMSt.Count, Is.EqualTo(2));
        }

        [Test]
        public void TestToArray()
        {
            var intArr1 = intMSt.ToArray();
            Assert.That(intArr1.Length, Is.EqualTo(0));

            intMSt.Push(2);
            intMSt.Push(1);
            intMSt.Push(0);

            var intArr2 = intMSt.ToArray();
            Assert.That(intArr2.Length, Is.EqualTo(3));

            for (int i = 0; i < 3; i++) {
                Assert.That(intArr2[i], Is.EqualTo(i));
            }

        }

        public void TestClear()
        {
            intMSt.Push(1);
            intMSt.Push(2);
            intMSt.Push(3);

            intMSt.Clear();

            Assert.That(intMSt.Count, Is.EqualTo(0));
            Assert.Throws<InvalidOperationException>(() => intMSt.Peek());
            Assert.Throws<InvalidOperationException>(() => intMSt.Pop());
        }
    }
}
