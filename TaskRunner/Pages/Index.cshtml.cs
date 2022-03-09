using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Diagnostics;
using TaskRunner.LongTask;
using TaskRunner.Services;

namespace TaskRunner.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IExampleService2 exampleService2;
        static int id = 1;
        public IndexModel(ILogger<IndexModel> logger, IExampleService2 ExampleService2)
        {
            _logger = logger;
            this.exampleService2 = ExampleService2;
        }

        public void OnGet()
        {
            var bagTaskRunner = HttpContext.RequestServices.GetHostedService<ConsumeBigTask>();

            //var paramsForSpecificTask = new { Id = 1, Name = "A" };
            var paramsForSpecificTask = new { Id = id++, Name = "SomeName" };

            _ = bagTaskRunner.RunBigTasks(paramsForSpecificTask);
            exampleService2.SomeMethod();
        }
    }
}