using System.Diagnostics;

namespace TaskRunner.Services
{
    public interface IExampleService
    {
        void SomeMethod();
    }
    public class ExampleService : IExampleService
    {
        public void SomeMethod()
        {
            throw new NotImplementedException();
        }
    }

    public interface IExampleService2
    {
        void SomeMethod();
    }
    public class ExampleService2 : IExampleService2
    {
        private readonly IExampleService IExampleServiceImpl;
        private readonly int serviceId;

        public ExampleService2(IExampleService exampleService)
        {
            IExampleServiceImpl = exampleService;
            Random rnd = new Random();
            serviceId = rnd.Next(1, 10000);
            
        }

        public void SomeMethod()
        {
            Debug.WriteLine($"ExampleService2 SomeMethod() id={serviceId}");
        }
    }
}
