using MyCollection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStructureTest
{
    internal class MyHashMapTest
    {
        MyHashMap<int> intHM;

        [SetUp]
        public void SetUp()
        {
            intHM = new MyHashMap<int>();
        }

        [Test]
        public void TestClear()
        {
            intHM.Add("0", 0);
            intHM.Add("0", 1);
            intHM.Add("0", 2);
            intHM.Add("1", 0);
            intHM.Add("1", 1);
            intHM.Add("1", 2);
            intHM.Add("1", 3);

            Assert.IsTrue(intHM.Contains("0"));
            Assert.IsTrue(intHM.Contains("1"));
            Assert.That(intHM.Count, Is.EqualTo(2));

            intHM.Clear();

            Assert.IsFalse(intHM.Contains("0"));
            Assert.IsFalse(intHM.Contains("1"));
            Assert.That(intHM.Count, Is.EqualTo(0));
        }

        [Test]
        public void TestGetValues()
        {
            Assert.That(intHM.GetValues("42").Length, Is.EqualTo(0));

            intHM.Add("42", 0);
            intHM.Add("42", 1);
            intHM.Add("42", 2);

            Assert.That(intHM.GetValues("42").Length, Is.EqualTo(3));
        }

        [Test]
        public void TestGetAllValues()
        {

            intHM.Add("42", 0);
            intHM.Add("42", 1);
            intHM.Add("42", 2);
            intHM.Add("1", 3);
            intHM.Add("2", 4);
            intHM.Add("2", 5);

            int[] ans = new int[6];

            foreach (var n in intHM.GetAllValues()) {
                ans[n] += 1;
            }

            Assert.IsTrue(ans.All(n => n == 1));
        }

        [Test]
        public void TestContains()
        {
            Assert.IsFalse(intHM.Contains("42"));
            Assert.IsFalse(intHM.Contains("1"));
            Assert.IsFalse(intHM.Contains("2"));
            intHM.Add("42", 0);
            intHM.Add("42", 1);
            intHM.Add("42", 2);
            intHM.Add("1", 3);
            intHM.Add("2", 4);
            Assert.IsTrue(intHM.Contains("42"));
            Assert.IsTrue(intHM.Contains("1"));
            Assert.IsTrue(intHM.Contains("2"));
            intHM.Remove("42");
            Assert.IsFalse(intHM.Contains("42"));
            Assert.IsTrue(intHM.Contains("1"));
            Assert.IsTrue(intHM.Contains("2"));
        }

        [Test]
        public void TestAdd()
        {
            intHM.Add("42", 0);
            Assert.That(intHM.Count, Is.EqualTo(1));
            Assert.That(intHM.GetAllValues().Count, Is.EqualTo(1));
            Assert.That(intHM.GetValues("42").Count, Is.EqualTo(1));
            Assert.IsTrue(intHM.Contains("42"));

            intHM.Add("42", 1);
            Assert.That(intHM.Count, Is.EqualTo(1));
            Assert.That(intHM.GetAllValues().Count, Is.EqualTo(2));
            Assert.That(intHM.GetValues("42").Count, Is.EqualTo(2));
            Assert.IsTrue(intHM.Contains("42"));

            intHM.Add("42", 2);
            Assert.That(intHM.Count, Is.EqualTo(1));
            Assert.That(intHM.GetAllValues().Count, Is.EqualTo(3));
            Assert.That(intHM.GetValues("42").Count, Is.EqualTo(3));
            Assert.IsTrue(intHM.Contains("42"));

            intHM.Add("1", 3);
            Assert.That(intHM.Count, Is.EqualTo(2));
            Assert.That(intHM.GetAllValues().Count, Is.EqualTo(4));
            Assert.That(intHM.GetValues("1").Count, Is.EqualTo(1));
            Assert.IsTrue(intHM.Contains("1"));

            intHM.Add("2", 4);
            Assert.That(intHM.Count, Is.EqualTo(3));
            Assert.That(intHM.GetAllValues().Count, Is.EqualTo(5));
            Assert.That(intHM.GetValues("2").Count, Is.EqualTo(1));
            Assert.IsTrue(intHM.Contains("2"));

            Assert.That(intHM.Count, Is.EqualTo(3));
            Assert.That(intHM.GetAllValues().Length, Is.EqualTo(5));
        }

        [Test]
        public void TestRemove()
        {
            Assert.IsFalse(intHM.Remove("42"));
            Assert.That(intHM.Count, Is.EqualTo(0));
            Assert.That(intHM.GetAllValues().Length, Is.EqualTo(0));

            intHM.Add("42", 0);
            intHM.Add("42", 1);
            intHM.Add("42", 2);
            intHM.Add("1", 3);
            intHM.Add("2", 4);
            intHM.Add("2", 5);

            Assert.IsTrue(intHM.Remove("2"));
            Assert.That(intHM.Count, Is.EqualTo(2));
            Assert.That(intHM.GetAllValues().Length, Is.EqualTo(4));


            Assert.IsTrue(intHM.Remove("1"));
            Assert.That(intHM.Count, Is.EqualTo(1));
            Assert.That(intHM.GetAllValues().Length, Is.EqualTo(3));

            Assert.IsTrue(intHM.Remove("42"));
            Assert.That(intHM.Count, Is.EqualTo(0));
            Assert.That(intHM.GetAllValues().Length, Is.EqualTo(0));

            Assert.IsFalse(intHM.Remove("42"));
            Assert.That(intHM.Count, Is.EqualTo(0));
            Assert.That(intHM.GetAllValues().Length, Is.EqualTo(0));
        }

        [Test]
        public void TestEnumerable()
        {
            intHM.Add("0", 0);
            intHM.Add("0", 1);
            intHM.Add("0", 2);
            intHM.Add("0", 3);
            intHM.Add("1", 0);
            intHM.Add("1", 1);
            intHM.Add("1", 2);
            intHM.Add("1", 3);

            var listCnt = 0;
            var eCount = 0;
            var ans = new int[4];
            foreach (var kvp in intHM) {
                var key = kvp.Key;
                var val = kvp.Value;
                listCnt++;
                eCount += val.Count;
                foreach(var n in val) {
                    ans[n]++;
                }
            }

            Assert.That(listCnt, Is.EqualTo(2));
            Assert.That(eCount, Is.EqualTo(8));
            Assert.That(ans.All(n => n == 2));
        }

        [Test]
        public void TestEnumerableKeys()
        {
            intHM.Add("0", 0);
            intHM.Add("0", 1);
            intHM.Add("0", 2);
            intHM.Add("0", 3);
            intHM.Add("1", 0);
            intHM.Add("1", 1);
            intHM.Add("1", 2);
            intHM.Add("1", 3);

            var ans = new int[4];
            var cnt = 0;
            foreach (var key in intHM.Keys) {
                cnt++;
                foreach (var n in intHM.GetValues(key)) {
                    ans[n]++;
                }
            }

            Assert.That(cnt, Is.EqualTo(2));
            Assert.IsTrue(ans.All(n => n == 2));
        }
    }
}
