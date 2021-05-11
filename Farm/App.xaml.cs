using Farm.Views;
using System;
using System.IO;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Farm
{
    public partial class App : Application
    {
        public static string FolderPath { get; private set; }
        public static string AppDataPath { get; private set; }

        public App()
        {
            InitializeComponent();
            FolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData));

            AppDataPath = Path.Combine(FileSystem.AppDataDirectory, "text.txt");
            MainPage = new LoadingPage();

        }

        protected async override void OnStart()
        {
            await ((LoadingPage)MainPage).UpdateData();
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
