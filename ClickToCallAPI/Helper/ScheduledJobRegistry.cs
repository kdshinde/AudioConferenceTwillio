using System;
using System.Collections.Generic;
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
            // Schedule<SampleJob>().NonReentrant().ToRunOnceAt(appointment);
            Schedule<SampleJob>().NonReentrant().ToRunNow();
        }
    }
}