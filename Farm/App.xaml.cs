using Farm.Models;
using Farm.Views;
using System;
using System.Collections.Generic;
using System.IO;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Farm
{
    public partial class App : Application
    {
        public static string FolderPath { get; private set; }
        public static string AppDataPath { get; private set; }

        public static List<DisclosureInfomation> DisInfo { get; set; }

        public App()
        {
            InitializeComponent();
            FolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData));

            AppDataPath = Path.Combine(FileSystem.AppDataDirectory, "data.json");
            MainPage = new LoadingPage();

        }

        protected async override void OnStart()
        {
            await ((LoadingPage)MainPage).InitDataSetting();
            MainPage = new AppShell();

        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {

        }
    }
}
