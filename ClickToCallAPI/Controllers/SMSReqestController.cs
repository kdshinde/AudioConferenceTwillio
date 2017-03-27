using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ClickToCallAPI.Helper;
using ClickToCallAPI.Models;

namespace ClickToCallAPI.Controllers
{
    [RoutePrefix("api/SMSRequest")]
    public class SMSReqestController : ApiController
    {
        /// <summary>
        /// Handle a POST from our web form and connect a call via REST API
        /// </summary>
        // POST api/SMSRequest/SendSMS
        [HttpPost]
        [Route("SendSMS")]
        public IHttpActionResult SendSMS([FromBody] SMSRequestModel model)
        {
            var request = new SMSRequest(model.UserNumber);
            request.SendSms(); 
            return Ok();

        }
    }
}
