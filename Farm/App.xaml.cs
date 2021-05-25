using OganicInput.Models;
using OganicInput.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace OganicInput
{
    public partial class App : Application
    {
        public static string FolderPath { get; private set; }
        public static string AppDataPath { get; private set; }
        public static string HistoryPath { get; private set; }

        public static IList<DisclosureInfomation> DisInfo { get; set; }
        public static IList<DisclosureInfomation> DisSearchInfo { get; set; }

        public static bool IsFirstLoad { get; set; } = true;
        public App()
        {
            InitializeComponent();
            FolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData));

            AppDataPath = Path.Combine(FileSystem.AppDataDirectory, "data.json");
            HistoryPath = Path.Combine(FileSystem.AppDataDirectory, "data_history.json");

            MainPage = new AppShell();
           
        }

        protected override void OnStart()
        {
            //await ((LoadingPage)MainPage).InitDataSetting();
        }

        protected override void OnSleep()
        {

        }

        protected override void OnResume()
        {

        }

       
    }
}
