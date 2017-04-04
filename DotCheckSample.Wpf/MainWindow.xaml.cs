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
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace DotCheckSample.Wpf
{
    public partial class MainWindow : Window
    {
        private string[] _reasons={"Client Dinner","Purchase Airfare","Prepay Hotel","Register for Conference"};

        object submitButtonContent;

        public MainWindow()
        {
            InitializeComponent();

            if ( !IsUIEnabled )
                txtDescription.Text = "It looks like something went wrong. The application is disabled. Please contact support.";  

            this.DataContext = this;
            NotificationSelection notifications = NotificationManager.GetNotificationSelection();

            // set PreEmptive Analytics to true all the time, because in this example, Dotfuscator will inject the code to do the notifications.
            notifications.PreEmptiveAnalytics = true;
            this.chkPA.DataContext = notifications;
            this.chkPA.IsEnabled = false;

            // Enable the Application Insights checkbox only if there is an instrumentation key provided. 
            // If you have a key, add it in the NotificationManager class.
            this.chkAI.DataContext = notifications;
            this.chkAI.IsEnabled = NotificationManager.IsAIKeySet;

            this.licenseKeylbl.Content = Wpf.App.LicenseKey;
            this.departmentlbl.Content = Wpf.App.Department;
            this.userNamelbl.Content = Wpf.App.UserName;

            this.expenseReason.ItemsSource=_reasons;

            submitButtonContent = btnSubmit.Content;

        }

        public bool IsUIEnabled
        {
            get
            {
                return !DotCheckSample.Wpf.App.IsExpired();
            }
        }

        public Visibility UIVisibility
        {
            get
            {
                return Wpf.App.IsExpired() ? Visibility.Collapsed : Visibility.Visible;
            }
        }

        private string _lastTab;

        private void tabCtrl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.Source is TabControl)
            {
                if (!string.IsNullOrEmpty(_lastTab))
                {
                    InjectADebugCheckHere();
                }
                _lastTab = ((TabItem)((TabControl)e.Source).SelectedItem).Tag as string;
            }
        }

        private void InjectADebugCheckHere() {
            
        }

        private bool showOptionsDialog = true;

        private void DebugNotification(bool isDebugging)
        {
            if (isDebugging)
            {
                NotificationManager.ReportDebugging();

                try
                {
                    File.Create(System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), DotCheckSample.Wpf.App.DATFILENAME));
                }
                catch { }

                if (showOptionsDialog)
                {
                    showOptionsDialog = false;

                    OptionsDialog dlg = new OptionsDialog();

                    dlg.Owner = this;

                    bool? dialogResult = dlg.ShowDialog();

                }
                
            }
        }

        private void DisableSubmit( bool isDebugging )
        {
            btnSubmit.IsEnabled = !isDebugging;
        }

        private async void Submit_Click(object sender, RoutedEventArgs e)
        {
            var strAmt = expenseAmount.Text;
            decimal amt;
            if (!decimal.TryParse(strAmt, System.Globalization.NumberStyles.Currency, System.Globalization.CultureInfo.CurrentCulture, out amt))
            {

                expenseAmount.Text=string.Empty;
                MessageBox.Show( this, "You must enter a valid number.","Invalid Amount");
                return;
            }
            
            btnSubmit.Content = "Waiting...";
            string response = await ServiceHelper.SendExpense(amt, (string)expenseReason.SelectedValue);
            if ( !String.IsNullOrEmpty(response) )
            {
                MessageBox.Show(this, response, "Expense Request");
            }
            btnSubmit.Content = submitButtonContent;
        }

        private void Hyperlink_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }
    }
}
