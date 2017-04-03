using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using FluentScheduler;

namespace ClickToCallAPI.Helper
{
    public class ScheduledJobRegistry: Registry
    {
        public ScheduledJobRegistry(DateTime appointment)
        {
          //  JobManager.AddJob(() => File.WriteAllText(@"C:\Users\Public\TestFolder\WriteText.txt", "Test"), s => s.ToRunNow());
          IJob job = new SampleJob("",DateTime.Now);
          JobManager.AddJob(job, s => s.ToRunOnceIn(1).Minutes());
            //Schedule<SampleJob>().ToRunNow();
        }


    }
}