using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Configuration;
using System.Net.Http;
using System.Web;
using System.Web.Hosting;
using System.Web.Http.Controllers;
using FluentScheduler;

namespace ClickToCallAPI.Helper
{
    public class SampleJob: IJob, IRegisteredObject
    {
        private readonly object _lock = new object();
        private bool _shuttingDown;
        public string Uri { get; set; }
        //public DateTime Appointment { get; set; }

        public SampleJob(string url, DateTime appointment)
        {
            HostingEnvironment.RegisterObject(this);
            Uri = url;
        }

        public void Execute()
        {
            //WriteToFile();
            lock (_lock)
            {
                if (_shuttingDown)
                    return;

                //PostJson(@"http://clicktocallapi.us-east-1.elasticbeanstalk.com/api/CallCenter/Call");
                //PostJson(@"http://localhost:62688//api/CallCenter/Call");
                //var dbTest = new DbTest();
                WriteToFile();
            }
        }

        public void WriteToFile()
        {
            string text = "A class is the most powerful data type in C#. Like a structure, " +
               "a class defines the data and behavior of the data type. ";
            File.WriteAllText(@"C:\Users\Public\TestFolder\WriteText.txt", text);
        }

        public void PostJson(string uri)
        {
            var httpwebRequest = (HttpWebRequest) WebRequest.Create(uri);
            httpwebRequest.ContentType = "application/json";
            httpwebRequest.Method = "POST";

            using (var streamWriter = new StreamWriter(httpwebRequest.GetRequestStream()))
            {
                string json = "{\"UserNumber\": \" + 15102876432\"," +
                                "\"SalesNumber\": \"+15083806169\"}";
                streamWriter.Write(json);
            }

            using (var httpresponse = (HttpWebResponse) httpwebRequest.GetResponse())
            {
                using (var streamreader = new StreamReader(httpresponse.GetResponseStream()))
                {
                    string result = streamreader.ReadToEnd();
                }
            }
        }

        public void Stop(bool immediate)
        {
            lock (_lock)
            {
                _shuttingDown = true;
            }
            
            HostingEnvironment.UnregisterObject(this);
        }
    }
}