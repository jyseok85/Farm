using System;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Farm.Views
{
    public partial class AboutPageSample : ContentPage
    {
        public AboutPageSample()
        {
            InitializeComponent();
        }

        async void OnButtonClicked(object sender, EventArgs e)
        {
            // Launch the specified URL in the system browser.
            await Launcher.OpenAsync("https://aka.ms/xamarin-quickstart");
        }
    }
}