using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using ClickToCallAPI.Models;
using System.Configuration;
using System.Security.Policy;
using ClickToCallAPI.Services;

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
        /// Handle a POST from our web form and connect a call via REST API
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
            string SalesNumber = Uri.EscapeDataString(callViewModel.SalesNumber);
            
            //var uriHandler = GetUri(callViewModel.SalesNumber); 
            var uriHandler = ConfigurationManager.AppSettings["AWSUrl"] + SalesNumber;
            await _notificationService.MakePhoneCallAsync(callViewModel.UserNumber, twilioNumber, uriHandler);
                        
            return Json(new { success = true, message = "Phone call incoming!" });
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