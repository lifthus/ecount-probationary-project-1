using MyCollection;
using System;
using System.Collections.Generic;
using System.Linq;
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

            Console.ReadLine();
        }
    }
}
