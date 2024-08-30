using MyCollection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStructureTest
{
    internal class MyDictionaryTest
    {
        MyDictionary<int, string> isD;

        [SetUp]
        public void SetUp()
        {
            isD = new MyDictionary<int, string>();
        }

        [Test]
        public void TestAdd()
        {
            isD.Add(1, "3");
            Assert.That(isD.Count, Is.EqualTo(1));
            isD.Add(1, "2");
            Assert.That(isD.Count, Is.EqualTo(1));

            string tmp;
            Assert.IsTrue(isD.TryGetValue(1, out tmp));
            Assert.That(tmp, Is.EqualTo("2"));

            isD.Add(1, "1");
            Assert.That(isD.Count, Is.EqualTo(1));
            isD.Add(2, "2");
            Assert.That(isD.Count, Is.EqualTo(2));
            isD.Add(3, "3");
            Assert.That(isD.Count, Is.EqualTo(3));
        }

        [Test]
        public void TestRemove()
        {
            isD.Add(1, "1");
            isD.Add(2, "2");
            isD.Add(3, "3");

            Assert.IsFalse(isD.Remove(42));
            Assert.That(isD.Count, Is.EqualTo(3));


            Assert.IsTrue(isD.Contains(2));
            Assert.IsTrue(isD.Remove(2));
            Assert.IsFalse(isD.Contains(2));
            Assert.That(isD.Count, Is.EqualTo(2));

            Assert.IsTrue(isD.Contains(1));
            Assert.IsTrue(isD.Remove(1));
            Assert.IsFalse(isD.Contains(1));
            Assert.That(isD.Count, Is.EqualTo(1));

            Assert.IsTrue(isD.Remove(3));
            Assert.That(isD.Count, Is.EqualTo(0));

            Assert.IsFalse(isD.Remove(2));
            Assert.That(isD.Count, Is.EqualTo(0));
        }

        [Test]
        public void TestContains()
        {
            Assert.IsFalse(isD.Contains(42));

            isD.Add(1, "1");
            isD.Add(2, "2");
            isD.Add(3, "3");

            Assert.IsTrue(isD.Contains(1));
            Assert.IsTrue(isD.Contains(2));
            Assert.IsTrue(isD.Contains(3));

            Assert.IsFalse(isD.Contains(42));

            Assert.That(isD.Count, Is.EqualTo(3));
        }

        [Test]
        public void TestClear()
        {
            isD.Add(1, "1");
            isD.Add(2, "2");
            isD.Add(3, "3");

            isD.Clear();

            Assert.IsFalse(isD.Contains(1));
            Assert.IsFalse(isD.Contains(2));
            Assert.IsFalse(isD.Contains(3));

            Assert.That(isD.Count, Is.EqualTo(0));
        }

        [Test]
        public void TestEnumerable()
        {
            isD.Add(0, "0");
            isD.Add(1, "1");
            isD.Add(2, "2");
            isD.Add(3, "3");

            int[] arr = new int[4];

            var cnt = 0;
            foreach (var kvp in isD) {
                var k = kvp.Key;
                var v = kvp.Value;
                var vint = Int32.Parse(v);
                arr[vint]++;
                cnt++;
            }
            Assert.That(cnt, Is.EqualTo(4));

            for (int i = 0; i < arr.Length; i++) {
                Assert.That(arr[i], Is.EqualTo(1));
            }

            foreach (var kvp in isD) {
                var k = kvp.Key;
                var v = kvp.Value;
                var vint = Int32.Parse(v);
                arr[vint]++;
                cnt++;
            }

            for (int i = 0; i < arr.Length; i++) {
                Assert.That(arr[i], Is.EqualTo(2));
            }
        }

        [Test]
        public void TestKeyEnumerable()
        {
            isD.Add(0, "0");
            isD.Add(1, "1");
            isD.Add(2, "2");

            int[] arr = new int[3];
            
            foreach(var k in isD.Keys) {
                arr[k]++;
            }

            Assert.That(arr.All(n => n == 1));

            foreach (var k in isD.Keys) {
                arr[k]++;
            }

            Assert.That(arr.All(n => n == 2));
        }

        [Test]
        public void TestValueEnumerable()
        {
            isD.Add(0, "0");
            isD.Add(1, "1");
            isD.Add(2, "2");

            int[] arr = new int[3];

            foreach (var v in isD.Values) {
                var vInt = Int32.Parse(v);
                arr[vInt]++;
            }

            Assert.That(arr.All(n => n == 1));

            foreach (var v in isD.Values) {
                var vInt = Int32.Parse(v);
                arr[vInt]++;
            }

            Assert.That(arr.All(n => n == 2));
        }

        [Test]
        public void TestTryGetValue()
        {
            string tmp;
            Assert.IsFalse(isD.TryGetValue(42, out tmp));

            isD.Add(1, "1");
            isD.Add(2, "2");
            isD.Add(3, "3");

            Assert.IsTrue(isD.TryGetValue(1, out tmp));
            Assert.That(tmp, Is.EqualTo("1"));
            Assert.IsTrue(isD.TryGetValue(2, out tmp));
            Assert.That(tmp, Is.EqualTo("2"));
            Assert.IsTrue(isD.TryGetValue(3, out tmp));
            Assert.That(tmp, Is.EqualTo("3"));
        }

        [Test]
        public void TestResize()
        {
            isD.Add(1, "42");
            Assert.That(isD.Count, Is.EqualTo(1));
            isD.Add(2, "42");
            Assert.That(isD.Count, Is.EqualTo(2));
            isD.Add(3, "42");
            Assert.That(isD.Count, Is.EqualTo(3));
            isD.Add(4, "42");
            Assert.That(isD.Count, Is.EqualTo(4));
            isD.Add(5, "42");
            Assert.That(isD.Count, Is.EqualTo(5));
            isD.Add(6, "42");
            Assert.That(isD.Count, Is.EqualTo(6));
            isD.Add(7, "42");
            Assert.That(isD.Count, Is.EqualTo(7));
            isD.Add(8, "42");
            Assert.That(isD.Count, Is.EqualTo(8));
            isD.Add(9, "42");
            Assert.That(isD.Count, Is.EqualTo(9));
            isD.Add(10, "42");
            Assert.That(isD.Count, Is.EqualTo(10));
            isD.Add(11, "42");
            Assert.That(isD.Count, Is.EqualTo(11));
            isD.Add(12, "42");
            Assert.That(isD.Count, Is.EqualTo(12));
            isD.Add(13, "42");
            Assert.That(isD.Count, Is.EqualTo(13));
        }
    }
}
