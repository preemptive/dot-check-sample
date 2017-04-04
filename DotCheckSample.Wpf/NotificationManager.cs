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

using System.Collections.Generic;
using DotCheckSample.Wpf;

namespace DotCheckSample
{
    public static class NotificationManager
    {
        private static ApplicationInsightHelper aiClient;

        private static NotificationSelection notifications;

        // Set your Application Insights Instrumentation Key here.
        // For instructions on setting up Application Insights and getting a key, see here: https://docs.microsoft.com/en-us/azure/application-insights/app-insights-create-new-resource
        private static string aiKey = null; // e.g. "11111111-2222-3333-4444-555555555555"

        internal static NotificationSelection GetNotificationSelection()
        {
            if (notifications == null)
            {
                notifications = new NotificationSelection();
            }
            return notifications;

        }

        private static ApplicationInsightHelper GetAIClient()
        {
            if (aiClient == null)
                aiClient = new ApplicationInsightHelper(aiKey);

            return aiClient;
        }

        internal static void ReportDebugging()
        {
            NotificationSelection notifications = GetNotificationSelection();

            if (notifications.ApplicationInsights)
            {
                var properties = new Dictionary<string, string> { { "license", DotCheckSample.Wpf.App.LicenseKey }, { "department", DotCheckSample.Wpf.App.Department } };
                GetAIClient().TrackEvent("DebugDetected", properties);
                GetAIClient().FlushData();
            }

        }

        internal static bool IsAIKeySet
        {
            get
            {
                return !string.IsNullOrEmpty(aiKey);
            }
        }
    }
}
