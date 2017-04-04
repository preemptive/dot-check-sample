// Copyright (c) 2017 PreEmptive Solutions; All Right Reserved, http://www.preemptive.com/
//
// This source is subject to the Microsoft Public License (MS-PL).
// Please see the License.txt file for more information.
// All other rights reserved.
//
// THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY
// KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
// IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.

using System;
using System.Threading.Tasks;
using RestSharp;
using System.IO;


namespace DotCheckSample.Wpf
{

    public class ServiceHelper
    {
        //public const string SERVICE_URL = "http://localhost:12524";
        public const string SERVICE_URL = "http://pasample.azurewebsites.net";

        public static async Task<string> SendTweet()
        {

            var client = new RestClient(ServiceHelper.SERVICE_URL);

            var request = new RestRequest("api/Tweet/Tweet", Method.POST);
            request.RequestFormat = RestSharp.DataFormat.Json;
            request.AddJsonBody(new
            {
                UserName = Wpf.App.UserName,
                Platform = "DotNet",
                CheckType = "Debug",
                LicenseKey = DotCheckSample.Wpf.App.LicenseKey,
                Id = Guid.NewGuid(),
                Department = DotCheckSample.Wpf.App.Department
            });

            var response = await client.ExecutePostTaskAsync(request);

            if (response.ResponseStatus != RestSharp.ResponseStatus.Completed)
            {
                return "Unknown error processing the request.";

            }

            var respStream = response.Content;
            Newtonsoft.Json.JsonSerializer js = new Newtonsoft.Json.JsonSerializer();
            var twitterResponse = (TweetResponse)js.Deserialize(new Newtonsoft.Json.JsonTextReader(new StringReader(respStream)), typeof(TweetResponse));

            return twitterResponse.Message;

        }

        public static async Task<string> SendExpense(decimal amount, string reason)
        {
            var client = new RestClient(ServiceHelper.SERVICE_URL);

            var request = new RestRequest("api/Expense/Approve", Method.POST);
            request.RequestFormat = RestSharp.DataFormat.Json;
            request.AddJsonBody(new
            {
                Amount = amount,
                Reason = reason,
                LicenseKey = DotCheckSample.Wpf.App.LicenseKey,
                Id = Guid.NewGuid(),
                Department = DotCheckSample.Wpf.App.Department
            });

            var response = await client.ExecutePostTaskAsync(request);

            if (response.ResponseStatus != RestSharp.ResponseStatus.Completed)
            {
                return "Unknown error processing the request.";
            }

            var respStream = response.Content;
            Newtonsoft.Json.JsonSerializer js = new Newtonsoft.Json.JsonSerializer();
            var expResponse = (ExpenseApprovalResponse)js.Deserialize(new Newtonsoft.Json.JsonTextReader(new StringReader(respStream)), typeof(ExpenseApprovalResponse));

            if (expResponse.Exception == null || string.IsNullOrEmpty(expResponse.Exception.Message))
            {
                return "Your request was approved.";
            }
            else
            {
                return "Your request was rejected because - " + expResponse.Exception.Message;
            }
            
        }
    }

    public class ExpenseApprovalResponse
    {
        public ExceptionModel Exception {
            get;
            set;
        }
    }
    public class ExceptionModel
    {
        public string Message {
            get;
            set;
        }
    }

    public class TweetResponse
    {
        public string Message
        {
            get;
            set;
        }
    }

}
