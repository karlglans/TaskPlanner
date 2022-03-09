namespace TaskRunner.LongTask
{
    public interface IBigTaskPlanner
    {
        Task WaitTimeSpan(int milliSec);
    }

    public class BigTaskPlanner : IBigTaskPlanner
    {
        TimeSpan PlannedTime { get; set; } = TimeSpan.Zero;

        /**
         *  Will block till minWaitMilliSec has passed since last call
         **/
        public async Task WaitTimeSpan(int minWaitMilliSec)
        {
            var waitTime = new TimeSpan(0, 0, 0, 0, minWaitMilliSec);
            var diffLastTime = DateTime.Now.TimeOfDay - PlannedTime;
            if (diffLastTime.TotalMilliseconds > minWaitMilliSec)
            {
                // enough time has passed. No waiting is needed
                PlannedTime = DateTime.Now.TimeOfDay.Add(waitTime);
                await Task.CompletedTask;
            }
            else
            {
                // we need to wait
                PlannedTime = PlannedTime.Add(waitTime);
                var diff = PlannedTime.Subtract(DateTime.Now.TimeOfDay);
                await Task.Delay((int)diff.TotalMilliseconds);
            }
        }
    }
}
