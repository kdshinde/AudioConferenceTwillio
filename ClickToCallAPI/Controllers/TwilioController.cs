using ClickToCallAPI.Helper;
using System.Web.Http;
using Twilio.TwiML;

namespace ClickToCallAPI.Controllers
{
    /// <summary>
    /// Extends the standard base controller to simplify returning a TwiML response
    /// </summary>
    public class TwilioController : ApiController
    {
        /// <summary>
        /// Returns a property formatted TwiML response
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        public TwiMLResult TwiML(TwilioResponse response)
        {
            return new TwiMLResult(Request, response);
        }
    }
}
