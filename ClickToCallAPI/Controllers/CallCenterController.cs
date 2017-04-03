using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using ClickToCallAPI.Models;
using System.Configuration;
using System.IO;
using System.Security.Policy;
using System.Threading;
using ClickToCallAPI.Helper;
using ClickToCallAPI.Services;
using FluentScheduler;

namespace ClickToCallAPI.Controllers
{
    [RoutePrefix("api/CallCenter")]
    public class CallCenterController : ApiController
    {
        private readonly INotificationService _notificationService;
        private const string OriginHeader = "Origin";

        public CallCenterController() : this(new NotificationService())
        {
        }

        public CallCenterController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        //public IHttpActionResult Index()
        //{
        //    return View();
        //}

        /// <summary>
        /// Calls Twillio API to initiate phone conference
        /// </summary>
        // POST api/CallCenter/Call
        [HttpPost]
        [Route("Call")]
        public async Task<IHttpActionResult> Call([FromBody] CallViewModel callViewModel)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(m => m.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();
                var errorMessage = string.Join(". ", errors);
                return Json(new { success = false, message = errorMessage });
            }

            var twilioNumber = ConfigurationManager.AppSettings["TwilioNumber"];            
            string salesNumber = Uri.EscapeDataString(callViewModel.SalesNumber);
            
            //var uriHandler = GetUri(callViewModel.SalesNumber); 
            var uriHandler = ConfigurationManager.AppSettings["AWSUrl"] + salesNumber;
            await _notificationService.MakePhoneCallAsync(callViewModel.UserNumber, twilioNumber, uriHandler);
         
            return Json(new { success = true, message = "Phone call incoming!" });
        }

        [HttpPost]
        [Route("ScheduleCall")]
        public IHttpActionResult ScheduleCall([FromBody] SchedulerModel schedulerModel)
        {
            //var registry = new Registry();
            //registry.Schedule<SampleJob>().ToRunNow();
            //JobManager.Initialize(registry);

            JobManager.Initialize(new ScheduledJobRegistry(schedulerModel.Appointment));
            //JobManager.AddJob(() => File.WriteAllText(@"C:\Users\Public\TestFolder\WriteText.txt", "Test"), (s) => s.ToRunEvery(5).Seconds());
            //‌​JobManager.AddJob(() => Write(), s => s.ToRunEvery(2).Seconds());
            JobManager.StopAndBlock();
            return Json(new { success = true, message = "Phone call incoming!" });
        }

        public void Write()
        {
            string text = "A class is the most powerful data type in C#. Like a structure, " +
               "a class defines the data and behavior of the data type. ";
            File.WriteAllText(@"C:\Users\Public\TestFolder\WriteText.txt", text);
        }


        //private string GetUri(string salesNumber)
        //{
        //    if (IsProduction())
        //    {
        //        return Url.Action("Connect", "Call", new { salesNumber }, Request.Url.Scheme);
        //    }

        //    var urlAction = Url.Action("Connect", "Call", new { salesNumber });

        //    var origin = Request.Headers[OriginHeader];
        //    return $"{origin}{urlAction}";
        //}

        //private bool IsProduction()
        //{
        //    var origin = Request.Headers[OriginHeader];
        //    return !new List<string> { "ngrok.io", "localhost" }.Any(domain => origin.Contains(domain));
        //}
    }
}