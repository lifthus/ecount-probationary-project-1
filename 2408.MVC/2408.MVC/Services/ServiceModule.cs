using _2408.MVC.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _2408.MVC.Services
{
    public class ServiceModule
    {
        private static Dictionary<Type, Service> _services = new Dictionary<Type, Service>();

        public static void Init() {
            _services.Add(typeof(ProductService), new ProductService());
            _services.Add(typeof(SaleService), new SaleService());
            TestControllers();
        }

        private static void TestControllers()
        {
            new IndexController();
            new ProductAPIController();
            new ProductController();
            new StudyController();
        }

        public static T GetService<T>()
            where T : Service, new()
        {
            if (!_services.ContainsKey(typeof(T))) {
                throw new Exception($"선언되지 않은 서비스:{typeof(T)}");
            }
            var service = _services[typeof(T)] as T;
            return service;
        }
    }

    public class Service { }
}