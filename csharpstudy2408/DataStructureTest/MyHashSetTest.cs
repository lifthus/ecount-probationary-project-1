using MyCollection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStructureTest
{
    internal class MyHashSetTest
    {
        MyHashSet<int> intHS;

        [SetUp]
        public void SetUp()
        {
            intHS = new MyHashSet<int>();
        }

        [Test]
        public void TestAdd()
        {
            Assert.IsTrue(intHS.Add(1));
            Assert.That(intHS.Count, Is.EqualTo(1));
            Assert.IsFalse(intHS.Add(1));
            Assert.That(intHS.Count, Is.EqualTo(1));
            Assert.IsFalse(intHS.Add(1));
            Assert.That(intHS.Count, Is.EqualTo(1));
            Assert.IsTrue(intHS.Add(2));
            Assert.That(intHS.Count, Is.EqualTo(2));
            Assert.IsTrue(intHS.Add(3));
            Assert.That(intHS.Count, Is.EqualTo(3));

            Assert.IsFalse(intHS.Add(3));
            Assert.That(intHS.Count, Is.EqualTo(3));
        }

        [Test]
        public void TestRemove()
        {
            intHS.Add(1);
            intHS.Add(2);
            intHS.Add(3);

            Assert.IsTrue(intHS.Contains(2));
            Assert.IsTrue(intHS.Remove(2));
            Assert.IsFalse(intHS.Contains(2));
            Assert.That(intHS.Count, Is.EqualTo(2));

            Assert.IsFalse(intHS.Remove(42));

            Assert.IsTrue(intHS.Contains(1));
            Assert.IsTrue(intHS.Remove(1));
            Assert.IsFalse(intHS.Contains(1));
            Assert.That(intHS.Count, Is.EqualTo(1));

            Assert.IsTrue(intHS.Remove(3));
            Assert.That(intHS.Count, Is.EqualTo(0));

            Assert.IsFalse(intHS.Remove(2));
            Assert.That(intHS.Count, Is.EqualTo(0));

        }

        [Test]
        public void TestContains()
        {
            Assert.IsFalse(intHS.Contains(42));

            intHS.Add(1);
            intHS.Add(2);
            intHS.Add(3);

            Assert.IsTrue(intHS.Contains(1));
            Assert.IsTrue(intHS.Contains(2));
            Assert.IsTrue(intHS.Contains(3));

            Assert.That(intHS.Count, Is.EqualTo(3));
        }

        [Test]
        public void TestClear()
        {
            intHS.Add(1);
            intHS.Add(2);
            intHS.Add(3);

            intHS.Clear();

            Assert.That(intHS.Count, Is.EqualTo(0));
        }

        [Test]
        public void TestEnumerable()
        {
            intHS.Add(0);
            intHS.Add(0);
            intHS.Add(1);
            intHS.Add(2);

            int[] arr = new int[3];

            var cnt = 0;
            foreach (var n in intHS) {
                arr[n]++;
                cnt++;
            }
            Assert.That(cnt, Is.EqualTo(3));

            for (int i = 0; i < arr.Length; i++) {
                Assert.That(arr[i], Is.EqualTo(1));
            }

            foreach (var n in intHS) {
                arr[n]++;
            }

            for (int i = 0; i < arr.Length; i++) {
                Assert.That(arr[i], Is.EqualTo(2));
            }
        }

        [Test]
        public void TestResize()
        {
            intHS.Add(1);
            Assert.That(intHS.Count, Is.EqualTo(1));
            intHS.Add(2);
            Assert.That(intHS.Count, Is.EqualTo(2));
            intHS.Add(3);
            Assert.That(intHS.Count, Is.EqualTo(3));
            intHS.Add(4);
            Assert.That(intHS.Count, Is.EqualTo(4));
            intHS.Add(5);
            Assert.That(intHS.Count, Is.EqualTo(5));
            intHS.Add(6);
            Assert.That(intHS.Count, Is.EqualTo(6));
            intHS.Add(7);
            Assert.That(intHS.Count, Is.EqualTo(7));
            intHS.Add(8);
            Assert.That(intHS.Count, Is.EqualTo(8));
            intHS.Add(9);
            Assert.That(intHS.Count, Is.EqualTo(9));
            intHS.Add(10);
            Assert.That(intHS.Count, Is.EqualTo(10));
            intHS.Add(11);
            Assert.That(intHS.Count, Is.EqualTo(11));
            intHS.Add(12);
            Assert.That(intHS.Count, Is.EqualTo(12));
            intHS.Add(13);
            Assert.That(intHS.Count, Is.EqualTo(13));
        }
    }
}
