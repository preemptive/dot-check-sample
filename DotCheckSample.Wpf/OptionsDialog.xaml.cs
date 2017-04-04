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
using System.Windows;

namespace DotCheckSample.Wpf
{
    public partial class OptionsDialog : Window
    {
        object btnOptionsTweetContent;

        public OptionsDialog()
        {
            InitializeComponent();
            btnOptionsTweetContent = btnOptionsTweet.Content;
        }

        private void btnOptionsClose_Click(object sender, RoutedEventArgs e)
        {
            System.Environment.Exit(0);
        }

        private async void btnOptionsTweet_Click(object sender, RoutedEventArgs e)
        {
            btnOptionsTweet.Content = "Tweeting...";
            await ServiceHelper.SendTweet();
            btnOptionsTweet.Content = btnOptionsTweetContent;
            CloseDialogFromClickEvent();
        }


        private void btnOptionsMalware_Click(object sender, RoutedEventArgs e)
        {
            CloseDialogFromClickEvent();
        }

        private void btnOptionsLucky_Click(object sender, RoutedEventArgs e)
        {
            CloseDialogFromClickEvent();
        }


        private void CloseDialogFromClickEvent()
        {
            DialogResult = true;
            Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //if ( !DialogResult.HasValue || !DialogResult.Value)
            //{
            //    System.Environment.Exit(0);
            //}
        }

    }
}
