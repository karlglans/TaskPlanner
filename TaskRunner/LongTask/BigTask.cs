using System.Diagnostics;
using System.Linq.Expressions;
using TaskRunner.Services;

namespace TaskRunner.LongTask
{
    public interface IBigTask
    {
        public Task RunAsync(dynamic anonymousParams);
        public Task NextStepAsync();
    }

    public class BigSpecificTask : IBigTask
    {
        enum Steps
        {
            Step1, Step2, Step3, Step4, StepDone
        }

        enum StepMinimumWait
        {
            Step1Ms = 5000, Step3Ms = 4000, StepDone
        }

        // some example parameters for this task
        public class BigSpecificTaskParams
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }

        readonly IExampleService2 exampleService2;
        readonly IBigTaskPlanner taskPlanner;
        private BigSpecificTaskParams taskParams;
        private Steps currStep = Steps.Step1;

        public BigSpecificTask(IExampleService2 exampleService2, IBigTaskPlanner taskPlanner)
        {
            this.exampleService2 = exampleService2;
            this.taskPlanner = taskPlanner;
            taskParams = new BigSpecificTaskParams();
        }

        private void ReadParams(dynamic paramsForBigSpecificTask)
        {
            // TODO find a better way to do this
            taskParams.Id = paramsForBigSpecificTask.Id;
            taskParams.Name = paramsForBigSpecificTask.Name;
        }

        public async Task RunAsync(dynamic paramsForBigSpecificTask)
        {
            ReadParams(paramsForBigSpecificTask);
            await NextStepAsync();
        }

        public async Task NextStepAsync()
        {
            switch (currStep) {
                case Steps.Step1: // vänta in klartecken att starta. (kan vara för tätt på impå föregående task)
                    await Step1();
                    break;
                case Steps.Step2: // ladda upp CV
                    await Step2();
                    break;
                case Steps.Step3: // vänta in klartecken att fortsätta
                    await Step3();
                    break;
                case Steps.Step4: // begär virus check.
                    await Step4();
                    break;
            }
            if (currStep != Steps.StepDone)
            {
                await NextStepAsync();
            }
        }

        private async Task Step1()
        {
            Debug.WriteLine($"Step 1, waiting Task {taskParams.Id}");
            exampleService2.SomeMethod();
            await taskPlanner.WaitTimeSpan((int) StepMinimumWait.Step1Ms);
            currStep = Steps.Step2;
        }

        private async Task Step2()
        {
            Debug.WriteLine($"Step 2, Task {taskParams.Id}");
            await Task.CompletedTask;
            currStep = Steps.Step3;
        }

        private async Task Step3()
        {
            Debug.WriteLine($"Step 3, morewaiting Task {taskParams.Id}");
            await taskPlanner.WaitTimeSpan((int)StepMinimumWait.Step3Ms);
            currStep = Steps.Step4;
        }

        private async Task Step4()
        {
            Debug.WriteLine($"Step 4, Task {taskParams.Id}");
            await Task.CompletedTask;
            currStep = Steps.StepDone; // avsluta eller återgå till steg 3
        }
    }
}
