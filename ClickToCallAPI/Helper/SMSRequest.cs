using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace ClickToCallAPI.Helper
{
    public class SMSRequest
    {
        public string UserNumber { get; set; }

        public SMSRequest(string userNumber)
        {
            UserNumber = userNumber;
        }

        public void SendSms()
        {
            var accountSid = ConfigurationManager.AppSettings["TwilioAccountSID"];
            var authToken = ConfigurationManager.AppSettings["TwilioAuthToken"];
            var twilioNumber = ConfigurationManager.AppSettings["TwilioNumber"];

            TwilioClient.Init(accountSid, authToken);

            // make an associative array of people we know, indexed by phone number
            var people = new Dictionary<string, string>() {
                {$"{UserNumber}", "John"}
            };

            // Iterate over all our friends
            foreach (var person in people)
            {
                // Send a new outgoing SMS by POSTing to the Messages resource
                MessageResource.Create(
                    //from: new PhoneNumber("555-555-5555"), // From number, must be an SMS-enabled Twilio number
                    from: new PhoneNumber(twilioNumber),
                    to: new PhoneNumber(person.Key), // To number, if using Sandbox see note above
                                                     // Message content
                    body: $"Hi {person.Value}, how can Homesite help you with Insurance?");                
            }
            


        }
    }
}