using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csharpstudy240812
{
    /* [요구사항]
     1. PublicOfficer 를 상속 받는 클래스를 2개 만드세요. (police, firefigher)
        - 속성을 추가하여도 무방합니다.

     2. Central Archives 클래스를 static 으로 구현하세요.            
         - List<PublicOfficer> 를 가지고 있는 중앙 저장소입니다.                
            ㄴ 해당 필드는 외부에서는 읽기만 가능해야 합니다.
        
         - Register, Remove 메소드 구현
            ㄴ List<PublicOfficer> 의 시퀸스를 추가, 삭제합니다.

         - 모든 생성되는 Officer 는 CentralArchives 에 등록되어야 합니다.                

     3. TrainingCenter 클래스를 static 으로 구현하세요.
            - PublicOfficer 를 상속받은 클래스를 생성하는 팩토리 클래스입니다.
            - Police 뿐만 아니라 다른 Class 에 대한 Train 함수도 구현해야 합니다.                

     4. Extractor 클래스를 static 으로 구현하세요.
            - CentralArchives 에서 필요한 직업의 정보를 추출하는 Utility 클래스입니다.
            - GetPoliceList 메소드 구현합니다. (다른 class 메소드도 구현해야 합니다.)
*/

    #region[publicOfficer]

    public abstract class PublicOfficer
    {
        public string Name { get; set; }
        public int YearsOfService { get; set; }

        public PublicOfficer()
        {
        }

        public virtual void Work()
        {
            // 해당 인스턴스의 "직업명" / ToString() 메소드의 반환값 / "is working" 문자열을 Console 출력하도록
            Console.WriteLine($"{ToString()} is working");
        }

        public override string ToString()
        {
            // "Name/yearsOfService" 을 반환
            return $"{Name}/{YearsOfService}";
        }
    }

    public class Police : PublicOfficer
    {
        // 구현
        public Police(): base()
        {
        }

        public override string ToString()
        {
            return $"경찰관/{base.ToString()}";
        }
    }

    public class Firefighter : PublicOfficer
    {
        public Firefighter(): base()
        {

        }

        public override string ToString()
        {
            return $"소방관/{base.ToString()}";
        }
    }
    #endregion

    public static class CentralArchives
    {
        // 모든 Officier 들의 생성된 기록을 관리합니다.

        // 수식어 키워드를 직접 고민해보세요.
        // List<PublicOfficer> _officers = new List<PublicOfficer>();

        // _officers 을 외부로 노출할 수 있는 프로퍼티를 구현하세요.
        // 바깥에서는 _officers 의 내용물을 추가, 수정, 삭제할 수 없어야 합니다.
        private readonly static List<PublicOfficer> _officers = new List<PublicOfficer>();

        public static void Register(PublicOfficer officer)
        {
            _officers.Add(officer);
        }

        public static bool Remove(PublicOfficer officer)
        {
            return _officers.Remove(officer);
        }

        public static IReadOnlyList<PublicOfficer> GetOfficers ()
        {
            return _officers.AsReadOnly();
        }
    }

    public static class TrainingCenter
    {
        // PublicOfficer 를 상속받은 모든 클래스를 생성하는 팩토리 클래스입니다. 

        public static T Train<T>(string name, int yearsOfService)
             where T : PublicOfficer, new()
        {
            var newOfficer = new T()
            {
                Name = name,
                YearsOfService = yearsOfService
            };
            CentralArchives.Register(newOfficer);
            return newOfficer;
        }
    }

    public static class Extractor
    {
        // CentralArchives 에서 필요한 직업의 정보를 추출하는 Utility 클래스입니다.

       public static List<T> GetList<T>(Func<T, bool> filter = null) where T: PublicOfficer
        {
            if (filter == null) {
                filter = (_) => true;
            }
            return CentralArchives.GetOfficers()
                .OfType<T>()
                .Where(filter)
                .ToList();
        }
    }
}
