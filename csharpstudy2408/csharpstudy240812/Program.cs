using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csharpstudy240812
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var p1 = TrainingCenter.Train<Police>("john", 10);
            var p2 = TrainingCenter.Train<Police>("jack", 6);
            var p3 = TrainingCenter.Train<Police>("james", 3);
            var f1 = TrainingCenter.Train<Firefighter>("ed", 5);
            var f2 = TrainingCenter.Train<Firefighter>("steve", 7);
            var f3 = TrainingCenter.Train<Firefighter>("rogers", 8);

            p1.Work();
            p2.Work();
            f1.Work();

            Console.WriteLine($"경찰관 {Extractor.GetList<Police>().Count()}명 / 소방관 {Extractor.GetList<Firefighter>().Count()}명 / 총 {CentralArchives.GetOfficers().Count()}명");

            CentralArchives.Remove(p3);
            Console.WriteLine($"경찰관 {Extractor.GetList<Police>().Count()}명 / 소방관 {Extractor.GetList<Firefighter>().Count()}명 / 총 {CentralArchives.GetOfficers().Count()}명");

            CentralArchives.Register(p3);
            Console.WriteLine($"경찰관 {Extractor.GetList<Police>().Count()}명 / 소방관 {Extractor.GetList<Firefighter>().Count()}명 / 총 {CentralArchives.GetOfficers().Count()}명");

            Console.WriteLine($"경찰관 홀수 {Extractor.GetList<Police>(po=>po.YearsOfService%2!=0).Count()}명 / 경찰관 짝수 {Extractor.GetList<Police>(po => po.YearsOfService % 2 == 0).Count()}명 / 경찰관 3배 {Extractor.GetList<Police>(po => po.YearsOfService % 3 == 0).Count()}명");
            Console.WriteLine($"소방관 홀수 {Extractor.GetList<Firefighter>(po => po.YearsOfService % 2 != 0).Count()}명 / 소방관 짝수 {Extractor.GetList<Firefighter>(po => po.YearsOfService % 2 == 0).Count()}명 / 소방관 3배 {Extractor.GetList<Firefighter>().Count(po => po.YearsOfService % 3 == 0)}명");
            Console.WriteLine($"총원 {Extractor.GetList<PublicOfficer>().Count()}명");

            Console.ReadLine();
        }
    }
}
