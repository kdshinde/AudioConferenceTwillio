using System.Configuration;
using ClickToCallAPI.Services;
using Twilio.TwiML;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Net;
using System.Text;

namespace ClickToCallAPI.Controllers
{
    [RoutePrefix("api/Call")]
    public class CallController : TwilioController
    {
        private readonly IRequestValidationService _requestValidationService;

        public CallController() : this(new RequestValidationService())
        {
        }

        public CallController(IRequestValidationService requestValidationService)
        {
            _requestValidationService = requestValidationService;
        }

        /// <summary>
        /// Handle a POST from our web form and connect a call via REST API
        /// </summary>
        // POST api/Call/Connect
        [HttpPost]        
        [Route("Connect")]
        public IHttpActionResult Connect([FromUri] string salesNumber)
        {
               
            var twilioAuthToken = ConfigurationManager.AppSettings["TwilioAuthToken"];
            if (!_requestValidationService.IsValidRequest(HttpContext.Current, twilioAuthToken))
            {
                return Unauthorized();
            }

            var response = new TwilioResponse();
            response
              .Say("Thanks for contacting Homesite's Customer Service department. Our " +
                     "next available representative will take your call.")
              .Dial(salesNumber)
              .Hangup();
            return TwiML(response);

       }

        //private IHttpActionResult GetErrorResult(IdentityResult result)
        //{
        //    if (result == null)
        //    {
        //        return InternalServerError();
        //    }

        //    if (!result.Succeeded)
        //    {
        //        if (result.Errors != null)
        //        {
        //            foreach (string error in result.Errors)
        //            {
        //                ModelState.AddModelError("", error);
        //            }
        //        }

        //        if (ModelState.IsValid)
        //        {
        //            // No ModelState errors are available to send, so just return an empty BadRequest.
        //            return BadRequest();
        //        }

        //        return BadRequest(ModelState);
        //    }

        //    return null;
        //}
    }
}
