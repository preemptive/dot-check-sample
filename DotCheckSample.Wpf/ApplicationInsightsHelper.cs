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

// This is a modified version of MIT Licensed code, initially from here: https://github.com/DeadlyEmbrace/ApplicationInsightExample
// See ThirdPartyLicenses.txt

using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.ApplicationInsights;

namespace DotCheckSample
{
    public class ApplicationInsightHelper
    {
        private readonly TelemetryClient _telemetryClient;
        private string _sessionKey;
        private string _userName;
        private string _version;
        private string _application;

        /// <summary>
        /// Constructor
        /// </summary>
        public ApplicationInsightHelper(string instrumentationKey)
        {
            _telemetryClient = new TelemetryClient() { InstrumentationKey = instrumentationKey };
            GatherDetails();
            SetupTelemetry();
        }

        /// <summary>
        /// This method flushes the Telemetry Client data to the server
        /// </summary>
        public void FlushData()
        {
            DoDataFlush();
        }

        public void TrackEvent( string eventName, IDictionary<string,string> properties )
        {            
            _telemetryClient.TrackEvent(eventName, properties);
        }

        /// <summary>
        /// Set up the Telemetry client context data
        /// This records a variety of details to enhance our data collection
        /// </summary>
        private void SetupTelemetry()
        {
            _telemetryClient.Context.Properties.Add("Application Version", _version);
            _telemetryClient.Context.User.Id = _userName;
            _telemetryClient.Context.User.UserAgent = _application;
            _telemetryClient.Context.Component.Version = _version;

            _telemetryClient.Context.Session.Id = _sessionKey;

        }

        /// <summary>
        /// Flush the TelemetryClient data to the server
        /// Gives a message so user knows the data has been sent
        /// </summary>
        private void DoDataFlush()
        {
            _telemetryClient.Flush();
        }

        /// <summary>
        /// Gathers the details used to add details to our telemetry data
        /// </summary>
        private void GatherDetails()
        {
            _sessionKey = Guid.NewGuid().ToString();
            _userName = Environment.UserName;

            _version = $"v.{ Assembly.GetEntryAssembly().GetName().Version}";
            _application = $"{ Assembly.GetEntryAssembly().GetName().Name} {_version}";
        }


    }
}