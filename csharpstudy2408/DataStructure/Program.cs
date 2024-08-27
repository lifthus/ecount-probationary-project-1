using MyCollection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Threading.Tasks;

namespace DataStructure
{
    public class StringIgnoreCaseComparer : IEqualityComparer<string>
    {
        public bool Equals(string a, string b)
        {
            if (a.Length != b.Length) return false;
            for (int i = 0; i < a.Length; i++) {
                var src = ToLower(a[i]);
                var dst = ToLower(b[i]);
                if (src != dst) return false;
            }
            return true;
        }

        private char ToLower(char c)
        {
            if ('A' <= c && c <= 'Z') {
                return (char)(c + 32);
            }
            return c;
        }


        public int GetHashCode(string obj)
        {
            return obj.GetHashCode() ^ obj.GetHashCode();
        }
    }

    class Program
    {
        static void Main()
        {
            Console.WriteLine("===== 같음 비교자 =====");
            //--------------------------------------------------------------------------------------
            // 기본 비교자를 사용하는 동적 배열 생성
            var x = new MyList<string>();
            x.Add("samsung");
            x.Add("Hyundae");
            x.Add("LG");
            Console.WriteLine(x.Contains("hyundae"));    //=> false
            Console.WriteLine(x.Contains("Hyundae"));    //=> true


            //--------------------------------------------------------------------------------------
            // 사용자 정의 비교자를 사용하는 동적 배열 생성
            var y = new MyList<string>(new StringIgnoreCaseComparer());
            y.Add("samsung");
            y.Add("Hyundae");
            y.Add("LG");
            Console.WriteLine(y.Contains("hyundae"));    //=> true
            Console.WriteLine(y.Contains("Hyundae"));    //=> true

            Console.WriteLine("===== 크기 비교자 =====");

            var carList = new MyList<Car>();
            carList.Add(new Car() { Make = "Hyundai", Year = 1998 });
            carList.Add(new Car() { Make = "Ford", Year = 1996 });
            carList.Add(new Car() { Make = "Hyundai", Year = 2000 });
            carList.Add(new Car() { Make = "Ford", Year = 2020 });
            carList.Add(new Car() { Make = "Hyundai", Year = 1998 });
            Console.WriteLine(" * Make ASC");
            carList.Sort(new CarMakeAscendingComparer());
            foreach (var car in carList) {
                Console.WriteLine(car.ToString());
            }
            Console.WriteLine("* Year DESC");
            carList.Sort(new CarYearDescendingComparer());
            foreach (var car in carList) {
                Console.WriteLine(car.ToString());
            }

            Console.ReadLine();
        }
    }

    public class Car : IComparable<Car>
    {
        public string Make;
        public int Year;

        public override string ToString()
        {
            return $"{Year}: {Make}";
        }

        public override bool Equals(object obj)
        {
            if (obj is Car car) {
                return Make == car.Make && Year == car.Year;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return Make.GetHashCode() ^ Year.GetHashCode();
        }

        public int CompareTo(Car car)
        {
            int yearCmp = Year.CompareTo(car.Year);
            if (yearCmp != 0) {
                return yearCmp;
            }
            return Make.CompareTo(car.Make);
        }
    }

    public class CarMakeAscendingComparer: IComparer<Car>
    {
        public int Compare(Car a, Car b)
        {
            var makeCmp = a.Make.CompareTo(b.Make);
            if (makeCmp != 0) {
                return makeCmp;
            }
            return a.CompareTo(b);
        }
    }

    public class CarYearDescendingComparer: IComparer<Car>
    {
        public int Compare(Car a, Car b)
        {
            var yearDescCmp = b.Year.CompareTo(a.Year);
            if (yearDescCmp != 0) {
                return yearDescCmp;
            }
            return a.CompareTo(b);
        }
    }
}
