using Prism.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Test_Q3
{
    public class MainWindowViewModel
    {
        public DelegateCommand MyDelegate { get; set; }
        private bool IsProcessOver;
        public Results Results { get; set; }        
        
        public string Url { get; set; }

        public MainWindowViewModel()
        {
            MyDelegate = new DelegateCommand(ExecuteCommand, CanExecuteMethod);
            IsProcessOver = true;
            Results = new Results() { Result= "Please wait..." };
            
            Task.Run(() =>
            {
                while (true)
                {
                    MyDelegate.RaiseCanExecuteChanged(); // go check the enable/disable
                    Thread.Sleep(500);
                }

            });
        }

        // =========================== delegate
        private bool CanExecuteMethod()
        {
            return IsProcessOver;
        }

        private async void ExecuteCommand()
        {
            IsProcessOver = false;
            string text;
            await Task.Run(() =>
            {
                using (WebClient client = new WebClient())
                {
                    text = client.DownloadString(Url);
                }

                int size = System.Text.ASCIIEncoding.ASCII.GetByteCount(text);

                Results.Result = $"The size is: {size} bytes";
            });          
            IsProcessOver = true;
        }
    }
}
