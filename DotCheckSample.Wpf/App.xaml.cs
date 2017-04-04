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
using System.IO;
using System.Windows;

namespace DotCheckSample.Wpf
{
    public partial class App : Application
    {
        private static Random _rand = new Random();

        private static string[] _keys = { "ABC-123", "XYZ-432", "EFG-234", "HIJ-012", "YUI-765", "ITI-912", "LKL-876", "TRE-432", "LIK-101", "TRE-111" };
        private static string[] _userNames = { "Sue", "Josh", "Fred", "Nathan", "Bill", "Mark", "Gabe", "Sebastian", "Joe", "Pat", "Cindy", "Laura", "Emily" };

        private static string[] _departments = { "Executive Management", "Human Resources", "IT Operations", "Sales", "Marketing", "Support", "Finance", "VIP", "Evaluation" };
        private static string _department = _departments[_rand.Next(1, _departments.Length)];
        private static string _userName = _userNames[_rand.Next(0, _userNames.Length)];
        private static string _licenseKey = GetLicenseKey();

        private static bool _isExpired = false;
        
        internal const string DATFILENAME = "dotchecksample.dat";

        private static void CE_DebugAction(bool debuggerPresent)
        {
            _isExpired = IsBricked();
        }

        internal static string GetLicenseKey()
        {
            return _keys[_rand.Next(0, _keys.Length - 1)];
        }

        internal static bool IsExpired()
        {
            return _isExpired;
        }

        public static string LicenseKey
        {
            get
            {
                return _licenseKey;
            }
        }

        public static string UserName
        {
            get
            {
                return _userName;
            }
        }
        public static string Department
        {
            get {
                return _department;
            }

        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
        }

        private static string GetToken()
        {
            if (!IsBricked())
                return "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";
            else
                return "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";
        }

        private static bool IsBricked()
        {
            return File.Exists( Path.Combine(Environment.GetFolderPath( Environment.SpecialFolder.CommonApplicationData), DATFILENAME ));
        }

    }
}
