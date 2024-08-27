using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cssd10
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Person[] persons = new Person[3] {
                new Person { Name = "에드" , Age = 34},
                new Person { Name ="테일러", Age = 36},
                new Person { Name ="제임스", Age = 33}
            };

            Console.WriteLine("<<IComparable>>");
            Array.Sort(persons);
            for (int i = 0; i < persons.Count(); i++) {
                Console.WriteLine(persons[i].Name);
            }
            Console.WriteLine("<<IComparer>>");
            Array.Sort(persons, new AgeDbcComparer());
            for (int i = 0; i < persons.Count(); i++) {
                Console.WriteLine(persons[i].Name);
            }

            Console.WriteLine("<<Enum>>");
            var people = new People(persons);
            foreach (var person in people) {
                Console.WriteLine(person.Name);
            }

            Console.ReadLine();
        }

        public class Person : IComparable
        {
            public string Name { get; set; }
            public int Age { get; set; }
            public int CompareTo(object obj)
            {
                if (obj == null) {
                    throw new NullReferenceException();
                }
                if (!(obj is Person)) {
                    throw new ArgumentException();
                }
                return this.Age.CompareTo(((Person)obj).Age);
            }
        }

        public class AgeDbcComparer : IComparer<Person>
        {
            public int Compare(Person x, Person y)
            {
                return y.Age.CompareTo(x.Age);
            }
        }

        public class People : IEnumerable<Person>
        {
            private Person[] _persons;

            public People(Person[] persons)
            {
                _persons = persons;
            }

            public IEnumerator<Person> GetEnumerator()
            {
                return new PersonEnumerator(_persons);
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }

        public class PersonEnumerator : IEnumerator<Person>
        {
            private Person[] _persons;
            private int _position = -1;

            public PersonEnumerator(Person[] persons)
            {
                _persons = persons;
            }

            public Person Current {
                get {
                    if (_position < 0 || _position >= _persons.Length)
                        throw new InvalidOperationException();
                    return _persons[_position];
                }
            }

            object IEnumerator.Current => Current;

            public bool MoveNext()
            {
                _position++;
                return (_position < _persons.Length);
            }

            public void Reset()
            {
                _position = -1;
            }

            public void Dispose()
            {
            }
        }

    }
}
